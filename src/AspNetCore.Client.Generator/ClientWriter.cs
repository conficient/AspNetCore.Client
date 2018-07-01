﻿using AspNetCore.Client.Core;
using AspNetCore.Client.Generator.Data;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Client.Generator
{
	public static class ClientWriter
	{
		internal static string Namespace
		{
			get
			{
				return $"{Path.GetFileName(Settings.RouteToServiceProjectFolder)}.Clients";
			}
		}

		private static string GetInstaller(IList<ParsedFile> parsedFiles)
		{
			var clients = string.Join(Environment.NewLine, parsedFiles.SelectMany(x => x.Classes.Where(y => y.NotEmpty).Select(y => $@"			services.AddScoped<I{y.ClientName}, {y.ClientName}>();")));

			return $@"
	public static class {Settings.ClientInterfaceName}Installer
	{{
		/// <summary>
		/// Register the autogenerated clients into the container with a lifecycle of scoped.
		/// </summary>
		/// <param name=""services""></param>
		/// <param name=""baseAddress"">Address to be used inside the HttpClient injected</param>
		/// <returns></returns>
		public static IServiceCollection InstallClients(this IServiceCollection services, string baseAddress = null)
		{{
			services.AddScoped<{Settings.ClientInterfaceName}>((provider) =>
			{{
				var client = provider.GetService<HttpClient>();
				if(baseAddress != null)
				{{
					client.BaseAddress = new Uri(baseAddress);
				}}
				return new {Settings.ClientInterfaceName}(client);
			}});

{clients}
			return services;
		}}
	}}
";
		}

		public static string GetIncludePreHttpCheck()
		{
			return $@"

	public class {Constants.HttpOverrideClass} : {Constants.HttpOverride}
	{{
		public async {Helpers.GetTaskType()}<{nameof(HttpResponseMessage)}> {Constants.HttpOverrideGetMethod}({nameof(String)} {Constants.UrlVariable}, {nameof(CancellationToken)} {Constants.CancellationTokenParameter} = default({nameof(CancellationToken)}))
		{{
			return await {nameof(Task)}.{nameof(Task.FromResult)}<{nameof(HttpResponseMessage)}>(null);
		}}

		public async {nameof(Task)} {Constants.HttpOverrideOnNonOverridedResponse}({nameof(String)} {Constants.UrlVariable}, {nameof(HttpResponseMessage)} {Constants.ResponseVariable}, {nameof(CancellationToken)} {Constants.CancellationTokenParameter} = default({nameof(CancellationToken)}))
		{{
			await {nameof(Task)}.{nameof(Task.CompletedTask)};
		}}
	}}

	public interface {Constants.HttpOverride}
	{{
		{Helpers.GetTaskType()}<{nameof(HttpResponseMessage)}> {Constants.HttpOverrideGetMethod}({nameof(String)} {Constants.UrlVariable}, {nameof(CancellationToken)} {Constants.CancellationTokenParameter} = default({nameof(CancellationToken)}));
		{nameof(Task)} {Constants.HttpOverrideOnNonOverridedResponse}({nameof(String)} {Constants.UrlVariable}, {nameof(HttpResponseMessage)} {Constants.ResponseVariable}, {nameof(CancellationToken)} {Constants.CancellationTokenParameter} = default({nameof(CancellationToken)}));
	}}
";
		}

		private static string GetServiceClients()
		{
			return $@"

	public class {Settings.ClientInterfaceName}
	{{
		public readonly {nameof(FlurlClient)} {Constants.FlurlClientVariable};

		public {Settings.ClientInterfaceName}({nameof(HttpClient)} client)
		{{
			{Constants.FlurlClientVariable} = new {nameof(FlurlClient)}(client);
		}}
	}}

	public interface I{Settings.ClientInterfaceName} : {nameof(IClient)} {{ }}";
		}

		public static void WriteClientsFile(IList<ParsedFile> parsedFiles)
		{
			IList<string> requiredUsingStatements = new List<string>
			{
				"using System;",
				"using System.Collections.Generic;",
				"using System.Linq;",
				"using System.Net;",
				"using System.Threading.Tasks;",
				"using System.Threading.Tasks;",
				"using System.Net.Http;",
				"using Flurl.Http;",
				"using Flurl;",
				"using System.Runtime.CompilerServices;",
				"using AspNetCore.Client.Core;",
				"using AspNetCore.Client.Core.Authorization;",
				"using AspNetCore.Client.Core.Exceptions;",
				"using Microsoft.Extensions.DependencyInjection;",
				"using System.Threading;"
			};

			//if (Settings.Instance.BlazorClients)
			//{
			//	requiredUsingStatements.Add("using Microsoft.AspNetCore.Blazor;");
			//}
			//else
			//{
			requiredUsingStatements.Add("using Newtonsoft.Json;");
			//}

			var distinctUsingStatements = parsedFiles
											.SelectMany(x => x.UsingStatements)
											.Union(requiredUsingStatements)
											.Distinct()
											.ToArray();

			string usingBlock = string.Join(Environment.NewLine, distinctUsingStatements);


			var namespaceGroupings = parsedFiles.SelectMany(x => x.Classes).Where(x => x.NotEmpty).GroupBy(x => x.NamespaceVersion);

			List<string> blocks = new List<string>();

			foreach (var group in namespaceGroupings)
			{
				var groupNamespace =
$@"
{string.Join(Environment.NewLine, group.Select(x => x.GetText()))}
";
				blocks.Add(groupNamespace);
			}


			string str =
$@"//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated from a template.
//		Manual changes to this file may cause unexpected behavior in your application.
//		Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

{usingBlock}

namespace {Settings.ClientNamespace}
{{

{GetInstaller(parsedFiles)}
{GetIncludePreHttpCheck()}
{GetServiceClients()}
{string.Join(Environment.NewLine, blocks)}
}}
";

			Helpers.SafelyWriteToFile($"{Environment.CurrentDirectory}/Clients.cs", str);

		}

	}
}

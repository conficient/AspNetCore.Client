//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated from a template.
//		Manual changes to this file may cause unexpected behavior in your application.
//		Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using TestBlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Flurl.Http;
using Flurl;
using System.Runtime.CompilerServices;
using AspNetCore.Client.Core;
using AspNetCore.Client.Core.Authorization;
using AspNetCore.Client.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Newtonsoft.Json;

namespace TestBlazorApp.Clients
{


	public static class TestBlazorAppClientInstaller
	{
		/// <summary>
		/// Register the autogenerated clients into the container with a lifecycle of scoped.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="baseAddress">Address to be used inside the HttpClient injected</param>
		/// <returns></returns>
		public static IServiceCollection InstallClients(this IServiceCollection services, string baseAddress = null)
		{
			services.AddScoped<TestBlazorAppClient>((provider) =>
			{
				var client = provider.GetService<HttpClient>();
				if(baseAddress != null)
				{
					client.BaseAddress = new Uri(baseAddress);
				}
				return new TestBlazorAppClient(client);
			});

			services.AddScoped<ISampleDataClient, SampleDataClient>();
			return services;
		}
	}



	public class DefaultHttpOverride : IHttpOverride
	{
		public async ValueTask<HttpResponseMessage> GetResponseAsync(String url, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await Task.FromResult<HttpResponseMessage>(null);
		}

		public async Task OnNonOverridedResponseAsync(String url, HttpResponseMessage response, CancellationToken cancellationToken = default(CancellationToken))
		{
			await Task.CompletedTask;
		}
	}

	public interface IHttpOverride
	{
		ValueTask<HttpResponseMessage> GetResponseAsync(String url, CancellationToken cancellationToken = default(CancellationToken));
		Task OnNonOverridedResponseAsync(String url, HttpResponseMessage response, CancellationToken cancellationToken = default(CancellationToken));
	}



	public class TestBlazorAppClient
	{
		public readonly FlurlClient ClientWrapper;

		public TestBlazorAppClient(HttpClient client)
		{
			ClientWrapper = new FlurlClient(client);
		}
	}

	public interface ITestBlazorAppClient : IClient { }



	public interface ISampleDataClient : ITestBlazorAppClient
	{
		
		IEnumerable<WeatherForecast> WeatherForecasts(Action<HttpResponseMessage> ResponseCallback = null, 
			CancellationToken cancellationToken = default(CancellationToken));

		
		HttpResponseMessage WeatherForecastsRaw(CancellationToken cancellationToken = default(CancellationToken));

		
		ValueTask<IEnumerable<WeatherForecast>> WeatherForecastsAsync(Action<HttpResponseMessage> ResponseCallback = null, 
			CancellationToken cancellationToken = default(CancellationToken));

		
		ValueTask<HttpResponseMessage> WeatherForecastsRawAsync(CancellationToken cancellationToken = default(CancellationToken));

	}


	public class SampleDataClient : ISampleDataClient
	{
		public readonly TestBlazorAppClient Client;
		public readonly IHttpOverride HttpOverride;

		public SampleDataClient(TestBlazorAppClient client, IHttpOverride httpOverride)
		{
			Client = client;
			HttpOverride = httpOverride;
		}


		public IEnumerable<WeatherForecast> WeatherForecasts(Action<HttpResponseMessage> ResponseCallback = null, 
			CancellationToken cancellationToken = default(CancellationToken))
		{

			
			var controller = "SampleData";
			var action = "WeatherForecasts";

			string url = $@"api/{controller}/{action}";
			HttpResponseMessage response = null;
			response = HttpOverride.GetResponseAsync(url, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
			if(response == null)
			{
				response = Client.ClientWrapper
				.Request(url)
				.AllowAnyHttpStatus()
				.GetAsync(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
				
				HttpOverride.OnNonOverridedResponseAsync(url, response, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
			}

			if(ResponseCallback != null && ResponseCallback.Method.IsDefined(typeof(AsyncStateMachineAttribute), true))
			{
				throw new NotSupportedException("Async void action delegates for ResponseCallback are not supported. As they will run out of the scope of this call.");
			}
			ResponseCallback?.Invoke(response);
			
			if(response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult());
			}
			else
			{
				return default(IEnumerable<WeatherForecast>);
			}

		}


		public HttpResponseMessage WeatherForecastsRaw(CancellationToken cancellationToken = default(CancellationToken))
		{

			
			var controller = "SampleData";
			var action = "WeatherForecasts";

			string url = $@"api/{controller}/{action}";
			HttpResponseMessage response = null;
			response = HttpOverride.GetResponseAsync(url, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
			if(response == null)
			{
				response = Client.ClientWrapper
				.Request(url)
				.AllowAnyHttpStatus()
				.GetAsync(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
				
				HttpOverride.OnNonOverridedResponseAsync(url, response, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
			}

			return response;
		}


		public async ValueTask<IEnumerable<WeatherForecast>> WeatherForecastsAsync(Action<HttpResponseMessage> ResponseCallback = null, 
			CancellationToken cancellationToken = default(CancellationToken))
		{

			
			var controller = "SampleData";
			var action = "WeatherForecasts";

			string url = $@"api/{controller}/{action}";
			HttpResponseMessage response = null;
			response = await HttpOverride.GetResponseAsync(url, cancellationToken).ConfigureAwait(false);
			if(response == null)
			{
				response = await Client.ClientWrapper
				.Request(url)
				.AllowAnyHttpStatus()
				.GetAsync(cancellationToken).ConfigureAwait(false);
				
				await HttpOverride.OnNonOverridedResponseAsync(url, response, cancellationToken).ConfigureAwait(false);
			}

			if(ResponseCallback != null && ResponseCallback.Method.IsDefined(typeof(AsyncStateMachineAttribute), true))
			{
				throw new NotSupportedException("Async void action delegates for ResponseCallback are not supported. As they will run out of the scope of this call.");
			}
			ResponseCallback?.Invoke(response);
			
			if(response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
			}
			else
			{
				return default(IEnumerable<WeatherForecast>);
			}

		}


		public async ValueTask<HttpResponseMessage> WeatherForecastsRawAsync(CancellationToken cancellationToken = default(CancellationToken))
		{

			
			var controller = "SampleData";
			var action = "WeatherForecasts";

			string url = $@"api/{controller}/{action}";
			HttpResponseMessage response = null;
			response = await HttpOverride.GetResponseAsync(url, cancellationToken).ConfigureAwait(false);
			if(response == null)
			{
				response = await Client.ClientWrapper
				.Request(url)
				.AllowAnyHttpStatus()
				.GetAsync(cancellationToken).ConfigureAwait(false);
				
				await HttpOverride.OnNonOverridedResponseAsync(url, response, cancellationToken).ConfigureAwait(false);
			}

			return response;
		}

	}


}

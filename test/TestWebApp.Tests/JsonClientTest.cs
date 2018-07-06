using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestWebApp.Clients;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using TestWebApp.Contracts;
using AspNetCore.Client.Core;
using NUnit.Framework;

namespace TestWebApp.Tests
{
	[TestFixture]
	public class JsonClientTest
	{
		[Test]
		public void GetTest()
		{
			var endpoint = new JsonServerInfo();

			var valuesClient = endpoint.Provider.GetService<IValuesClient>();
			var values = valuesClient.Get();


			Assert.AreEqual(new List<string> { "value1", "value2" }, values);


		}

		[Test]
		public void HeaderTestString()
		{
			var endpoint = new JsonServerInfo();

			var valuesClient = endpoint.Provider.GetService<IValuesClient>();
			var value = valuesClient.HeaderTestString("Val1", "Val2");


			Assert.AreEqual("Val1", value);


		}

		[Test]
		public void HeaderTestInt()
		{
			var endpoint = new JsonServerInfo();

			var valuesClient = endpoint.Provider.GetService<IValuesClient>();
			var value = valuesClient.HeaderTestInt(15);


			Assert.AreEqual(15, value);
		}


		[Test]
		public void DtoReturns()
		{
			var endpoint = new JsonServerInfo();

			var valuesClient = endpoint.Provider.GetService<IValuesClient>();
			MyFancyDto dto = null;

			valuesClient.FancyDtoReturn(15,
				OKCallback: (_) =>
				 {
					 dto = _;
				 });


			Assert.AreEqual(15, dto.Id);
		}

		/// <summary>
		/// Microsoft.AspNetCore.TestHost.ClientHandler does not respect the CancellationToken and will always complete a request. Their unit test around it ClientCancellationAbortsRequest has a "hack" that cancels in TestServer when the token is canceled.
		/// When the HttpClient has the default HttpMessageHandler, the SendAsync will cancel approriately, until they match this functionality, this test will be disabled
		/// </summary>
		/// <returns></returns>
		//[Fact]
		public async Task CancelTestAsync()
		{
			var endpoint = new JsonServerInfo();

			var valuesClient = endpoint.Provider.GetService<IValuesClient>();

			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			var token = cancellationTokenSource.Token;

			Assert.ThrowsAsync<FlurlHttpException>(async () =>
			{
				var task = valuesClient.CancellationTestEndpointAsync(cancellationToken: token);
				cancellationTokenSource.CancelAfter(1500);
				await task.ConfigureAwait(false);
			});

			//Assert.True(ex.InnerException is TaskCanceledException);
		}
	}
}
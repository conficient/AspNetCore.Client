//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated from a template.
//		Manual changes to this file may cause unexpected behavior in your application.
//		Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AspNetCore.Client.Authorization;
using AspNetCore.Client.Exceptions;
using AspNetCore.Client.Http;
using AspNetCore.Client.RequestModifiers;
using AspNetCore.Client.Serializers;
using AspNetCore.Client;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading;
using System;
using TestBlazorApp.Shared;

namespace TestBlazorApp.Clients
{




	public static class TestBlazorAppClientInstaller
	{
		/// <summary>
		/// Register the autogenerated clients into the container with a lifecycle of scoped.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configure">Overrides for client configuration</param>
		/// <returns></returns>
		public static IServiceCollection AddTestBlazorClients(this IServiceCollection services, Action<ClientConfiguration> configure)
		{
			var configuration = new ClientConfiguration();

			configuration.RegisterClientWrapperCreator<ITestBlazorAppClient>(TestBlazorAppClientWrapper.Create);
			configuration.UseClientWrapper<ITestBlazorAppClientWrapper, TestBlazorAppClientWrapper>((provider) => new TestBlazorAppClientWrapper(provider.GetService<Func<ITestBlazorAppClient, IFlurlClient>>(), configuration.GetSettings(), provider));

			configure?.Invoke(configuration);

			services.AddScoped<ITestBlazorAppClientRepository,TestBlazorAppClientRepository>();
			services.AddScoped<ISampleDataClient, SampleDataClient>();

			return configuration.ApplyConfiguration<ITestBlazorAppClient>(services);
		}
	}



	public interface ITestBlazorAppClientWrapper : IClientWrapper { }

	public class TestBlazorAppClientWrapper :  ITestBlazorAppClientWrapper
	{
		public TimeSpan Timeout { get; internal set; }
		public IFlurlClient ClientWrapper { get; internal set; }

		public TestBlazorAppClientWrapper(Func<ITestBlazorAppClient,IFlurlClient> client, ClientSettings settings, IServiceProvider provider)
		{
			ClientWrapper = client(null);
			if (settings.BaseAddress != null)
			{
				ClientWrapper.BaseUrl = settings.BaseAddress(provider);
			}

			Timeout = settings.Timeout;
		}

		public static ITestBlazorAppClientWrapper Create(Func<ITestBlazorAppClient,IFlurlClient> client, ClientSettings settings, IServiceProvider provider)
		{
			return new TestBlazorAppClientWrapper(client, settings, provider);
		}
	}

	public interface ITestBlazorAppClient : IClient { }





	public interface ITestBlazorAppClientRepository
	{
		ISampleDataClient SampleData { get; }
	}

	internal class TestBlazorAppClientRepository : ITestBlazorAppClientRepository
	{
		public ISampleDataClient SampleData { get; }

		public TestBlazorAppClientRepository
		(
			ISampleDataClient param_sampledata
		)
		{
			this.SampleData = param_sampledata;
		}
	}





}





namespace TestBlazorApp.Clients
{





	public interface ISampleDataClient : ITestBlazorAppClient
	{

		
		IEnumerable<WeatherForecast> WeatherForecasts
		(
			Action<HttpResponseMessage> ResponseCallback = null,
			IDictionary<String,Object> headers = null,
			IEnumerable<Cookie> cookies = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default
		);

		
		HttpResponseMessage WeatherForecastsRaw
		(
			IDictionary<String,Object> headers = null,
			IEnumerable<Cookie> cookies = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default
		);

		
		ValueTask<IEnumerable<WeatherForecast>> WeatherForecastsAsync
		(
			Action<HttpResponseMessage> ResponseCallback = null,
			IDictionary<String,Object> headers = null,
			IEnumerable<Cookie> cookies = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default
		);

		
		ValueTask<HttpResponseMessage> WeatherForecastsRawAsync
		(
			IDictionary<String,Object> headers = null,
			IEnumerable<Cookie> cookies = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default
		);


	}




	internal class SampleDataClient : ISampleDataClient
	{
		protected readonly ITestBlazorAppClientWrapper Client;
		protected readonly IHttpOverride HttpOverride;
		protected readonly IHttpSerializer Serializer;
		protected readonly IHttpRequestModifier Modifier;

		public SampleDataClient(
			ITestBlazorAppClientWrapper param_client,
			Func<ITestBlazorAppClient,IHttpOverride> param_httpoverride,
			Func<ITestBlazorAppClient,IHttpSerializer> param_serializer,
			Func<ITestBlazorAppClient,IHttpRequestModifier> param_modifier)
		{
			Client = param_client;
			HttpOverride = param_httpoverride(this);
			Serializer = param_serializer(this);
			Modifier = param_modifier(this);
		}



		
		public IEnumerable<WeatherForecast> WeatherForecasts
		(
			Action<HttpResponseMessage> ResponseCallback = null,
			IDictionary<String,Object> headers = null,
			IEnumerable<Cookie> cookies = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default
		)
		{

			var controller = "SampleData";
			var action = "WeatherForecasts";
			string url = $@"api/{controller}/{action}";
			HttpResponseMessage response = null;
			response = HttpOverride.GetResponseAsync(HttpMethod.Get, url, null, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();

			if(response == null)
			{
				response = Client.ClientWrapper
							.Request(url)
							.WithRequestModifiers(Modifier)				
							.WithCookies(cookies)				
							.WithHeaders(headers)				
							.WithTimeout(timeout ?? Client.Timeout)
							.AllowAnyHttpStatus()
							.GetAsync(cancellationToken)
							.ConfigureAwait(false).GetAwaiter().GetResult();

				HttpOverride.OnNonOverridedResponseAsync(HttpMethod.Get, url, null, response, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
			}

			if(ResponseCallback != null && ResponseCallback.Method.IsDefined(typeof(AsyncStateMachineAttribute), true))
			{
				throw new NotSupportedException("Async void action delegates for ResponseCallback are not supported.As they will run out of the scope of this call.");
			}

			ResponseCallback?.Invoke(response);


			if(response.IsSuccessStatusCode)
			{
				return Serializer.Deserialize<IEnumerable<WeatherForecast>>(response.Content).ConfigureAwait(false).GetAwaiter().GetResult();
			}
			else
			{
				return default(IEnumerable<WeatherForecast>);
			}

		}

		
		public HttpResponseMessage WeatherForecastsRaw
		(
			IDictionary<String,Object> headers = null,
			IEnumerable<Cookie> cookies = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default
		)
		{

			var controller = "SampleData";
			var action = "WeatherForecasts";
			string url = $@"api/{controller}/{action}";
			HttpResponseMessage response = null;
			response = HttpOverride.GetResponseAsync(HttpMethod.Get, url, null, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();

			if(response == null)
			{
				response = Client.ClientWrapper
							.Request(url)
							.WithRequestModifiers(Modifier)				
							.WithCookies(cookies)				
							.WithHeaders(headers)				
							.WithTimeout(timeout ?? Client.Timeout)
							.AllowAnyHttpStatus()
							.GetAsync(cancellationToken)
							.ConfigureAwait(false).GetAwaiter().GetResult();

				HttpOverride.OnNonOverridedResponseAsync(HttpMethod.Get, url, null, response, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
			}

			return response;
		}

		
		public async ValueTask<IEnumerable<WeatherForecast>> WeatherForecastsAsync
		(
			Action<HttpResponseMessage> ResponseCallback = null,
			IDictionary<String,Object> headers = null,
			IEnumerable<Cookie> cookies = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default
		)
		{

			var controller = "SampleData";
			var action = "WeatherForecasts";
			string url = $@"api/{controller}/{action}";
			HttpResponseMessage response = null;
			response = await HttpOverride.GetResponseAsync(HttpMethod.Get, url, null, cancellationToken).ConfigureAwait(false);

			if(response == null)
			{
				response = await Client.ClientWrapper
							.Request(url)
							.WithRequestModifiers(Modifier)				
							.WithCookies(cookies)				
							.WithHeaders(headers)				
							.WithTimeout(timeout ?? Client.Timeout)
							.AllowAnyHttpStatus()
							.GetAsync(cancellationToken)
							.ConfigureAwait(false);

				await HttpOverride.OnNonOverridedResponseAsync(HttpMethod.Get, url, null, response, cancellationToken).ConfigureAwait(false);
			}

			if(ResponseCallback != null && ResponseCallback.Method.IsDefined(typeof(AsyncStateMachineAttribute), true))
			{
				throw new NotSupportedException("Async void action delegates for ResponseCallback are not supported.As they will run out of the scope of this call.");
			}

			ResponseCallback?.Invoke(response);


			if(response.IsSuccessStatusCode)
			{
				return await Serializer.Deserialize<IEnumerable<WeatherForecast>>(response.Content).ConfigureAwait(false);
			}
			else
			{
				return default(IEnumerable<WeatherForecast>);
			}

		}

		
		public async ValueTask<HttpResponseMessage> WeatherForecastsRawAsync
		(
			IDictionary<String,Object> headers = null,
			IEnumerable<Cookie> cookies = null,
			TimeSpan? timeout = null,
			CancellationToken cancellationToken = default
		)
		{

			var controller = "SampleData";
			var action = "WeatherForecasts";
			string url = $@"api/{controller}/{action}";
			HttpResponseMessage response = null;
			response = await HttpOverride.GetResponseAsync(HttpMethod.Get, url, null, cancellationToken).ConfigureAwait(false);

			if(response == null)
			{
				response = await Client.ClientWrapper
							.Request(url)
							.WithRequestModifiers(Modifier)				
							.WithCookies(cookies)				
							.WithHeaders(headers)				
							.WithTimeout(timeout ?? Client.Timeout)
							.AllowAnyHttpStatus()
							.GetAsync(cancellationToken)
							.ConfigureAwait(false);

				await HttpOverride.OnNonOverridedResponseAsync(HttpMethod.Get, url, null, response, cancellationToken).ConfigureAwait(false);
			}

			return response;
		}


	}




}




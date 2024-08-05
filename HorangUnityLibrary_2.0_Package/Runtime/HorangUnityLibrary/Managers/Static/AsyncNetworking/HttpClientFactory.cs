using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;

namespace Horang.HorangUnityLibrary.Managers.Static.AsyncNetworking
{
	public static class HttpClientFactory
	{
		public record ClientOptions
		{
			public DecompressionMethods DecompressionMethod { get; set; } = DecompressionMethods.None;
			public SslProtocols SslProtocol { get; set; } = SslProtocols.None;
			public bool UseCookie { get; set; } = true;
			public bool UseDefaultCredential { get; set; } = true;
			public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5.0);
		}
		
		public static HttpClient Create(ClientOptions clientOptions = null)
		{
			clientOptions ??= new ClientOptions();
			
			var handler = new HttpClientHandler();
			handler.AutomaticDecompression = clientOptions.DecompressionMethod;
			handler.SslProtocols = clientOptions.SslProtocol;
			handler.UseCookies = clientOptions.UseCookie;
			handler.CookieContainer = clientOptions.UseCookie ? new CookieContainer() : null;
			handler.UseDefaultCredentials = clientOptions.UseDefaultCredential;

			var client = new HttpClient(handler, true);
			client.Timeout = clientOptions.Timeout;

			return client;
		}
	}
}
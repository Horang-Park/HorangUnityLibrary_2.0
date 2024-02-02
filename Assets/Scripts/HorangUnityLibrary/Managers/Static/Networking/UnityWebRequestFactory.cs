using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Horang.HorangUnityLibrary.Managers.Static.Networking
{
	public static class UnityWebRequestFactory
	{
		/// <summary>
		/// Generate Unity web request with API method GET.
		/// </summary>
		/// <param name="uri">To request uri</param>
		/// <param name="headerParameters">To add header parameters</param>
		/// <returns>Generated Unity web request</returns>
		public static UnityWebRequest Get(string uri, params (string, string)[] headerParameters)
		{
			var www = UnityWebRequest.Get(uri);

			return AddHeader(www, headerParameters);
		}

		/// <summary>
		/// Generate Unity web request with API method POST with json post data type.
		/// </summary>
		/// <param name="uri">To request uri</param>
		/// <param name="postData">To post data in json</param>
		/// <param name="contentType">The type of post data parameter (default "application/json")</param>
		/// <param name="headerParameters">To add header parameters</param>
		/// <returns>Generated Unity web request</returns>
		public static UnityWebRequest Post(string uri, string postData, string contentType = "application/json", params (string, string)[] headerParameters)
		{
			var www = UnityWebRequest.Post(uri, postData, contentType);

			return AddHeader(www, headerParameters);
		}
		
		/// <summary>
		/// Generate Unity web request with API method POST with WWWForm post data type.
		/// </summary>
		/// <param name="uri">To request uri</param>
		/// <param name="postData">To post data in WWWForm</param>
		/// <param name="headerParameters">To add header parameters</param>
		/// <returns>Generated Unity web request</returns>
		public static UnityWebRequest Post(string uri, WWWForm postData, params (string, string)[] headerParameters)
		{
			var www = UnityWebRequest.Post(uri, postData);

			return AddHeader(www, headerParameters);
		}

		/// <summary>
		/// Generate Unity web request with API method POST with IMultipartFormSection list post data type.
		/// </summary>
		/// <param name="uri">To request uri</param>
		/// <param name="postData">To post data in IMultipartFormSection list</param>
		/// <param name="headerParameters">To add header parameters</param>
		/// <returns></returns>
		public static UnityWebRequest Post(string uri, List<IMultipartFormSection> postData, params (string, string)[] headerParameters)
		{
			var www = UnityWebRequest.Post(uri, postData);

			return AddHeader(www, headerParameters);
		}

		/// <summary>
		/// Generate Unity web request with API method PUT.
		/// </summary>
		/// <param name="uri">To request uri</param>
		/// <param name="putData">To put data in byte array</param>
		/// <param name="headerParameters">To add header parameters</param>
		/// <returns>Generated Unity web request</returns>
		public static UnityWebRequest Put(string uri, byte[] putData, params (string, string)[] headerParameters)
		{
			var www = UnityWebRequest.Put(uri, putData);

			return AddHeader(www, headerParameters);
		}

		/// <summary>
		/// Generate Unity web request with API method PUT.
		/// </summary>
		/// <param name="uri">To request uri</param>
		/// <param name="putData">To put data in string</param>
		/// <param name="headerParameters">To add header parameters</param>
		/// <returns>Generated Unity web request</returns>
		public static UnityWebRequest Put(string uri, string putData, params (string, string)[] headerParameters)
		{
			var www = UnityWebRequest.Put(uri, putData);
			
			return AddHeader(www, headerParameters);
		}

		/// <summary>
		/// Generate Unity web request with API method DELETE.
		/// </summary>
		/// <param name="uri">To request uri</param>
		/// <param name="headerParameters">To add header parameters</param>
		/// <returns>Generated unity web request</returns>
		public static UnityWebRequest Delete(string uri, params (string, string)[] headerParameters)
		{
			var www = UnityWebRequest.Delete(uri);

			return AddHeader(www, headerParameters);
		}

		private static UnityWebRequest AddHeader(UnityWebRequest r, params (string, string)[] hP)
		{
			foreach (var p in hP)
			{
				r.SetRequestHeader(p.Item1, p.Item2);
			}

			return r;
		}
	}
}
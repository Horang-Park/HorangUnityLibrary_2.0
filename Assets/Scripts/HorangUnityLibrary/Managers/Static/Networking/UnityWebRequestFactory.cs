using UnityEngine.Networking;

namespace HorangUnityLibrary.Managers.Static.Networking
{
	public static class UnityWebRequestFactory
	{
		public static UnityWebRequest Get(string uri, params (string, string)[] headerParameters)
		{
			var www = UnityWebRequest.Get(uri);

			return AddHeader(www, headerParameters);
		}

		public static UnityWebRequest Post(string uri, string postData, string contentType, params (string, string)[] headerParameters)
		{
			var www = UnityWebRequest.Post(uri, postData, contentType);

			return AddHeader(www, headerParameters);
		}

		public static UnityWebRequest Put(string uri, byte[] putData, params (string, string)[] headerParameters)
		{
			var www = UnityWebRequest.Put(uri, putData);

			return AddHeader(www, headerParameters);
		}

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
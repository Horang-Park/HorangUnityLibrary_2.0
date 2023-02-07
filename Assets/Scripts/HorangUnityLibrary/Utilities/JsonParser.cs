using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Horang.HorangUnityLibrary.Utilities
{
	public struct JsonParser
	{
		private static readonly JsonSerializerSettings Settings = new()
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Error,
			MissingMemberHandling = MissingMemberHandling.Ignore,
			ObjectCreationHandling = ObjectCreationHandling.Auto,
			NullValueHandling = NullValueHandling.Ignore,
			DefaultValueHandling = DefaultValueHandling.Include
		};

		/// <summary>
		/// JSON parsing
		/// </summary>
		/// <param name="json">To parsing JSON data</param>
		/// <param name="onParsingFailure">Calling when parsing failed (code, message)</param>
		/// <typeparam name="T">Parsing template</typeparam>
		/// <returns>Parsed data</returns>
		public static T JsonParsing<T>(string json, Action<long, string> onParsingFailure = null)
		{
			Log.Print($"Original JSON: {json}");
		
			if (string.IsNullOrEmpty(json))
			{
				Log.Print("Trying to parsing empty json.", LogPriority.Error);
				
				onParsingFailure?.Invoke(-1, "Json string is null or empty.");
		
				return default;
			}

			try
			{
				return JsonConvert.DeserializeObject<T>(json, Settings);
			}
			catch (Exception e)
			{
				Log.Print($"An error occurred while parsing json. Message: {e.Message}", LogPriority.Error);
				
				onParsingFailure?.Invoke(e.HResult, e.Message);
				
				return default;
			}
		}

		/// <summary>
		/// JSON array parsing
		/// </summary>
		/// <param name="json">To parsing JSON array data</param>
		/// <param name="onParsingFailure">Calling when parsing failed (code, message)</param>
		/// <typeparam name="T">Parsing template</typeparam>
		/// <returns>Parsed data</returns>
		public static List<T> JsonArrayParsing<T>(string json, Action<long, string> onParsingFailure = null)
		{
			Log.Print($"Original JSON: {json}");

			if (string.IsNullOrEmpty(json))
			{
				Log.Print("Trying to parsing empty json.", LogPriority.Error);
				
				onParsingFailure?.Invoke(-1, "Json string is null or empty.");
		
				return default;
			}

			try
			{
				var jArray = JsonConvert.DeserializeObject(json, Settings) as JArray;

				return (jArray ?? throw new NullReferenceException()).Select(jToken => ((JObject)jToken).ToString()).Select(ArrayElementParsing<T>).ToList();
			}
			catch (Exception e)
			{
				Log.Print($"An error occurred while parsing json. Message: {e.Message}", LogPriority.Error);
				
				onParsingFailure?.Invoke(e.HResult, e.Message);
				
				return default;
			}
		}
		
		private static T ArrayElementParsing<T>(string element)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(element, Settings);
			}
			catch (JsonException e)
			{
				Log.Print($"An error occurred while parsing array json. Message: {e.Message}", LogPriority.Error);

				throw;
			}
		}
	}
}
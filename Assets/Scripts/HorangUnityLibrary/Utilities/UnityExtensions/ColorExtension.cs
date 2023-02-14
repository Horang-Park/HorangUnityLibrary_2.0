using System;
using UnityEngine;

namespace Horang.HorangUnityLibrary.Utilities.UnityExtensions
{
	public struct ColorFormat256
	{
		public int r;
		public int g;
		public int b;
		public int a;
	}
	
	public static class ColorExtension
	{
		private const int Defined255 = (2 << 8) - 1;
		private const float Inverse255 = 1.0f / Defined255; // 0~255
		private const int RequireHexLength = 6;

		/// <summary>
		/// 256 RGBA color to Unity color 0.0~1.0.
		/// </summary>
		/// <param name="color">To convert color</param>
		/// <returns>Converted color</returns>
		public static Color Rgba256ToColor(ColorFormat256 color)
		{
			var rV = Mathf.Clamp(color.r * Inverse255, 0.0f, 1.0f);
			var gV = Mathf.Clamp(color.g * Inverse255, 0.0f, 1.0f);
			var bV = Mathf.Clamp(color.b * Inverse255, 0.0f, 1.0f);
			var aV = Mathf.Clamp(color.a * Inverse255, 0.0f, 1.0f);
			
			return new Color(rV, gV, bV, aV);
		}

		/// <summary>
		/// Unity color to RGBA 0~255.
		/// </summary>
		/// <param name="color">To convert color</param>
		/// <returns>Converted color</returns>
		public static ColorFormat256 ColorToRgba256(Color color)
		{
			var rV = (int)(color.r * Defined255);
			var gV = (int)(color.g * Defined255);
			var bV = (int)(color.b * Defined255);
			var aV = (int)(color.a * Defined255);

			return new ColorFormat256 { r = rV, g = gV, b = bV, a = aV };
		}

		/// <summary>
		/// Web hex color to Unity color 0.0~1.0.
		/// </summary>
		/// <param name="colorHex">To change color hex. must be "#ffffff" or "ffffff"</param>
		/// <returns>If valid colorHex, will return its color. otherwise return clear color</returns>
		public static Color HexToColor(string colorHex)
		{
			colorHex = colorHex.Replace("#", string.Empty);

			if (colorHex.Length.Equals(RequireHexLength) is false)
			{
				Log.Print($"The color hex [{colorHex}] is invalid.", LogPriority.Error);
				
				return Color.clear;
			}

			var rV = Convert.ToInt32(colorHex[..2], 16);
			var gV = Convert.ToInt32(colorHex[2..4], 16);
			var bV = Convert.ToInt32(colorHex[4..], 16);

			return Rgba256ToColor(new ColorFormat256 { r = rV, g = gV, b = bV, a = 255 });
		}
	}
}
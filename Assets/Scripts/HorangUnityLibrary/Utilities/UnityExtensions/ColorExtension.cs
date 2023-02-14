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
		private const float Inverse255 = 1.0f / 255; // 0~255
		private const int RequireHexLength = 6;

		/// <summary>
		/// 256 RGBA color to Unity color 0.0~1.0.
		/// </summary>
		/// <param name="color">To convert color</param>
		/// <returns>Converted color</returns>
		public static Color Rgba256ToColor(ColorFormat256 color)
		{
			var rV = color.r * Inverse255;
			var gV = color.g * Inverse255;
			var bV = color.b * Inverse255;
			var aV = color.a * Inverse255;
			
			return new Color(rV, gV, bV, aV);
		}

		/// <summary>
		/// Unity color to RGBA 0~255
		/// </summary>
		/// <param name="color">To convert color</param>
		/// <returns>Converted color</returns>
		public static ColorFormat256 ColorToRgba256(Color color)
		{
			var rV = (int)(color.r * 255);
			var gV = (int)(color.g * 255);
			var bV = (int)(color.b * 255);
			var aV = (int)(color.a * 255);

			return new ColorFormat256 { r = rV, g = gV, b = bV, a = aV };
		}

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
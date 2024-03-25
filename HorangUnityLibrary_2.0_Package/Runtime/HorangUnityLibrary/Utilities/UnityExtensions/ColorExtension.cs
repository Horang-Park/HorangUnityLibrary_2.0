using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
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
		private const int Defined255 = (2 << 7) - 1;
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
		/// <param name="alpha">Color's alpha</param>
		/// <returns>If valid colorHex, will return its color. otherwise return clear color</returns>
		public static Color HexToColor(string colorHex, int alpha = 255)
		{
			colorHex = colorHex.Replace("#", string.Empty);

			if (colorHex.Length.Equals(RequireHexLength) is false)
			{
				Log.Print($"The color hex [{colorHex}] is invalid.", LogPriority.Error);
				
				return Color.clear;
			}

			var regex = new Regex(@"[a-fA-F0-9]");
			var rV = regex.IsMatch(colorHex[..2]) ? Convert.ToInt32(colorHex[..2], 16) : 0;
			var gV = regex.IsMatch(colorHex[2..4]) ? Convert.ToInt32(colorHex[2..4], 16) : 0;
			var bV = regex.IsMatch(colorHex[4..]) ? Convert.ToInt32(colorHex[4..], 16) : 0;

			return Rgba256ToColor(new ColorFormat256 { r = rV, g = gV, b = bV, a = alpha });
		}

		/// <summary>
		/// Change Unity color to web hex color.
		/// </summary>
		/// <param name="color">To change color to hex</param>
		/// <returns>Converted hex color</returns>
		public static string ColorToHex(Color color)
		{
			var r = Convert.ToString((int)(color.r * 255), 16);
			var g = Convert.ToString((int)(color.g * 255), 16);
			var b = Convert.ToString((int)(color.b * 255), 16);

			return new StringBuilder().Append(r).Append(g).Append(b).ToString();
		}

		public static Color SetR(this Color color, float red)
		{
			var c = color;
			c.r = Mathf.Clamp01(red);

			return c;
		}
		
		public static Color SetG(this Color color, float green)
		{
			var c = color;
			c.g = Mathf.Clamp01(green);

			return c;
		}
		
		public static Color SetB(this Color color, float blue)
		{
			var c = color;
			c.b = Mathf.Clamp01(blue);

			return c;
		}
	}
}
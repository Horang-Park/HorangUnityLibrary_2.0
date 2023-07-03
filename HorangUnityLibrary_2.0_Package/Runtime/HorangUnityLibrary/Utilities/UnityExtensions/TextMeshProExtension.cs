using TMPro;

namespace Horang.HorangUnityLibrary.Utilities.UnityExtensions
{
	public static class TextMeshProExtension
	{
		public enum TagType
		{
			Bold,
			Italic,
			Underline,
			Strikethrough,
			Superscript,
			Subscript,
			Mark,
		}

		private static readonly string[] openTags =
		{
			"<b>",
			"<i>",
			"<u>",
			"<s>",
			"<sup>",
			"<sub>",
			"<mark>",
		};

		private static readonly string[] closeTags =
		{
			"</b>",
			"</i>",
			"</u>",
			"</s>",
			"</sup>",
			"</sub>",
			"</mark>",
		};

		public static void SetTextWithFormat(this TMP_Text component, string text, TagType tagType) =>
			component.text = $"{openTags[(int)tagType]}{text}{closeTags[(int)tagType]}";
	}
}
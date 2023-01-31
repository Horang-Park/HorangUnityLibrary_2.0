using System;
using System.Collections.Generic;
using HorangUnityLibrary.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace HorangUnityLibrary.Managers.NumberImageFont
{
	public class Digit : MonoBehaviour
	{
		[SerializeField]
		private List<Sprite> numberImageFonts;

		private SpriteRenderer spriteRenderer;
		private Image image;

		private const int RequireNumberFontCount = 10;

		public int Value
		{
			set
			{
				if (value is > 9 or < 0)
				{
					Log.Print("The range of value is must be 0~9", LogPriority.Error);

					return;
				}
				
				if (spriteRenderer is not null || spriteRenderer)
				{
					spriteRenderer.sprite = numberImageFonts[value];
				}
				else if (image is not null || image)
				{
					image.sprite = numberImageFonts[value];
				}
			}
		}

		public Vector2 Bound
		{
			get
			{
				if (spriteRenderer is not null || spriteRenderer)
				{
					return spriteRenderer.size;
				}
				
				if (image is not null || image)
				{
					return new Vector2(image.minWidth, image.minHeight);
				}
				
				return Vector2.zero;
			}
		}

		private void Awake()
		{
			if (ValidationUsable())
			{
				return;
			}
			
			Log.Print($"Cannot use number image font on {gameObject.name} game object.", LogPriority.Error);
		}

		private void Start()
		{
			numberImageFonts.Sort(((spriteLhs, spriteRhs) => string.Compare(spriteRhs.name, spriteLhs.name, StringComparison.CurrentCultureIgnoreCase)));
			numberImageFonts.Reverse();
		}

		private bool ValidationUsable()
		{
			if ((TryGetComponent(out spriteRenderer) 
			    || TryGetComponent(out image))
			    && numberImageFonts.Count == RequireNumberFontCount
			    && GetComponentInParent(typeof(NumberImageFontManager)) is NumberImageFontManager)
			{
				return true;
			}

			return false;
		}
	}
}
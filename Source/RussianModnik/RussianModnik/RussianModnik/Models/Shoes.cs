using System;
using System.Collections.Generic;
using System.Text;

namespace RussianModnik.Models
{
	public class Shoes : ItemBase
	{
		//Represents a universal size (foot length in cm)
		public double Size { get; set; } = 255;

		public string ShoeSizeUS => ShoeSizeHelper.GetShoeSizeUS(Size, this.Gender);
		public string ShoeSizeEU => ShoeSizeHelper.GetShoeSizeEU(Size, this.Gender);
		public string ShoeSizeRu => ShoeSizeHelper.GetShoeSizeRU(Size, this.Gender);

		private static class ShoeSizeHelper
		{
			//Maybe the GetShoeSize... Could somehow be refactored as there is a lot of similar code
			//But shoe size is something rather illogical thus why I kept a unique conversion for each of the size types
			public static string GetShoeSizeUS(double size, Gender gender)
			{
				if (gender == Gender.Female)
				{
					const double BaseSizeMM = 204;
					const double Step = 8;
					const double BaseSizeUs = 3;

					double usSize = (size - BaseSizeMM) / Step + BaseSizeUs;
					if (Math.Truncate(usSize) < 0.3)
						usSize = Math.Floor(usSize);
					else if (Math.Truncate(usSize) > 0.7)
						usSize = Math.Ceiling(usSize);
					else
						usSize = Math.Floor(usSize) + 0.5;

					return "US " + Math.Round(usSize, 1).ToString();
				}
				else if (gender == Gender.Male)
				{
					const double BaseSizeMM = 221;
					const double Step = 8;
					const double BaseSizeUs = 4;

					double usSize = (size - BaseSizeMM) / Step + BaseSizeUs;
					if (Math.Truncate(usSize) < 0.3)
						usSize = Math.Floor(usSize);
					else if (Math.Truncate(usSize) > 0.7)
						usSize = Math.Ceiling(usSize);
					else
						usSize = Math.Floor(usSize) + 0.5;

					return "US " + Math.Round(usSize, 1).ToString();
				}

				return string.Empty;
			}

			public static string GetShoeSizeEU(double size, Gender gender)
			{
				if (gender == Gender.Female)
				{
					const double BaseSizeMM = 204;
					const double Step = 8;
					const double BaseSizeEU = 32.5;

					double euSize = (size - BaseSizeMM) / Step + BaseSizeEU;
					if (Math.Truncate(euSize) < 0.3)
						euSize = Math.Floor(euSize);
					else if (Math.Truncate(euSize) > 0.7)
						euSize = Math.Ceiling(euSize);
					else
						euSize = Math.Floor(euSize) + 0.5;

					return "EU " + Math.Round(euSize, 1).ToString();
				}
				else if (gender == Gender.Male)
				{
					const double BaseSizeMM = 221;
					const double Step = 8;
					const double BaseSizeEU = 36;

					double euSize = (size - BaseSizeMM) / Step + BaseSizeEU;
					if (Math.Truncate(euSize) < 0.3)
						euSize = Math.Floor(euSize);
					else if (Math.Truncate(euSize) > 0.7)
						euSize = Math.Ceiling(euSize);
					else
						euSize = Math.Floor(euSize) + 0.5;

					return "EU " + Math.Round(euSize, 1).ToString();
				}

				return string.Empty;
			}

			public static string GetShoeSizeRU(double size, Gender gender)
			{
				const double BaseSizeMM = 225;
				const double Step = 5;
				const double BaseSizeRu = 36;

				double russianSize = (size - BaseSizeMM) / Step + BaseSizeRu;
				if (Math.Truncate(russianSize) < 0.3)
					russianSize = Math.Floor(russianSize);
				else if (Math.Truncate(russianSize) > 0.7)
					russianSize = Math.Ceiling(russianSize);
				else
					russianSize = Math.Floor(russianSize) + 0.5;

				return "RU " + Convert.ToInt32(russianSize);
			}
		}
	}
}

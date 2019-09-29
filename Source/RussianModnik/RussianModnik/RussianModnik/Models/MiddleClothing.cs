using System;
using System.Collections.Generic;
using System.Text;

namespace RussianModnik.Models
{
	public class MiddleClothing : ItemBase
	{
		//Waist circumference in cm
		public double Size { get; set; }

		public string InternationalSize => SizeHelper.ToInternationalSize(Size, this.Gender);
		public string SizeUS => SizeHelper.ToSizeUS(Size, this.Gender);


		private static class SizeHelper
		{
			private static int ToReadableSize(double d) => Convert.ToInt32(Math.Floor(d));

			public static string ToSizeRussia(double waist, Gender gender)
			{
				const double BaseSizeCm = 71;
				const double BaseSize = 44;
				const double Step = 5;

				return "RU" + ToReadableSize(BaseSize + (waist - BaseSizeCm) / Step);
			}

			public static string ToSizeUS(double waist, Gender gender)
			{
				const double BaseSizeCm = 71;
				const double BaseSize = 34;
				const double Step = 5;

				return "US" + ToReadableSize(BaseSize + (waist - BaseSizeCm) / Step);
			}

			public static string ToInternationalSize(double waist, Gender gender)
			{
				const double BaseSizeCm = 76;
				const double Step = 5;

				return SizeConversionArray[ToReadableSize((waist - BaseSizeCm) / Step)];
			}
		}
	}
}

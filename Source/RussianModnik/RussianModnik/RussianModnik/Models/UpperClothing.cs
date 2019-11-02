using System;
using System.Collections.Generic;
using System.Text;

namespace RussianModnik.Models
{
	public class UpperClothing : ItemBase
	{
		//Represents a universal size (height in cm)
		//Use this for further conversions
		public double Height { get; set; } = 170;

		public string InternationalSize => SizeHelper.ToInternationalSize(Height);
		public string RuSize => SizeHelper.ToSizeRussia(Height);
		public string EuSize => SizeHelper.ToSizeEu(Height);

		private static class SizeHelper
		{
			public static int Floor(double d) => Convert.ToInt32(Math.Floor(d));

			public static string ToInternationalSize(double height)
			{
				const double BaseHeight = 164;
				const double Step = 3;

                int index = Floor((height - BaseHeight) / Step);
                if (index < 0)
                    index = 0;
                return "М\\С " + SizeConversionArray[index]; 
			}

			public static string ToSizeRussia(double height)
			{
				const double BaseHeight = 164;
				const double Step = 3;
				const double BaseSizeRus = 42;

                int index = Floor((height - BaseHeight) / Step);
                if (index < 0)
                    index = 0;
                return "RU " + BaseSizeRus + index;
			}

			public static string ToSizeEu(double height)
			{
				const double BaseHeight = 164;
				const double Step = 3;
				const double BaseSizeEU = 34;

                int index = Floor((height - BaseHeight) / Step);
                if (index < 0)
                    index = 0;
                return "EU " + BaseSizeEU + index;
			}
		}
	}
}

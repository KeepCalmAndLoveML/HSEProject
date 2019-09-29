using System;
using System.Collections.Generic;
using System.Text;

namespace RussianModnik.Models
{
	public class UpperClothing : ItemBase
	{
		public InternationalSize Size { get; set; }

		public string FormattedSize => SizeConversionArray[(int)Size];
	}
}

using System;

namespace FormsPrototype.Models
{
	public enum Gender
	{
		Male,
		Female
	}

	//Represents a minimalised version of a recommendation
	//For the sake of simplicity, this only works with shoes
	public class Item
	{
		//Unique Id
		public string Id { get; set; }

		//Maybe use this as an Id? 
		public string RessourceIdSmall { get; set; }
		public string RessourceIdDetail { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		//Not for rendering
		public Gender Gender { get; set; }

		//Represents a universal size (foot length in mm) that can be converted into any size format
		public double ShoeSize { get; set; }

		//This is used for rendering
		public string ShoeSizeUS => ShoeSizeHelper.GetShoeSizeUS(ShoeSize, this.Gender);
		public string ShoeSizeEU => ShoeSizeHelper.GetShoeSizeEU(ShoeSize, this.Gender);
		public string ShoeSizeAsia => ShoeSizeHelper.GetShoeSizeAsia(ShoeSize, this.Gender);
		public string GenderString => this.Gender == Gender.Male ? "Male" : "Female";

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

					double usSize = ( size - BaseSizeMM ) / Step + BaseSizeUs;
					if(Math.Truncate(usSize) < 0.3)
						usSize = Math.Floor(usSize);
					else if(Math.Truncate(usSize) > 0.7)
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

					double usSize = ( size - BaseSizeMM ) / Step + BaseSizeUs;
					if(Math.Truncate(usSize) < 0.3)
						usSize = Math.Floor(usSize);
					else if(Math.Truncate(usSize) > 0.7)
						usSize = Math.Ceiling(usSize);
					else
						usSize = Math.Floor(usSize) + 0.5;

					return "US " + Math.Round(usSize, 1).ToString();
				}

				return string.Empty;
			}

			public static string GetShoeSizeEU(double size, Gender gender)
			{
				if(gender == Gender.Female)
				{
					const double BaseSizeMM = 204;
					const double Step = 8;
					const double BaseSizeEU = 32.5;

					double euSize = ( size - BaseSizeMM ) / Step + BaseSizeEU;
					if(Math.Truncate(euSize) < 0.3)
						euSize = Math.Floor(euSize);
					else if(Math.Truncate(euSize) > 0.7)
						euSize = Math.Ceiling(euSize);
					else
						euSize = Math.Floor(euSize) + 0.5;

					return "EU " + Math.Round(euSize, 1).ToString();
				}
				else if(gender == Gender.Male)
				{
					const double BaseSizeMM = 221;
					const double Step = 8;
					const double BaseSizeEU = 36;

					double euSize = ( size - BaseSizeMM ) / Step + BaseSizeEU;
					if(Math.Truncate(euSize) < 0.3)
						euSize = Math.Floor(euSize);
					else if(Math.Truncate(euSize) > 0.7)
						euSize = Math.Ceiling(euSize);
					else
						euSize = Math.Floor(euSize) + 0.5;

					return "EU " + Math.Round(euSize, 1).ToString();
				}

				return string.Empty;
			}

			public static string GetShoeSizeAsia(double size, Gender gender)
			{
				if(gender == Gender.Female)
				{
					const double BaseSizeMM = 204;
					const double Step = 8;
					const double BaseSizeAsia = 19;

					double asianSize = ( size - BaseSizeMM ) / Step + BaseSizeAsia;
					if(Math.Truncate(asianSize) < 0.3)
						asianSize = Math.Floor(asianSize);
					else if(Math.Truncate(asianSize) > 0.7)
						asianSize = Math.Ceiling(asianSize);
					else
						asianSize = Math.Floor(asianSize) + 0.5;

					return "Asia " + Math.Round(asianSize, 1).ToString();
				}
				else if(gender == Gender.Male)
				{
					const double BaseSizeMM = 221;
					const double Step = 8;
					const double BaseSizeAsia = 22;

					double asianSize = ( size - BaseSizeMM ) / Step + BaseSizeAsia;
					if(Math.Truncate(asianSize) < 0.3)
						asianSize = Math.Floor(asianSize);
					else if(Math.Truncate(asianSize) > 0.7)
						asianSize = Math.Ceiling(asianSize);
					else
						asianSize = Math.Floor(asianSize) + 0.5;

					return "Asia " + Math.Round(asianSize, 1).ToString();
				}

				return string.Empty;
			}
		}
	}
}
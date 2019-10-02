using System;

namespace RussianModnik.Models
{
	public abstract class ItemBase
	{
		//Convert International size to string
		public static readonly string[] SizeConversionArray = new string[]
		{
			"XS", "S", "M", "L", "XL", "XXL"
		};


		//Unique Id
		public string Id { get; set; }

		public string Title { get; set; } = "Lorem Ipsum";
		public string Description { get; set; } = "Lorem Ipsum";

		//The gender that this item fits too
		public Gender Gender { get; set; } = Gender.Female;

		public string GenderString => this.Gender == Gender.Female ? "Женский пол" : "Мужской пол";

		//Small picture of the item to show in list view
		public string RessourceIdSmall { get; set; }

		//Detailed picture to show in dedicated page
		public string RessourceIdDetail { get; set; }
	}
}
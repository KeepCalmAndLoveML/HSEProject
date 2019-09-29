using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

using RussianModnik.Models;

namespace RussianModnik.Stores
{
	public class UpperClothingStore
	{
		public static UpperClothingStore MainStore;

		private InCodeStore<UpperClothing> MenClothing, WomenClothing;

		public UpperClothingStore()
		{
			var womenItems = new ObservableCollection<UpperClothing>()
			{
				new UpperClothing()
				{
					Id = Guid.NewGuid().ToString(),
					Title = "Толстовка",
					RessourceIdSmall = "BodyTypes.wbt_one.png".ToImageRessourceId(),
					RessourceIdDetail = "BodyTypes.wbt_one.png".ToImageRessourceId(),
				},
			};
			WomenClothing = new InCodeStore<UpperClothing>(womenItems);

			var menItems = new ObservableCollection<UpperClothing>()
			{
				new UpperClothing()
				{
					Id = Guid.NewGuid().ToString(),
					Title = "Толстовка",
					RessourceIdSmall = "BodyTypes.wbt_one.png".ToImageRessourceId(),
					RessourceIdDetail = "BodyTypes.wbt_one.png".ToImageRessourceId(),
				},
			};
			MenClothing = new InCodeStore<UpperClothing>(menItems);
		}

		public IEnumerable<UpperClothing> GetItems(Gender gender = Gender.Female) => gender == Gender.Female ? WomenClothing.GetItems() : MenClothing.GetItems();
	}
}

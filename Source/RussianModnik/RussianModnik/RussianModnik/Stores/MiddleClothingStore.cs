using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using RussianModnik.Models;

namespace RussianModnik.Stores
{
	public class MiddleClothingStore
	{
		public static MiddleClothingStore MainStore;

		private InCodeStore<MiddleClothing> MenClothing, WomenClothing;

		public MiddleClothingStore()
		{
			var womenItems = new ObservableCollection<MiddleClothing>()
			{
				new MiddleClothing()
				{
					Id = Guid.NewGuid().ToString(),
					Title = "Джинсы",
					RessourceIdSmall = "BodyTypes.wbt_one.png".ToImageRessourceId(),
					RessourceIdDetail = "BodyTypes.wbt_one.png".ToImageRessourceId(),
				},
			};
			WomenClothing = new InCodeStore<MiddleClothing>(womenItems);

			var menItems = new ObservableCollection<MiddleClothing>()
			{
				new MiddleClothing()
				{
					Id = Guid.NewGuid().ToString(),
					Title = "Джинсы",
					RessourceIdSmall = "BodyTypes.wbt_one.png".ToImageRessourceId(),
					RessourceIdDetail = "BodyTypes.wbt_one.png".ToImageRessourceId(),
				},
			};
			MenClothing = new InCodeStore<MiddleClothing>(menItems);
		}

		public IEnumerable<MiddleClothing> GetItems(Gender gender = Gender.Female) => gender == Gender.Female ? WomenClothing.GetItems() : MenClothing.GetItems();

	}
}

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
					Title = "Кардиган",
					RessourceIdSmall = "UpperClothing.cardigan.png".ToImageRessourceId(),
					RessourceIdDetail = "UpperClothing.cardigan2.png".ToImageRessourceId(),
				},
				new UpperClothing()
				{
					Id = Guid.NewGuid().ToString(),
					Title = "Платье",
					RessourceIdSmall = "UpperClothing.dress.png".ToImageRessourceId(),
					RessourceIdDetail = "UpperClothing.dress2.png".ToImageRessourceId(),
				},
				new UpperClothing()
				{
					Id = Guid.NewGuid().ToString(),
					Title = "Жакет",
					RessourceIdSmall = "UpperClothing.jacket.png".ToImageRessourceId(),
					RessourceIdDetail = "UpperClothing.jacket2.png".ToImageRessourceId(),
				},
				new UpperClothing()
				{
					Id = Guid.NewGuid().ToString(),
					Title = "Рубашка",
					RessourceIdSmall = "UpperClothing.shirt.png".ToImageRessourceId(),
					RessourceIdDetail = "UpperClothing.shirt2.png".ToImageRessourceId(),
				},
				new UpperClothing()
				{
					Id = Guid.NewGuid().ToString(),
					Title = "Топ",
					RessourceIdSmall = "UpperClothing.top.png".ToImageRessourceId(),
					RessourceIdDetail = "UpperClothing.top2.png".ToImageRessourceId(),
				},
				new UpperClothing()
				{
					Id = Guid.NewGuid().ToString(),
					Title = "Футболка",
					RessourceIdSmall = "UpperClothing.tshirt.png".ToImageRessourceId(),
					RessourceIdDetail = "UpperClothing.tshirt2.png".ToImageRessourceId(),
				},
				new UpperClothing()
				{
					Id = Guid.NewGuid().ToString(),
					Title = "Жилет",
					RessourceIdSmall = "UpperClothing.vest.png".ToImageRessourceId(),
					RessourceIdDetail = "UpperClothing.vest2.png".ToImageRessourceId(),
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

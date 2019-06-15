using System;
using System.Collections.Generic;

using FormsPrototype.Models;

namespace FormsPrototype.ViewModels
{
	public class ItemDetailViewModel : BaseViewModel
	{
		public Item Item { get; set; }
		public List<Item> RelatedItems { get; set; }

		public ItemDetailViewModel(Item item = null)
		{
			Title = item?.Name;
			Item = item;
			
			//TODO: This should get the items from the data store
			RelatedItems = new List<Item>()
			{
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Name = "Nike air max x96 aaa",
					Description = "Just put them on your feet",
					ShoeSize = 260,
					Gender = Gender.Male
				},
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Name = "Adidas ultraboost x123",
					Description ="Soooooooo comfortable",
					ShoeSize = 240,
					Gender = Gender.Female
				},
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Name = "Some Reebook shit",
					Description ="The coolest thing on the market",
					ShoeSize = 275,
					Gender = Gender.Male
				}
			};

		}
	}
}

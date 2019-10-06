using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using RussianModnik.Models;
using RussianModnik.Stores;

namespace RussianModnik.ViewModels
{
	public class MiddleClothingDetailViewModel : BaseViewModel
	{
		public MiddleClothing Item { get; set; }
		public List<MiddleClothing> RelatedItems { get; set; }

		public MiddleClothingDetailViewModel(MiddleClothing item = null)
		{
			Title = item?.Title;
			Item = item;

			GetItems();
		}

		public async void GetItems()
		{
			//TODO: This should get the items from the data store
			RelatedItems = MiddleClothingStore.MainStore.GetItems(Item.Gender).ToList();
		}
	}
}

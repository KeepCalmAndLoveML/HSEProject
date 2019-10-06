using System;
using System.Collections.Generic;
using System.Linq;

using RussianModnik.Models;
using RussianModnik.Stores;

namespace RussianModnik.ViewModels
{
	public class ShoesDetailViewModel : BaseViewModel
	{
		public Shoes Item { get; set; }
		public List<Shoes> RelatedItems { get; set; }

		public ShoesDetailViewModel(Shoes item = null)
		{
			Title = item?.Title;
			Item = item;

			GetItems();
		}

		public async void GetItems()
		{
			//TODO: This should get the items from the data store
			RelatedItems = ShoesStore.MainStore.GetItems(Item.Gender).ToList();
		}
	}
}

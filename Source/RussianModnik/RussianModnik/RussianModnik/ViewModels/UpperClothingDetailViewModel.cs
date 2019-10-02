using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using RussianModnik.Stores;
using RussianModnik.Models;

namespace RussianModnik.ViewModels
{
	public class UpperClothingDetailViewModel : BaseViewModel
	{
		public UpperClothing Item { get; set; }
		public List<UpperClothing> RelatedItems { get; set; }

		public UpperClothingDetailViewModel(UpperClothing item = null)
		{
			Title = item?.Title;
			Item = item;

			GetItems();
		}

		public async void GetItems()
		{
			//TODO: This should get the items from the data store
			RelatedItems = UpperClothingStore.MainStore.GetItems(Item.Gender).ToList();
		}
	}
}

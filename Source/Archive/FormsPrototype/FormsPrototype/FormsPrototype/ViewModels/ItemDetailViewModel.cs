using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

			Task.Run(GetItems);
		}

		public async void GetItems()
		{
			//TODO: This should get the items from the data store
			var ds = new Services.MockDataStore();
			var items = await ds.GetItemsAsync();

			RelatedItems = items.ToList();
		}
	}
}

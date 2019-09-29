using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace RussianModnik.Stores
{
	internal class InCodeStore<T>
	{
		private readonly ObservableCollection<T> Items;

		public InCodeStore(IEnumerable<T> base_items)
		{
			Items = new ObservableCollection<T>(base_items);
		}

		public IEnumerable<T> GetItems() => Items.Select(x => x.Copy());
	}
}

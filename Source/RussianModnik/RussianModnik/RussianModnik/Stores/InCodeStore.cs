using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

using RussianModnik.Models;

namespace RussianModnik.Stores
{
    //T should not be inherited from ItemBase 
    //But this is convenient
    internal class InCodeStore<T> where T : ItemBase
    {
        const double DefaultRating = 2.99;

        private readonly ObservableCollection<T> Items;

        public InCodeStore(IEnumerable<T> base_items)
        {
            Items = new ObservableCollection<T>(base_items);
        }

        public IEnumerable<T> GetItemsPref() => GetItems().Where(x => x.Rating >= DefaultRating || x.Rating == -1);

        public IEnumerable<T> GetItems() => Items.Select(x => x.Copy());
    }
}

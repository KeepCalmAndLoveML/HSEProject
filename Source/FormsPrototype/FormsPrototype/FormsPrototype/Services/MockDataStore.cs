using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormsPrototype.Models;

namespace FormsPrototype.Services
{
	public class MockDataStore : IDataStore<Item>
	{
		List<Item> items;

		public MockDataStore()
		{
			items = new List<Item>();
			var mockItems = new List<Item>
			{
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Name = "Nike air max x96 aaa",
					Description = "Just put them on your feet",
					ShoeSize = 260,
					Gender = Gender.Male,
					RessourceName = "shoes_one.png".ToImageRessourceId()
				},
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Name = "Adidas ultraboost x123",
					Description ="Soooooooo comfortable",
					ShoeSize = 240,
					Gender = Gender.Female,
					RessourceName = "shoes_one.png".ToImageRessourceId()
				},
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Name = "Some Reebook shit",
					Description ="The coolest thing on the market",
					ShoeSize = 275,
					Gender = Gender.Male,
					RessourceName = "shoes_one.png".ToImageRessourceId()
				}
			};

			foreach(var item in mockItems)
			{
				items.Add(item);
			}
		}

		public async Task<bool> AddItemAsync(Item item)
		{
			items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemAsync(Item item)
		{
			var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
			items.Remove(oldItem);
			items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemAsync(string id)
		{
			var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
			items.Remove(oldItem);

			return await Task.FromResult(true);
		}

		public async Task<Item> GetItemAsync(string id)
		{
			return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
		}

		public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
		{
			return await Task.FromResult(items);
		}
	}
}
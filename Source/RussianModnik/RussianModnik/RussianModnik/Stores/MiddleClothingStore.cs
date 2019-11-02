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
                    RessourceIdSmall = "MiddleClothing.jeans.png".ToImageRessourceId(),
                    RessourceIdDetail = "MiddleClothing.jeans2.png".ToImageRessourceId(),
                },
                new MiddleClothing()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Брюки",
                    RessourceIdSmall = "MiddleClothing.pants.png".ToImageRessourceId(),
                    RessourceIdDetail = "MiddleClothing.pants2.png".ToImageRessourceId(),
                },
                new MiddleClothing()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Шорты",
                    RessourceIdSmall = "MiddleClothing.short.png".ToImageRessourceId(),
                    RessourceIdDetail = "MiddleClothing.short2.png".ToImageRessourceId(),
                },
                new MiddleClothing()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Юбка",
                    RessourceIdSmall = "MiddleClothing.skirt.png".ToImageRessourceId(),
                    RessourceIdDetail = "MiddleClothing.skirt2.png".ToImageRessourceId(),
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

        public IEnumerable<MiddleClothing> GetItemsPref(Gender gender = Gender.Female) => gender == Gender.Female ? WomenClothing.GetItemsPref() : MenClothing.GetItemsPref();
        public IEnumerable<MiddleClothing> GetItems(Gender gender = Gender.Female) => gender == Gender.Female ? WomenClothing.GetItems() : MenClothing.GetItems();

    }
}

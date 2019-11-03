using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using RussianModnik.Models;

namespace RussianModnik.Stores
{
    public class ShoesStore
    {
        public static ShoesStore MainStore;

        private InCodeStore<Shoes> MenShoes, WomenShoes;

        public ShoesStore()
        {
            var womenItems = new List<Shoes>()
            {
                new Shoes()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Удобная убовь",
                    RessourceIdSmall = "Shoes.sneakers.png".ToImageRessourceId(),
                    RessourceIdDetail = "Shoes.sneakers2.png".ToImageRessourceId(),
                },
                new Shoes()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Туфли",
                    RessourceIdSmall = "Shoes.heels.png".ToImageRessourceId(),
                    RessourceIdDetail = "Shoes.heels2.png".ToImageRessourceId(),
                }
            };

            WomenShoes = new InCodeStore<Shoes>(womenItems);
        }

        public IEnumerable<Shoes> GetItemsPref(Gender gender = Gender.Female) => gender == Gender.Female ? WomenShoes.GetItemsPref() : MenShoes.GetItemsPref();
        public IEnumerable<Shoes> GetItems(Gender gender = Gender.Female) => gender == Gender.Female ? WomenShoes.GetItems() : MenShoes.GetItems();
    }
}

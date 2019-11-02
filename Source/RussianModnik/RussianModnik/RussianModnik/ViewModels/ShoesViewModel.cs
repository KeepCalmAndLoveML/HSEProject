using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

using RussianModnik.Models;
using RussianModnik.Stores;

namespace RussianModnik.ViewModels
{
    public class ShoesViewModel : BaseViewModel
    {
        public List<Shoes> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ShoesViewModel()
        {
            Title = "Обувь";

            Items = ShoesStore.MainStore.GetItems().ToList();
            LoadItemsCommand = new Command((obj) => ExecuteLoadItemsCommand(obj as Gender?));

            MessagingCenter.Subscribe<ParamsViewModel>(this, "Predictions computed", (ParamsViewModel vm) =>
            {
                LoadItemsCommand.Execute(Gender.Female);
                foreach (var item in Items)
                {

                }
            });
        }

        void ExecuteLoadItemsCommand(Gender? gender)
        {
            if (!gender.HasValue)
                gender = Gender.Female;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = ShoesStore.MainStore.GetItemsPref(gender.Value);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

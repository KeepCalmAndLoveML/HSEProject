using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using RussianModnik.Models;
using RussianModnik.Stores;

using Xamarin.Forms;

namespace RussianModnik.ViewModels
{
    public class MiddleClothingViewModel : BaseViewModel
    {
        public List<MiddleClothing> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public MiddleClothingViewModel()
        {
            Title = "Поясная группа";

            Items = MiddleClothingStore.MainStore.GetItems().ToList();
            LoadItemsCommand = new Command((obj) => ExecuteLoadItemsCommand(obj as Gender?));

            MessagingCenter.Subscribe<ParamsViewModel>(this, "Predictions computed", (ParamsViewModel vm) =>
            {
                LoadItemsCommand.Execute(Gender.Female);
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
                var items = MiddleClothingStore.MainStore.GetItemsPref(gender.Value);
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

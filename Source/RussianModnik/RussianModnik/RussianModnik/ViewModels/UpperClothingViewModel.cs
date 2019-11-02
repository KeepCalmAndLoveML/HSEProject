using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using RussianModnik.Models;
using RussianModnik.Stores;

using Xamarin.Forms;

namespace RussianModnik.ViewModels
{
    public class UpperClothingViewModel : BaseViewModel
    {
        public List<UpperClothing> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public UpperClothingViewModel()
        {
            Title = "Верхняя одежда";

            Items = UpperClothingStore.MainStore.GetItems().ToList();
            LoadItemsCommand = new Command((obj) => ExecuteLoadItemsCommand(obj as Gender?));

            MessagingCenter.Subscribe<ParamsViewModel>(this, "Predictions computed", (ParamsViewModel vm) =>
            {
                LoadItemsCommand.Execute(vm.ParamValues.GenderIsMan ? Gender.Male : Gender.Female);
                foreach (UpperClothing item in Items)
                {
                    item.Height = vm.ParamValues.Height;
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
                var items = UpperClothingStore.MainStore.GetItemsPref(gender.Value);
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

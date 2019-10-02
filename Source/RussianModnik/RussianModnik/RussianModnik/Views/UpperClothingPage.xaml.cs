using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RussianModnik.Models;
using RussianModnik.ViewModels;

namespace RussianModnik.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UpperClothingPage : ContentPage
	{
		private UpperClothingViewModel ViewModel;

		public UpperClothingPage()
		{
			InitializeComponent();

			ViewModel = new UpperClothingViewModel();
			this.BindingContext = ViewModel;
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			UpperClothing Item;
			if (ItemsListView.TryGetSelectedItem(args, out Item))
			{
				await Navigation.PushAsync(new UpperClothingDetailPage(new UpperClothingDetailViewModel(Item)));
			}
		}
	}
}
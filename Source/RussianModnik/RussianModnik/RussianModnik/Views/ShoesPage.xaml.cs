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
	public partial class ShoesPage : ContentPage
	{
		ShoesViewModel ViewModel;

		public ShoesPage()
		{
			InitializeComponent();

			BindingContext = ViewModel = new ShoesViewModel();
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			Shoes Item;
			if (ItemsListView.TryGetSelectedItem(args, out Item))
			{
                await Navigation.PushAsync(new ShoesDetailPage(new ShoesDetailViewModel(Item)));
			}
		}
	}
}
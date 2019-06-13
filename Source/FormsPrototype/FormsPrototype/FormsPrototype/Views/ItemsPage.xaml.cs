using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FormsPrototype.Models;
using FormsPrototype.Views;
using FormsPrototype.ViewModels;

namespace FormsPrototype.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemsPage : ContentPage
	{
		ItemsViewModel viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new ItemsViewModel();		
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			Item selected;
			if(ItemsListView.TryGetSelectedItem<Item>(args, out selected))
				await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(selected)));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if(viewModel.Items.Count == 0)
				viewModel.LoadItemsCommand.Execute(null);
		}
	}
}
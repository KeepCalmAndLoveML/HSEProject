using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FormsPrototype.Models;
using FormsPrototype.ViewModels;


namespace FormsPrototype.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetailPage : ContentPage
	{
		ItemDetailViewModel ViewModel;

		public ItemDetailPage(ItemDetailViewModel viewModel = null)
		{
			InitializeComponent();

			if(viewModel != null)
				ViewModel = viewModel;
			else
			{
				var item = new Item
				{
					Name = "Shoes",
					Description = "This is an item description.",
					ShoeSize = 260,
					Gender = Gender.Male
				};

				ViewModel = new ItemDetailViewModel(item);
			}
			BindingContext = ViewModel;

			RelatedListView.ItemSelected += RelatedListView_ItemSelected;

			//TODO: This should be in the xaml, but doesn't work there
			DetailImage.Source = ImageSource.FromResource(ViewModel.Item.RessourceIdDetail);
		}

		private async void RelatedListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			Item selected;
			if(RelatedListView.TryGetSelectedItem<Item>(e, out selected))
				await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(selected)));
		}
	}
}
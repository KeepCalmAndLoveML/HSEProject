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
		ItemDetailViewModel viewModel;

		public ItemDetailPage(ItemDetailViewModel viewModel)
		{
			InitializeComponent();

			this.viewModel = viewModel;
			Initialize();
		}

		public ItemDetailPage()
		{
			InitializeComponent();

			var item = new Item
			{
				Name = "Shoes",
				Description = "This is an item description.",
				ShoeSize = 260,
				Gender = Gender.Male
			};

			viewModel = new ItemDetailViewModel(item);
			Initialize();		
		}
		
		private void Initialize()
		{
			BindingContext = viewModel;

			RelatedListView.ItemSelected += RelatedListView_ItemSelected;
		}

		private async void RelatedListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			Item selected;
			if(RelatedListView.TryGetSelectedItem<Item>(e, out selected))
				await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(selected)));
		}
	}
}
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

			BindingContext = this.viewModel = viewModel;
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
			BindingContext = viewModel;
		}
	}
}
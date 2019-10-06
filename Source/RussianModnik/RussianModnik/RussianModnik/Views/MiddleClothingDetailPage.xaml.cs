using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RussianModnik.ViewModels;
using RussianModnik.Models;

namespace RussianModnik.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MiddleClothingDetailPage : ContentPage
	{
		MiddleClothingDetailViewModel ViewModel;

		public MiddleClothingDetailPage(MiddleClothingDetailViewModel viewModel)
		{
			InitializeComponent();

			this.BindingContext = ViewModel = viewModel;
			RelatedListView.ItemSelected += RelatedListView_ItemSelected;

			//TODO: This should be in the xaml, but doesn't work there
			DetailImage.Source = ImageSource.FromResource(ViewModel.Item.RessourceIdDetail);
		}

		private async void RelatedListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			MiddleClothing selected;
			if (RelatedListView.TryGetSelectedItem(e, out selected))
				await Navigation.PushAsync(new MiddleClothingDetailPage(new MiddleClothingDetailViewModel(selected)));
		}
	}
}
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
    public partial class ShoesDetailPage : ContentPage
    {
        ShoesDetailViewModel ViewModel;

        public ShoesDetailPage(ShoesDetailViewModel vm)
        {
            InitializeComponent();

            BindingContext = ViewModel = vm;

            RelatedListView.ItemSelected += RelatedListView_ItemSelected;

            //TODO: This should be in the xaml, but doesn't work there
            DetailImage.Source = ImageSource.FromResource(ViewModel.Item.RessourceIdDetail);
        }

        private async void RelatedListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Shoes selected;
            if (RelatedListView.TryGetSelectedItem(e, out selected))
                await Navigation.PushAsync(new ShoesDetailPage(new ShoesDetailViewModel(selected)));
        }
    }
}
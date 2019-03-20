using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FormsPrototype.Models;
using FormsPrototype.ViewModels;

using HorizontalListView = Sharpnado.Presentation.Forms.RenderedViews.HorizontalListView;


namespace FormsPrototype.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetailPage : ContentPage
	{
		ItemDetailViewModel viewModel;
		HorizontalListView relatedItemsListView;

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

			//Initialize this programmatically as XAML does not work for some reason
			relatedItemsListView = new HorizontalListView();
			SetBinding(HorizontalListView.ItemsSourceProperty, new Binding("RelatedItems"));
			relatedItemsListView.IsVisible = true;
			relatedItemsListView.ItemWidth = 144;
			relatedItemsListView.ItemHeight = 144;
			relatedItemsListView.SnapStyle = Sharpnado.Presentation.Forms.RenderedViews.SnapStyle.Center;
			relatedItemsListView.CollectionPadding = 8;
			relatedItemsListView.VisibleCellCount = 3;
			relatedItemsListView.Margin = new Thickness(0);

			#region ItemTemplate

			relatedItemsListView.ItemTemplate = new DataTemplate(() =>
				{
					return new ViewCell { View = new ItemView() };
				}
			);
			

			#endregion
		}
	}
}
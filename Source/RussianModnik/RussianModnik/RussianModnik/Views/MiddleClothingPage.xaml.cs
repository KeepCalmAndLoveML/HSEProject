﻿using System;
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
	public partial class MiddleClothingPage : ContentPage
	{
		private MiddleClothingViewModel ViewModel;

		public MiddleClothingPage()
		{
			InitializeComponent();

			this.BindingContext = ViewModel = new MiddleClothingViewModel();
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			MiddleClothing selected;
			if (ItemsListView.TryGetSelectedItem(args, out selected))
				await Navigation.PushAsync(new MiddleClothingDetailPage(new MiddleClothingDetailViewModel(selected)));
		}
	}
}
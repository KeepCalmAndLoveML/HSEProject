using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RussianModnik.Views;
using RussianModnik.Stores;

using RecommendationsModel;

using Xam.Plugin.SimpleAppIntro;

namespace RussianModnik
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			//Load data before anything else
			//DataManager.LoadDocument(Settings.SettingsFileName);

			UpperClothingStore.MainStore = new UpperClothingStore();
			MiddleClothingStore.MainStore = new MiddleClothingStore();
			ShoesStore.MainStore = new ShoesStore();
			
			MainPage = new MainPage();
		}

		protected override void OnStart()
		{ 

		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

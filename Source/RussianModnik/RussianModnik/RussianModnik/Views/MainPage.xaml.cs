using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RecommendationsModel;
using Xam.Plugin.SimpleAppIntro;

namespace RussianModnik.Views
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : TabbedPage
	{
		private AnimatedSimpleAppIntro WelcomePage;

		public MainPage()
		{
			InitializeComponent();

			WelcomePage = new AnimatedSimpleAppIntro(new List<object>() {
				new Slide(new SlideConfig(
					"Добро пожаловать!",
					"Хотите круто одеваться, но нет времени разбираться со всеми правилами моды?",
					"no-clothes.json", null
				)),
				new ButtonSlide(new ButtonSlideConfig(
					"За работу!",
					"Введите свои параметры и вы сразу же получите первоклассные рекомендации по стилю",
					"working.json", null, "Вперед!", null, buttonCommand: new Command(() => OnIntroFinished())
				))
			})
			{
				ShowNextButton = false,
				ShowPositionIndicator = true,
				ShowSkipButton = false,

				DoneButtonBackgroundColor = Color.Transparent.GetHexString(),
				DoneText = string.Empty
			};

			Navigation.PushModalAsync(WelcomePage);
		}

		public void OnIntroFinished()
		{
			WelcomePage.OnDoneButtonClicked?.Invoke();

			//Last page is always welcome page if this is called
			Navigation.PopModalAsync(); 
		}
	}
}
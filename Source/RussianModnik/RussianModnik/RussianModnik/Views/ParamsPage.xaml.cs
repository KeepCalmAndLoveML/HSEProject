using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;
using SkiaSharp.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RussianModnik.Animations;
using RussianModnik.ViewModels;
using SkiaSharp.Views.Forms;

namespace RussianModnik.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParamsPage : ContentPage
	{
		//Put settings here so they are easier to change in the future
		public readonly static Color GradientStart = Color.FromHex("#A6B1E1".ToUpper());
		public readonly static Color GradientEnd = Color.FromHex("#f4c95d".ToUpper());
		public readonly static TimeSpan StrokeAnimationDuration = TimeSpan.FromMilliseconds(900);
		public readonly static Easing StrokeAnimationEasing = Easing.CubicInOut;

		private readonly HighlightForm Highlighter;

		private ParamsViewModel ViewModel;

		public ParamsPage()
		{
			InitializeComponent();

			ViewModel = new ParamsViewModel();
			this.BindingContext = ViewModel;

			Highlighter = new HighlightForm
			(
				new HighlightSettings()
				{
					StrokeWidth = 8,
					StrokeStartColor = GradientStart,
					StrokeEndColor = GradientEnd,
					AnimationDuration = StrokeAnimationDuration,
					AnimationEasing = StrokeAnimationEasing,
				}
			);
		}

		private void DownloadClicked(object sender, EventArgs e)
		{
			IsBusy = true;
			ActivityFrame.IsVisible = true;
			MainIndicator.IsRunning = true;

			Task.Factory.StartNew(() =>
			{
				Task.Delay(3000).Wait(); //Show the user something is happening
				Device.BeginInvokeOnMainThread(() =>
				{
					ViewModel.PopulateParams();
					ShowParams();

					IsBusy = false;
					ActivityFrame.IsVisible = false;
					MainIndicator.IsRunning = false;
				});
			});
		}

		private void ShowParams()
		{
			HeightEntry.Text = ViewModel.ParamValues.Height.ToString();
			WeightEntry.Text = ViewModel.ParamValues.Weight.ToString();
			if (ViewModel.ParamValues.GenderIsMan)
				GenderPicker.SelectedItem = "Мужчина";
			else
				GenderPicker.SelectedItem = "Женщина";
			BodyTypePicker.SelectedItem = ViewModel.ParamValues.BodyType;
		}

		public ParamsPage(ParamsViewModel viewModel) : this()
		{
			ViewModel = viewModel;
			this.BindingContext = ViewModel;
		}

		#region Animations

		private void HighlightElement(View sender)
		{
			Highlighter.HighlightElement(sender, MainCanvasView, MainLayout);
		}

		private void EntryFocused(object sender, FocusEventArgs e)
		{
			HighlightElement((View)sender);
		}

		private void BoxViewFocused(object sender, FocusEventArgs e)
		{
			HighlightElement((View)sender);
		}

		private void PickerFocused(object sender, FocusEventArgs e)
		{
			HighlightElement((View)sender);
		}

		//Why doesn't this work?
		private void ChooseEyeColorGrid_Focused(object sender, FocusEventArgs e)
		{
			HighlightElement(ChooseEyeColorGrid);
		}

		private void MainCanvasViewPaintSurfaceRequested(object sender, SKPaintSurfaceEventArgs e)
		{
			Highlighter.Draw(MainCanvasView, e.Surface.Canvas);
		}

		#endregion

		private void FinishButton_Clicked(object sender, EventArgs e)
		{
			//Do some math stuff to calculate recommendations

			//HighlightElement((View)sender);
		}

		void UpdateBodyTypeGrids()
		{
			//MenBodyTypeGrid.IsVisible = ViewModel.GenderIsMan && BodyTypeSwitch.IsToggled;
			WomenBodyTypeGrid.IsVisible = ViewModel.GenderIsWoman && BodyTypeSwitch.IsToggled;
		}

		private void GenderPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			string gender = (sender as Picker).SelectedItem as string;
			if (gender == "Man")
				ViewModel.GenderIsMan = true;
			else
				ViewModel.GenderIsMan = false;

			UpdateBodyTypeGrids();
		}

		private void Switch_Toggled(object sender, ToggledEventArgs e)
		{
			UpdateBodyTypeGrids();
		}
	}
}
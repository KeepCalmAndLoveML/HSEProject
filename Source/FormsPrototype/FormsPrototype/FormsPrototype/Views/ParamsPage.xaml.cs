using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FormsPrototype.Animations;
using SkiaSharp.Views.Forms;

using FormsPrototype.Models;
using FormsPrototype.ViewModels;

namespace FormsPrototype.Views
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

			HighlightElement((View)sender);
		}

		private void GenderPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			string gender = (sender as Picker).SelectedItem as string;
			if (gender == "Man")
				ViewModel.GenderIsMan = true;
			else
				ViewModel.GenderIsMan = false;

			MenBodyTypeGrid.IsVisible = ViewModel.GenderIsMan && BodyTypeSwitchCell.On;
			WomenBodyTypeGrid.IsVisible = ViewModel.GenderIsWoman && BodyTypeSwitchCell.On;
			//DisplayAlert("Debug", $"{ViewModel.GenderIsMan}->{MBodyImage.IsVisible}, {ViewModel.GenderIsWoman}->{WBodyImage.IsVisible}", "Ok");
		}

		private void BodyTypeSwitchCell_OnChanged(object sender, ToggledEventArgs e)
		{
			SwitchCell sc = sender as SwitchCell;

			MenBodyTypeGrid.IsVisible &= sc.On;
			WomenBodyTypeGrid.IsVisible &= sc.On;
		}
	}
}
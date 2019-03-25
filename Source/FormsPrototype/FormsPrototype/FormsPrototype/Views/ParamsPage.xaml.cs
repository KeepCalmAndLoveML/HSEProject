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

namespace FormsPrototype.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParamsPage : ContentPage
	{
		//Put settings here so they are easier to change in the future
		public readonly static Color GradientStart = Color.FromHex("#FF4600");
		public readonly static Color GradientEnd = Color.FromHex("#CC00AF");
		public readonly static TimeSpan StrokeAnimationDuration = TimeSpan.FromMilliseconds(900);
		public readonly static Easing StrokeAnimationEasing = Easing.CubicInOut;

		private readonly HighlightForm Highlighter;

		private readonly List<ChooseEyeOption> EyeOptions;

		public ParamsPage()
		{
			InitializeComponent();

			Highlighter = new HighlightForm
			(
				new HighlightSettings()
				{
					StrokeWidth = 6,
					StrokeStartColor = GradientStart,
					StrokeEndColor = GradientEnd,
					AnimationDuration = StrokeAnimationDuration,
					AnimationEasing = StrokeAnimationEasing,
				}
			);

			EyeOptions = new List<ChooseEyeOption>()
			{
				new ChooseEyeOption { Name="Blue eyes", Description="Gorgeous"},
				new ChooseEyeOption { Name="Green eyes", Description="Astonishing"},
				new ChooseEyeOption { Name="Yellow eyes", Description="What?! Do they really exit?" },
				new ChooseEyeOption { Name="Black eyes", Description="Miraculous"},
				new ChooseEyeOption { Name="Gray eyes", Description="Like gandalf"},
				new ChooseEyeOption { Name="ААТ ВИН ТАА", Description="DUDE I LOVE IT"}
			};


			EyesListView.ItemsSource = EyeOptions;
		}

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
		
		private void MainCanvasViewPaintSurfaceRequested(object sender, SKPaintSurfaceEventArgs e)
		{
			Highlighter.Draw(MainCanvasView, e.Surface.Canvas);
		}

		private void FinishButton_Clicked(object sender, EventArgs e)
		{
			//Do some math stuff to calculate recommendations

			HighlightElement((View)sender);
		}
	}
}
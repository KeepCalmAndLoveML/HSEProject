using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FormsPrototype.Animations;
using SkiaSharp.Views.Forms;

namespace FormsPrototype.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParamsPage : ContentPage
	{
		private readonly HighlightForm Highlighter;
		
		public ParamsPage()
		{
			InitializeComponent();

			Highlighter = new HighlightForm
			(
				new HighlightSettings()
				{
					StrokeWidth = 6,
					StrokeStartColor = Color.FromHex("#FF4600"),
					StrokeEndColor = Color.FromHex("#CC00AF"),
					AnimationDuration = TimeSpan.FromMilliseconds(900),
					AnimationEasing = Easing.CubicInOut,
				}
			);	
		}

		private void HighlightElement(View sender)
		{
			Highlighter.HighlightElement(sender, MainCanvasView, MainLayout);
		}

		private void EntryFocused(object sender, FocusEventArgs e)
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
/*
 * Strange rendering issue on iOS:
 * The BarBackgroundColor porperty for my main page seems to mix the set color with some opacity
 * Same issue here: https://forums.xamarin.com/discussion/17811/tabbedpage-tabbar-background-color-tint
 * This seems to only be a problem on iOS
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms;
using FormsPrototype.iOS.CustomRenderers;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
namespace FormsPrototype.iOS.CustomRenderers
{
	//TODO: Acces the static ressource somehow
	class TabbedPageRenderer : TabbedRenderer
	{
		public TabbedPageRenderer()
		{
			TabBar.TintColor = Color.FromHex("#272A44").ToUIColor();
			TabBar.BarTintColor = Color.FromHex("#272A44").ToUIColor();
			TabBar.BackgroundColor = Color.FromHex("#272A44").ToUIColor();
		}

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			TabBar.TintColor = Color.FromHex("#272A44").ToUIColor();
			TabBar.BarTintColor = Color.FromHex("#272A44").ToUIColor();
			TabBar.BackgroundColor = Color.FromHex("#272A44").ToUIColor();
		}
	}
}
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Xamarin.Forms;
using SkiaSharp.Views.Forms;

namespace FormsPrototype
{
	// Extensions for SkiaSharp
	public static class SkiaSharpTools
	{
		public static Rectangle FromPixels(this SKCanvasView skCanvasView, Rectangle rc) =>
			new Rectangle(skCanvasView.FromPixels(rc.Location), skCanvasView.FromPixels(rc.Size));

		public static Size FromPixels(this SKCanvasView skCanvasView, Size rc) =>
			(Size)skCanvasView.FromPixels(new Point(rc.Width, rc.Height));

		public static Point FromPixels(this SKCanvasView skCanvasView, Point pt)
		{
			double wf = skCanvasView.CanvasSize.Width / skCanvasView.Width;
			double hf = skCanvasView.CanvasSize.Height / skCanvasView.Height;
			return new Point(pt.X * wf, pt.Y * hf);
		}
	}

	//Extensions for Xamarin.Forms controls
	public static class UIExtensions
	{
		public static bool TryGetSelectedItem<T>(this ListView ls, SelectedItemChangedEventArgs eventArgs, out T result) where T : class
		{
			result = eventArgs.SelectedItem as T;
			if (result == null)
				return false;

			// Manually deselect item.
			ls.SelectedItem = null;
			return true;
		}
	}

	//Extensions for basic C# classes 
	public static class SimpleExtensions
	{
		public static string[] Ressources = typeof(SimpleExtensions).Assembly.GetManifestResourceNames();
		
		public static string ToImageRessourceId(this string filename)
		{
			//Those checks might be slow, redundant and unnecessary...
			//But this ressource thing seems scary not throwing exceptions, so I might as well leave them here

			//I decided jpg might also be nice for memory saving.
			//if (Path.GetExtension(filename) != "png")
				//throw new ArgumentException("Are you sure you want to use non png files?");
			if(Ressources.Where(x => x.Contains(filename)).Count() == 0)
				throw new ArgumentException("Invalid ressource name");

			return $"{typeof(SimpleExtensions).Assembly.GetName().Name}.Images.{filename}";
		}

		public static TimeSpan Multiply(this TimeSpan timeSpan, double coef)
		{
			return TimeSpan.FromMilliseconds(timeSpan.TotalMilliseconds * coef);
		}
	}
}

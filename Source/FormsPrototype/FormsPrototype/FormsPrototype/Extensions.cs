using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using SkiaSharp.Views.Forms;

namespace FormsPrototype
{
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
}

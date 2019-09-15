using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace RussianModnik.Animations
{
	class HighlightSettings
	{
		//The color of the highlighter will be a gradient transition from Start to End
		public Color StrokeStartColor { get; set; }
		public Color StrokeEndColor { get; set; }

		public double StrokeWidth { get; set; }
		public TimeSpan AnimationDuration { get; set; }

		public Easing AnimationEasing { get; set; }
		public IList<View> Elements { get; set; }
	}
}

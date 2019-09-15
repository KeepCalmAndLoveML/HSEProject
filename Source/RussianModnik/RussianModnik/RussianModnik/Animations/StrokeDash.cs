using System;
using System.Collections.Generic;
using System.Text;

namespace RussianModnik.Animations
{
	//Integrate this intro HighlightSettings?
	class StrokeDash
	{
		//Length of "on" and "off" intervals (when the stroke is shown and when it is not) that will repeat for the whole animation
		public float[] Intervals { get; set; }

		//An initial offset that repeats once
		public float Phase { get; set; }

		/// Intervals = {3, 2}
		/// ---__---__---__ ...
		/// Intervals = {3, 2}, Phase = 1
		/// -_---__---__--- ...

		public StrokeDash(float[] intervals, float phase)
		{
			Intervals = new float[intervals.Length];
			Array.Copy(intervals, Intervals, intervals.Length);
			Phase = phase;
		}

		public StrokeDash(StrokeDash strokeDash)
			: this(strokeDash.Intervals, strokeDash.Phase)
		{
		}
	}
}

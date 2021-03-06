﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FormsPrototype.Animations
{
	//TODO: Try replacing arcs around buttons by lines (maybe make this a setting?)
	class HighlightPath
	{
		//The highlighter will higlight elements with types that inherit from those in this list
		//TODO: Adjust these
		public static readonly List<Type> AllowedUITypes = new List<Type>()
		{
			typeof(Button),
			typeof(Entry),
			typeof(BoxView),
			typeof(Image),
			typeof(Picker),
			typeof(ImageButton),
			typeof(Grid),
		};

		//StrokeDashMap[Id] -> StrokeDash
		private readonly Dictionary<int, StrokeDash> StrokeDashMap = new Dictionary<int, StrokeDash>();

		public StrokeDash FirstDash => StrokeDashMap.Values.FirstOrDefault();

		public int DashCount => StrokeDashMap.Count;

		public SKPath Path { get; private set; }

		public StrokeDash GetDashForView(IList<View> layoutChildren, View view) => StrokeDashMap[GetViewId(layoutChildren, view)];

		public StrokeDash GetDashForView(IList<View> layoutChildren, int viewId) => StrokeDashMap[viewId];

		public StrokeDash GetDash(int index) => StrokeDashMap.ElementAt(index).Value;

		public void SetDashForView(IList<View> layoutChildren, View view, StrokeDash strokeDash) => StrokeDashMap[GetViewId(layoutChildren, view)] = strokeDash;

		public int GetViewId(IList<View> layoutChildren, View view) => layoutChildren.IndexOf(view);

		public View GetView(IList<View> layoutChildren, int Id) => layoutChildren.ElementAt(Id);

		public static HighlightPath Create(SKCanvasView skCanvasView, IList<View> layoutChildren, double strokeWidth)
		{
			var path = new SKPath();

			var highlightPath = new HighlightPath()
			{
				Path = path
			};

			strokeWidth = (float)skCanvasView.FromPixels(new Point(0, strokeWidth)).Y;

			foreach(View view in layoutChildren)
			{
				int dashCount = highlightPath.DashCount;
				Rectangle viewBounds = skCanvasView.FromPixels(view.Bounds);

				if(!AllowedUITypes.Contains(view.GetType()))
					continue;

				if(path.Points.Length == 0)
				{
					// Move path point at the left and below of the view
					path.MoveTo(
						x: (float)viewBounds.X,
						y: (float)viewBounds.Y + (float)viewBounds.Height + (float)strokeWidth / 2);
				}

				float xCurr = path.LastPoint.X;
				float yCurr = path.LastPoint.Y;

				// Add arch for views except first one
				if(dashCount > 0)
				{
					float d = dashCount % 2 == 0 ? -1 : 1;
					float arcHeight = (float)viewBounds.Y + (float)viewBounds.Height - path.LastPoint.Y + (float)strokeWidth / 2;
					path.ArcTo(new SKRect(xCurr - arcHeight / 2, yCurr, xCurr + arcHeight / 2, yCurr + arcHeight), -90, 180 * d, false);
				}

				float dashOffset = new SKPathMeasure(path).Length;

				// Add line below the view
				// If it's not the first view, the start point is the end from arc end point 
				// and line direction is either to view start or view end
				path.LineTo((float)viewBounds.X + (float)viewBounds.Width * ( dashCount % 2 == 0 ? 1 : 0 ), path.LastPoint.Y);

				if(view is Button || view is BoxView)
				{
					xCurr = path.LastPoint.X;
					yCurr = path.LastPoint.Y;

					// Draw arc from below button to above button
					float d = dashCount % 2 == 0 ? -1 : 1;
					float arcHeight = (float)viewBounds.Height + (float)strokeWidth;
					path.ArcTo(new SKRect(xCurr - arcHeight / 2, yCurr - arcHeight, xCurr + arcHeight / 2, yCurr), 90, 180 * d, false);

					// Draw horizontal line above the button
					path.LineTo(xCurr + (float)viewBounds.Width * d, path.LastPoint.Y);

					// Draw arc pointing down
					xCurr = path.LastPoint.X;
					yCurr = path.LastPoint.Y;
					path.ArcTo(new SKRect(xCurr - arcHeight / 2, yCurr, xCurr + arcHeight / 2, yCurr + arcHeight), -90, 180 * d, false);
				}

				float solidDashLength = new SKPathMeasure(path).Length - dashOffset;

				var strokeDash = new StrokeDash
				(
					//We do not want the dash to ever be "off"
					intervals: new float[] { solidDashLength, 0 },
					phase: -dashOffset
				);

				highlightPath.SetDashForView(layoutChildren, view, strokeDash);
			}

			// Compute the 2nd value of interval, which is the length of remaining path
			float pathLength = new SKPathMeasure(path).Length;

			for(int i = 0; i < highlightPath.DashCount; ++i)
			{
				StrokeDash d = highlightPath.GetDash(i);
				d.Intervals[1] = pathLength - d.Intervals[0];
			}

			return highlightPath;
		}
	}
}
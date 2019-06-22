using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FormsPrototype.Animations
{
	class HighlightForm
	{
		private readonly HighlightSettings HightlightSettings;
		private SKPaint SKPaint;
		private HighlightState HightLightState;

		public HighlightForm(HighlightSettings highlightSettings)
		{
			HightlightSettings = highlightSettings;
		}

		public void Draw(SKCanvasView skCanvasView, SKCanvas skCanvas)
		{
			skCanvas.Clear();

			if(HightLightState == null)
				return;

			if(SKPaint == null)
				SKPaint = CreateHighlightSkPaint(skCanvasView, HightlightSettings, HightLightState.HighlightPath);

			StrokeDash strokeDash = HightLightState.StrokeDash;
			// Comment the next line to see whole path without dash effect
			SKPaint.PathEffect = SKPathEffect.CreateDash(strokeDash.Intervals, strokeDash.Phase);
			skCanvas.DrawPath(HightLightState.HighlightPath.Path, SKPaint);
		}

		public void Invalidate(SKCanvasView skCanvasView, Layout<View> formLayout)
		{
			if(HightLightState == null)
				return;

			View viewToHighlight = HightLightState.HighlightPath.GetView(formLayout.Children, HightLightState.CurrHighlightedViewId);
			HightLightState = null;

			HighlightElement(viewToHighlight, skCanvasView, formLayout);
		}

		public void HighlightElement(View viewToHighlight, SKCanvasView skCanvasView, Layout<View> formLayout)
		{
			IList<View> layoutChildren = formLayout.Children;

			if(HightLightState == null)
			{
				var path = HighlightPath.Create(skCanvasView, layoutChildren, HightlightSettings.StrokeWidth);
				HightLightState = new HighlightState()
				{
					HighlightPath = path
				};
			}

			HighlightPath highlightPath = HightLightState.HighlightPath;

			int currHighlightViewId = HightLightState.CurrHighlightedViewId;
			int iViewIdToHighlight = highlightPath.GetViewId(layoutChildren, viewToHighlight);

			if(currHighlightViewId == iViewIdToHighlight)
				return;

			StrokeDash fromDash;
			if(currHighlightViewId != -1)
				fromDash = HightLightState.StrokeDash;
			else
				fromDash = new StrokeDash(highlightPath.GetDashForView(layoutChildren, iViewIdToHighlight));

			HightLightState.CurrHighlightedViewId = iViewIdToHighlight;

			StrokeDash toDash = new StrokeDash(highlightPath.GetDashForView(layoutChildren, viewToHighlight));

			double distance = Math.Abs(layoutChildren[iViewIdToHighlight].Bounds.X - layoutChildren[currHighlightViewId].Bounds.X);
			DrawDash(skCanvasView, fromDash, toDash, distance / 70);
		}

		void DrawDash(SKCanvasView skCanvasView, StrokeDash fromDash, StrokeDash toDash, double durationMultiplier = 1)
		{
			if(fromDash != null)
			{
				var anim = new StrokeDashAnimation(
					from: fromDash,
					to: toDash,
					duration: HightlightSettings.AnimationDuration.Multiply(durationMultiplier));

				anim.Start((strokeDashToDraw) => RequestDraw(skCanvasView, strokeDashToDraw));
			}
			else
				RequestDraw(skCanvasView, toDash);
		}

		void RequestDraw(SKCanvasView skCanvasView, StrokeDash strokeDashToDraw)
		{
			HightLightState.StrokeDash = strokeDashToDraw;
			skCanvasView.InvalidateSurface();
		}

		static SKPaint CreateHighlightSkPaint(SKCanvasView skCanvasView, HighlightSettings highlightSettings, HighlightPath highlightPath)
		{
			var skPaint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Red,
				StrokeWidth = (float)skCanvasView.FromPixels(new Point(0, highlightSettings.StrokeWidth)).Y
			};

			float firstDashIntervalOn = highlightPath.FirstDash.Intervals[0];
			skPaint.Shader = SKShader.CreateLinearGradient(
								start: new SKPoint(firstDashIntervalOn * 0.30f, 0),
								end: new SKPoint(firstDashIntervalOn, 0),
								colors: new SKColor[] {
											highlightSettings.StrokeStartColor.ToSKColor(),
											highlightSettings.StrokeEndColor.ToSKColor()
								},
								colorPos: new float[] { 0, 1 },
								mode: SKShaderTileMode.Clamp);

			return skPaint;
		}
	}
}

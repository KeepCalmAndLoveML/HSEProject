using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace FormsPrototype.Animations
{
	class StrokeDashAnimation
	{
		private StrokeDash CurrentStrokeDash;

		public StrokeDash From { get; }
		public StrokeDash To { get; }

		public TimeSpan Duration { get; }

		//This makes things beautiful
		public Easing Easing { get; }

		public StrokeDashAnimation(StrokeDash from, StrokeDash to, TimeSpan duration)
		{
			From = from;
			To = to;
			Duration = duration;
		}

		public void Start(Action<StrokeDash> onValueCallback, IAnimatable page = null)
		{
			IAnimatable commitPage = page == null ? Application.Current.MainPage : page;

			CurrentStrokeDash = From;

			var anim = new Animation(_ => onValueCallback(CurrentStrokeDash));

			anim.Add(0, 1, new Animation(
				callback: v => CurrentStrokeDash.Phase = (float)v,
				start: From.Phase,
				end: To.Phase,
				easing: Easing));

			anim.Add(0, 1, new Animation(
				callback: v => CurrentStrokeDash.Intervals[0] = (float)v,
				start: From.Intervals[0],
				end: To.Intervals[0],
				easing: Easing));

			anim.Add(0, 1, new Animation(
				callback: v => CurrentStrokeDash.Intervals[1] = (float)v,
				start: From.Intervals[1],
				end: To.Intervals[1],
				easing: Easing));

			anim.Commit(
				owner: commitPage,
				name: "highlightAnimation",
				length: (uint)Duration.TotalMilliseconds);
		}
	}
}

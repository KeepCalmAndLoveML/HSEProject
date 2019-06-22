using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace FormsPrototype.CustomControls
{
	public class CustomViewCell : ViewCell
	{
		public static readonly BindableProperty IsTransparentBackgroundWhenSelectedCellProperty =
		   BindableProperty.Create("IsTransparentBackgroundWhenSelectedCell", typeof(bool), typeof(CustomViewCell),
			   false);

		public bool IsTransparentBackgroundWhenSelectedCell
		{
			get => (bool)GetValue(IsTransparentBackgroundWhenSelectedCellProperty);
			set => SetValue(IsTransparentBackgroundWhenSelectedCellProperty, value);
		}
	}
}

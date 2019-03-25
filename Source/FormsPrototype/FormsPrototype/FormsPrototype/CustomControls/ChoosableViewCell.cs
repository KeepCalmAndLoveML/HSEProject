using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace FormsPrototype.CustomControls
{
	//Describes a view cell that will highlight when chosen
	public class ChoosableViewCell : ViewCell
	{
		public static readonly BindableProperty SelectedItemBackgroundColorProperty = BindableProperty.Create("SelectedItemBackgroundColor", typeof(Color), typeof(ChoosableViewCell));

		public Color SelectedItemBackgroundColor
		{
			get
			{
				return (Color)GetValue(SelectedItemBackgroundColorProperty);
			}
			set
			{
				SetValue(SelectedItemBackgroundColorProperty, value);
			}
		}
	}
}

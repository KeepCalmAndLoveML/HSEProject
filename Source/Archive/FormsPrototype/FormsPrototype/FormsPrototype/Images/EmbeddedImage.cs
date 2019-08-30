using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsPrototype.Images
{
	public class EmbeddedImage : BindableObject, IMarkupExtension
	{
		public static readonly BindableProperty RessourceIdProperty = BindableProperty.Create(nameof(RessourceId), typeof(string), typeof(EmbeddedImage), default(string));
		public string RessourceId
		{
			get => (string)GetValue(RessourceIdProperty);
			set => SetValue(RessourceIdProperty, value);
		}

		public EmbeddedImage()
		{

		}

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return MyProvideValue();
		}

		//This was created for the converter
		public object MyProvideValue()
		{
			if(string.IsNullOrEmpty(RessourceId) || string.IsNullOrWhiteSpace(RessourceId))
				throw new InvalidOperationException($"Invalid ressource Id: {RessourceId}");

			return ImageSource.FromResource(RessourceId);
		}
	}

	public class EmbeddedImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var img = new EmbeddedImage()  { RessourceId = value as string };
			Debug.WriteLine(img.RessourceId);

			return img.MyProvideValue();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

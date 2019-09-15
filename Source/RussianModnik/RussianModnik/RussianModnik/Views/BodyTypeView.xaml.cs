using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RussianModnik.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BodyTypeView : ContentView
	{
		public readonly static Dictionary<string, string> TitleToRessourceId = new Dictionary<string, string>()
		{
			["Title"] = "mbt_one.png",
		};

		public static readonly BindableProperty ImageResosurceIdProperty = BindableProperty.Create(nameof(ImageRessourceId), typeof(string), typeof(BodyTypeView), default(string));
		public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(BodyTypeView), default(string));
		public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(BodyTypeView), default(string));
		public static readonly BindableProperty FormattedDescriptionProperty = BindableProperty.Create(nameof(FormattedDescription), typeof(FormattedString), typeof(BodyTypeView), default(FormattedString));


		public string Title
		{
			get => (string)GetValue(TitleProperty);
			set
			{
				SetValue(TitleProperty, value);
				Debug.WriteLine(Title);
			}
		}

		public string Description
		{
			get => (string)GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}

		public FormattedString FormattedDescription
		{
			get => GetValue(FormattedDescriptionProperty) as FormattedString;
			set => SetValue(FormattedDescriptionProperty, value);
		}

		public string ImageRessourceId
		{
			get => (string)GetValue(ImageResosurceIdProperty);
			set
			{
				SetValue(ImageResosurceIdProperty, value);
				Debug.WriteLine(ImageRessourceId);
				if (!string.IsNullOrEmpty(ImageRessourceId))
					MBodyImage.Source = ImageSource.FromResource(ImageRessourceId.ToImageRessourceId());
			}
		}

		public string RealRessourceId
		{
			get => string.IsNullOrEmpty(ImageRessourceId) ? string.Empty : ImageRessourceId.ToImageRessourceId();
		}

		public BodyTypeView()
		{
			InitializeComponent();
			this.BindingContext = this;
		}
	}
}
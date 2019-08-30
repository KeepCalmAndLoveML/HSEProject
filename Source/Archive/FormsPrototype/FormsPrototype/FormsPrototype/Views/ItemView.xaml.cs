using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FormsPrototype.Models;

//TODO: Implement this so that it can be used in other xaml pages without copy pasting the code
//Reference for custom views implementation: https://docs.microsoft.com/ru-ru/dotnet/framework/wpf/controls/how-to-create-a-custom-view-mode-for-a-listview

namespace FormsPrototype.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemView : ContentView
	{
		Item item;

		public ItemView ()
		{
			InitializeComponent();

			item = new Item
			{
				Name = "Shoes",
				Description = "This is an item description.",
				ShoeSize = 260,
				Gender = Gender.Male
			};

			this.BindingContext = item;
		}

		public ItemView(Item item)
		{
			InitializeComponent();

			this.BindingContext = this.item = item;
		}
	}
}
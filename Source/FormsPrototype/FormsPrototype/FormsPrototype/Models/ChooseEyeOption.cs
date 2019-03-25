using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace FormsPrototype.Models
{
	//Represents an item model for the eye choosing list view
    public class ChooseEyeOption
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public string ImageSource { get; set; }

		//TODO: Make this dependent of ImageSource
		public Image Image { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace RussianModnik.Models
{
	//Represents an item model for the eye choosing list view
	public class ChooseEyeOption
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string ImageRessourceId { get; set; }

		public ImageSource ImgSource
		{
			get => ImageSource.FromResource(ImageRessourceId);
			set => Name = Name; //Do nothing
		}
	}
}

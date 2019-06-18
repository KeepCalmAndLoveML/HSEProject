using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Linq;


using FormsPrototype.Models;
using Xamarin.Forms;

namespace FormsPrototype.ViewModels
{
	public class ParamsViewModel : BaseViewModel
	{
		private ChooseEyeOption _chosenEyeOption;

		public List<ChooseEyeOption> EyeOptions { get; set; }
		public ChooseEyeOption ChosenEyeOption
		{
			get => _chosenEyeOption;
			set
			{
				SetProperty(ref _chosenEyeOption, value);
				OnPropertyChanged();
			}
		}

		//Please don't hit me for male supremacy
		public bool GenderIsMan { get; set; } = true;

		//This is needed for Bindings
		public bool GenderIsWoman { get => !GenderIsMan; }

		public ParamsViewModel()
		{
			Title = "Parameters";

			EyeOptions = new List<ChooseEyeOption>()
			{
				new ChooseEyeOption { Name="Blue eyes", Description="Gorgeous"},
				new ChooseEyeOption { Name="Green eyes", Description="Astonishing"},
				new ChooseEyeOption { Name="Yellow eyes", Description="What?! Do they really exit?" },
				new ChooseEyeOption { Name="Black eyes", Description="Miraculous"},
				new ChooseEyeOption { Name="Gray eyes", Description="Like gandalf"},
			};

			EyeOptions.AddRange(Enumerable.Repeat(new ChooseEyeOption { Name = "Lorem Ipsum", Description = "Gorgeous" }, 10));

			ChosenEyeOption = EyeOptions[0];
		}

		public ICommand ItemClick
		{
			get => new Command((item) => ChosenEyeOption = (ChooseEyeOption)item);
		}
	}
}

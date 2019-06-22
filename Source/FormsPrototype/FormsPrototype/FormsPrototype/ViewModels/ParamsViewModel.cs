using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Linq;


using FormsPrototype.Models;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace FormsPrototype.ViewModels
{
	public class ParamsViewModel : BaseViewModel
	{
		private ChooseEyeOption _chosenEyeOption;
		private ReadOnlyCollection<string> MenBodyTypes, WomanBodyTypes;

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

		public List<string> BodyTypeNames
		{
			get
			{
				return GenderIsMan ? MenBodyTypes.ToList() : WomanBodyTypes.ToList();
			}
			set
			{

			}
		}

		//Please don't hit me for male supremacy
		public bool GenderIsMan { get; set; } = true;

		//This is needed for Bindings
		public bool GenderIsWoman
		{
			get => !GenderIsMan;
			set => GenderIsMan = !value;
		}

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

			MenBodyTypes = new ReadOnlyCollection<string>(new List<string>() { "Body type 1", "Body type 2", "Body type 3"});
			MenBodyTypes = new ReadOnlyCollection<string>(new List<string>() { "Body type 1", "Body type 2", "Body type 3" });
		}

		public ICommand ItemClick
		{
			get => new Command((item) => ChosenEyeOption = (ChooseEyeOption)item);
		}
	}
}

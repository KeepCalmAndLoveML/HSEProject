using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

using RecommendationsModel;

using RussianModnik.Models;
using RussianModnik.Services;

using Xamarin.Forms;

namespace RussianModnik.ViewModels
{
	public class ParameterCollection
	{
		private static readonly Dictionary<string, object> DefaultValues = new Dictionary<string, object>()
		{
			["Height"] = 175.0,
			["Weight"] = 55.0,
			["GenderIsMan"] = false,
			["BodyType"] = "Песочные часы"
		};

		public double Height { get; private set; } = (double)DefaultValues["Height"];
		public double Weight { get; set; } = (double)DefaultValues["Weight"];
		public bool GenderIsMan { get; set; } = (bool)DefaultValues["GenderIsMan"];
		public string BodyType { get; set; } = (string)DefaultValues["BodyType"];

		public ParameterCollection()
		{

		}

		public async void InitializeFrom(ParamsStorage storage)
		{
			var items = await storage.GetItemsAsync();
			foreach(Parameter p in items)
			{
				switch (p.Key)
				{
					case "Height":
						Height = (double)p.Value;
						break;
					case "Weight":
						Weight = (double)p.Value;
						break;
					case "GenderIsMan":
						GenderIsMan = (bool)p.Value;
						break;
					case "BodyType":
						BodyType = (string)p.Value;
						break;
				}
			}
		}
	}


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

		public ParameterCollection ParamValues { get; private set; }
		private ParamsStorage ParamStorage { get; set; }

		public List<string> BodyTypeNames
		{
			get => GenderIsMan ? MenBodyTypes.ToList() : WomanBodyTypes.ToList();
			set { }
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
			Title = "Параметры";
			GenderIsMan = false;

			EyeOptions = new List<ChooseEyeOption>()
			{
				new ChooseEyeOption { Name="Голубые глаза", Description="Невероятные"},
				new ChooseEyeOption { Name="Зелёные глаза", Description="Сногсшибательные"},
				new ChooseEyeOption { Name="Чёрные глаза", Description="Очаровательные"},
				new ChooseEyeOption { Name="Серые глаза", Description="Мистические"},
			};

			ChosenEyeOption = EyeOptions[0];

			WomanBodyTypes = new ReadOnlyCollection<string>(WomenMathModel.WomenBodyTypes);

			ParamStorage = new ParamsStorage();
			ParamValues = new ParameterCollection();
		}
		
		public void PopulateParams()
		{
			ParamValues.InitializeFrom(ParamStorage);
		}

		public ICommand ItemClick
		{
			get => new Command((item) => ChosenEyeOption = (ChooseEyeOption)item);
		}
	}

	public class BTypeNameToDescriptionConverter : IValueConverter
	{
		//Keep both versions of dicts in case we will change something in the future

		private static readonly Dictionary<string, string> NameToDescription = new Dictionary<string, string>()
		{
			["Песочные часы"] = @"Такой тип фигуры считается идеальным и гармоничным.
Главные характеристики: плечи и бёдры одинаковой ширины, талия уже бедёр хотя бы на 25 см",

			["Треугольник (Груша)"] = @"Женственный тип фигуры, который любят многие мужчины.
Главные характеристики: небольшие объёмы в груди, узкие плечи, широкие и женственные бёдра, узкая талия.
Разница между талией и бёдрами примерно равна 30 см",

			["Перевернутый треугольник"] = @"Женщины с таким типом фигуры, как правило, спортивны и энергичны.
Они не знают проблемы большинства женщин под названием ""зона галифе"".
Главные характеристики: широкие плечи, большая грудь и узкие бёдра",

			["Прямоугольник"] = @"Вы обладаете спортивной и стройной фигурой. Единственным вашим недостатком можно признать слабо выраженную талию
Главные характеристики: плечи и бёдра примерно одинаковой ширины, нет выраженной талии",

			["Яблоко"] = @"У вас много достоинств: пышный бюст, красивые и грациозные ноги, а также красивые и сияющие глаза.
Как правило, такие женщины очень открытые и позитивные. Пользуйтесь своими сильными сторонами и не бойтесь выглядеть стильно!
Главные характеристики: выступающий живот, пышный бюст, полные плечи, руки, ноги, а также изящные ноги ниже колена.",
		};

		private static bool FormatInit = false;
		private static readonly Dictionary<string, FormattedString> NameToFormattedDescription = new Dictionary<string, FormattedString>();

		private static void Initialize()
		{
			if (FormatInit)
				return;

			FormattedString temp;

			#region Песочные часы

			temp = new FormattedString();
			temp.Spans.Add(new Span()
			{
				Text = @"Такой тип фигуры считается идеальным и гармоничным

",
			});
			temp.Spans.Add(new Span()
			{
				Text = "Главные характеристики: ", FontAttributes = FontAttributes.Bold
			});
			temp.Spans.Add(new Span()
			{
				Text = "плечи и бёдра одинаковой ширины, талия уже бедёр хотя бы на 25 см",
			});

			NameToFormattedDescription.Add("Песочные часы", temp);

			#endregion

			#region Треугольник

			temp = new FormattedString();

			temp.Spans.Add(new Span() { Text = @"Женственный тип фигуры, который любят многие мужчины.

" });
			temp.Spans.Add(new Span() { Text = @"Главные характеристики: ", FontAttributes = FontAttributes.Bold });
			temp.Spans.Add(new Span() { Text = @"небольшие объёмы в груди, узкие плечи, широкие и женственные бёдра, узкая талия.
Разница между талией и бёдрами примерно равна 30 см" });

			NameToFormattedDescription.Add("Треугольник (Груша)", temp);

			#endregion

			#region Перервернутый треугольник

			temp = new FormattedString();
			temp.Spans.Add(new Span() { Text = @"Женщины с таким типом фигуры, как правило, спортивны и энергичны.
Они не знают проблемы большинства женщин под названием ""зона галифе""

" });
			temp.Spans.Add(new Span() { Text = @"Главные характеристики: ", FontAttributes = FontAttributes.Bold });
			temp.Spans.Add(new Span() { Text = @"широкие плечи, большая грудь и узкие бёдра" });


			NameToFormattedDescription.Add("Перевернутый треугольник", temp);
			#endregion

			#region Прямоугольник

			temp = new FormattedString();
			temp.Spans.Add(new Span() { Text = @"Вы обладаете спортивной и стройной фигурой. Единственным вашим недостатком можно признать слабо выраженную талию

" });
			temp.Spans.Add(new Span() { Text = @"Главные характеристики: ", FontAttributes = FontAttributes.Bold });
			temp.Spans.Add(new Span() { Text = @"плечи и бёдра примерно одинаковой ширины, нет выраженной талии" });

			NameToFormattedDescription.Add("Прямоугольник", temp);

			#endregion

			#region Яблоко

			temp = new FormattedString();
			temp.Spans.Add(new Span() { Text = @"У вас много достоинств: пышный бюст, красивые и грациозные ноги, а также красивые и сияющие глаза.
Как правило, такие женщины очень открытые и позитивные. Пользуйтесь своими сильными сторонами и не бойтесь выглядеть стильно!

" });
			temp.Spans.Add(new Span() { Text = @"Главные характеристики: ", FontAttributes = FontAttributes.Bold });
			temp.Spans.Add(new Span() { Text = @"выступающий живот, пышный бюст, полные плечи, руки, ноги, а также изящные ноги ниже колена." });

			NameToFormattedDescription.Add("Яблоко", temp);

			#endregion

			FormatInit = true;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!FormatInit)
				Initialize();

			
			string key = value as string;
			if (NameToFormattedDescription.ContainsKey(key))
				return NameToFormattedDescription[key];
			else
				return $"ERROR: Invalid key: {key}";
			
			/*
			string key = value as string;
			if (NameToDescription.ContainsKey(key))
				return NameToDescription[key];
			else
				return $"ERROR: Invalid key: {key}";
			*/
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

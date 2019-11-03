using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

using RecommendationsModel;

using RussianModnik.Models;
using RussianModnik.Stores;


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
            ["BodyType"] = "Песочные часы",
            ["FeetLength"] = 25.0,
        };

        public double Height { get; set; } = (double)DefaultValues["Height"];
        public double Weight { get; set; } = (double)DefaultValues["Weight"];
        public bool GenderIsMan { get; set; } = (bool)DefaultValues["GenderIsMan"];
        public string BodyType { get; set; } = (string)DefaultValues["BodyType"];
        public double FeetLength { get; set; } = (double)DefaultValues["FeetLength"];

        public ParameterCollection()
        {

        }

        public void InitializeFrom(ParamsStore storage)
        {
            var items = storage.GetItems();
            CultureInfo info = new CultureInfo("en-US");
            foreach (Parameter p in items)
            {
                switch (p.Key)
                {
                    case "Height":
                        Height = double.Parse(p.Value as string, info);
                        break;
                    case "Weight":
                        Weight = double.Parse(p.Value as string, info);
                        break;
                    case "HeightToWeight":
                        Weight = Height / double.Parse(p.Value as string, info);
                        Weight = Math.Round(Weight, 2);
                        break;
                    case "GenderIsMan":
                        GenderIsMan = Convert.ToBoolean(int.Parse(p.Value as string));
                        break;
                    case "BodyType":
                        BodyType = p.Value as string;
                        break;
                    case "FeetLength":
                        FeetLength = double.Parse(p.Value as string, info);
                        break;
                }
            }
        }

        public async void SaveTo(ParamsStore storage)
        {
            var boolToInt = new Func<bool, int>(x => x ? 1 : 0);

            //THIS IS A TEMPORARY SOLUTION FOR THE PROTOTYPE TO WORK.
            //IN THE FUTURE, PARAM VALUES WILL BE STORED IN FILES AND NOT IN SHARED PREFS
            //THE CODE AFTER THE RETURN WILL BE WHAT WILL BE USED            

            storage.UpdateItem(new Parameter(Height, WomenParameterExtractor.HeightIndex, "Height"));
            storage.UpdateItem(new Parameter(BodyType, WomenParameterExtractor.BodyTypeIndex, "BodyType"));
            storage.UpdateItem(new Parameter(Height / Weight, WomenParameterExtractor.WeightIndex, "HeightToWeight"));
            storage.UpdateItem(new Parameter(boolToInt(GenderIsMan), WomenParameterExtractor.GenderIndex, "GenderIsMan"));
            storage.UpdateItem(new Parameter(FeetLength, 4, "FeetLength"));

            return;

            await storage.UpdateItemAsync(new Parameter(Height, WomenParameterExtractor.HeightIndex, "Height"));
            await storage.UpdateItemAsync(new Parameter(BodyType, WomenParameterExtractor.BodyTypeIndex, "BodyType"));
            await storage.UpdateItemAsync(new Parameter(Height / Weight, WomenParameterExtractor.WeightIndex, "HeightToWeight"));
            await storage.UpdateItemAsync(new Parameter(boolToInt(GenderIsMan), WomenParameterExtractor.GenderIndex, "GenderIsMan"));
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
        private ParamsStore ParamStorage { get; set; }

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
                new ChooseEyeOption { Name="Голубые глаза", Description="Невероятные", ImageRessourceId="Eyes.blue_eyes.png".ToImageRessourceId() },
                new ChooseEyeOption { Name="Зелёные глаза", Description="Сногсшибательные", ImageRessourceId = "Eyes.green_eyes.png".ToImageRessourceId() },
                new ChooseEyeOption { Name="Чёрные глаза", Description="Очаровательные", ImageRessourceId = "Eyes.black_eyes.png".ToImageRessourceId() },
                new ChooseEyeOption { Name="Серые глаза", Description="Мистические", ImageRessourceId = "Eyes.gray_eyes.png".ToImageRessourceId() },
            };

            ChosenEyeOption = EyeOptions[0];

            WomanBodyTypes = new ReadOnlyCollection<string>(WomenMathModel.WomenBodyTypes);

            ParamStorage = new ParamsStore();
            ParamValues = new ParameterCollection();
        }

        public void PopulateParams()
        {
            ParamValues.InitializeFrom(ParamStorage);
        }

        public void SaveParams()
        {
            ParamValues.SaveTo(ParamStorage);

            ParamStorage.SaveData();
        }

        public void ComputePredictions()
        {
            if (MathLogic.Extractor.TryLoadParams())
            {
                bool predicted = MathLogic.TryComputePredictions().Result;
                List<double> temp;
                predicted = MathLogic.Model.TryComputePredictions(MathLogic.Extractor, out temp);
                if (predicted)
                {
                    var predictions = MathLogic.GetLastPredictions();
                    for (int index = 0; index < predictions.Count; index++)
                    {
                        string title = WomenMathModel.Clothes[index];
                        SearchByTitle(title).Rating = predictions[index];
                    }

                    MessagingCenter.Send<ParamsViewModel>(this, "Predictions computed");
                }
            }
        }

        private ItemBase SearchByTitle(string title)
        {
            //Search in UpperClothing
            if (UpperClothingStore.MainStore.GetItems().FirstOrDefault(x => x.Title == title) != null)
                return UpperClothingStore.MainStore.GetItems().First(x => x.Title == title);

            //Search in MiddleClothing
            if (MiddleClothingStore.MainStore.GetItems().FirstOrDefault(x => x.Title == title) != null)
                return MiddleClothingStore.MainStore.GetItems().First(x => x.Title == title);

            //Search in Shoes
            if (ShoesStore.MainStore.GetItems().FirstOrDefault(x => x.Title == title) != null)
                return ShoesStore.MainStore.GetItems().First(x => x.Title == title);

            //throw new ArgumentException($"No item with title {title}");
            System.Diagnostics.Debug.WriteLine($"No item with title {title}"); //Many items were not implemented
            return new UpperClothing();
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
                Text = "Главные характеристики: ",
                FontAttributes = FontAttributes.Bold
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using System.Diagnostics;

namespace RecommendationsModel
{
	public abstract class MathModel
	{
		protected readonly int ClothesCount;

		protected enum WeightValue
		{
			VeryGood = 5,
			MediumGood = 4,
			Default = 3,
			MediumBad = 2,
			VeryBad = 1
		}

		protected List<Func<object, List<double>>> ParamFunctions;
		protected List<double> ParamWeights;

		public MathModel(int cnt)
		{
			ClothesCount = cnt;
		}

		public bool TryComputePredictions(IParameterExtractor extractor, out List<double> Predictions)
		{
			Predictions = null;
			//Throw an exception
			if (ParamFunctions.Count != extractor.ParamsCount)
				throw new ArgumentException("Extractor doesn't combine with model");

			var functionResults = new List<List<double>>();
			for (int index = 0; index < extractor.ParamsCount; index++)
			{
				object pValue;
				if (extractor.TryGetValue(index + 1, out pValue))
				{
					functionResults.Add(ParamFunctions[index].Invoke(pValue));
				}
				else
				{
					//What to do here?
				}
			}

			Predictions = new List<double>();
			for (int iClothing = 0; iClothing < ClothesCount; iClothing++)
			{
				int last = 0;
				double iPred = functionResults.Sum(x => x[iClothing] * ParamWeights[last++]);
				iPred = iPred / ParamWeights.Sum();

				Predictions.Add(iPred);
			}

			return true;
		}
	}

	public class WomenMathModel : MathModel
	{
		public const int WomenClothesCount = 8 + 4 + 2; //Аксессуарный ряд + 8

		public static readonly string[] Clothes = new string[]
		{
			"Жакет", "Кардиган", "Жилет", "Платье", "Рубашка",
			"Футболка", "Топ", "Туника", "Брюки", "Джинсы", "Юбка",
			"Шорты", "Туфли", "Удобная убовь"
		};

		public static readonly string[] WomenBodyTypes = new string[]
		{
			"Песочные часы", "Треугольник (Груша)", "Перевернутый треугольник",
			"Прямоугольник", "Яблоко"
		};

		//This will be set on construction
		public readonly Dictionary<string, int> ClothesIndices;

		//Quick helper functions
		private List<double> DefautResult() => Enumerable.Repeat((double)WeightValue.Default, ClothesCount).ToList();

		private void SetValue(List<double> lst, string clothing, WeightValue value)
		{
			if (ClothesIndices.ContainsKey(clothing))
				lst[ClothesIndices[clothing]] = (double)value;
			else
				Debug.WriteLine($"WARNING: Unkown clothing piece: {clothing}");
		}


		#region ParamFunctions

		private List<double> BodyType(object value)
		{
			List<double> res = DefautResult();

			string name = value.ToString();
			
			switch (name)
			{
				case "Песочные часы":
					SetValue(res, "Балахоны", WeightValue.VeryBad);

					SetValue(res, "Платье", WeightValue.MediumGood);
					SetValue(res, "Юбка", WeightValue.MediumGood);

					SetValue(res, "Блузка", WeightValue.VeryGood);
					break;
				case "Треугольник (Груша)":
					SetValue(res, "Платье", WeightValue.MediumBad);

					SetValue(res, "Жакет", WeightValue.MediumGood);
					SetValue(res, "Футболка", WeightValue.MediumGood);
					SetValue(res, "Рубашка", WeightValue.MediumGood);
					SetValue(res, "Брюки", WeightValue.MediumGood);

					SetValue(res, "Блузка", WeightValue.VeryGood);
					SetValue(res, "Юбка", WeightValue.VeryGood);
					break;
				case "Перевернутый треугольник":
					SetValue(res, "Жакет", WeightValue.MediumBad);

					SetValue(res, "Юбка", WeightValue.MediumGood);
					SetValue(res, "Брюки", WeightValue.MediumGood);
					break;
				case "Прямоугольник":
					SetValue(res, "Юбка", WeightValue.MediumGood);
					SetValue(res, "Брюки", WeightValue.MediumGood);
					SetValue(res, "Однобортный жакет", WeightValue.MediumGood);
					break;
				case "Круг":
				case "Яблоко":
					SetValue(res, "Брюки", WeightValue.MediumGood);
					SetValue(res, "Жакет", WeightValue.MediumGood);
					break;
				default:
					Debug.WriteLine($"WARNING: Unknown body type: {name}");
					break;
			}

			return res;
		}

		private List<double> Height(object value)
		{
			List<double> res = DefautResult();

			double height = (double)value;

			if (height < 165.0) //Низкий рост
			{
				SetValue(res, "Брюки", WeightValue.MediumGood);

				SetValue(res, "Жилет", WeightValue.VeryGood);
			}

			if (height > 175.0) //Высокий рост 
			{
				SetValue(res, "Жакет", WeightValue.MediumGood);
				SetValue(res, "Юбка", WeightValue.MediumGood);

				SetValue(res, "Брюки", WeightValue.VeryGood);
			}

			return res;
		}

		private List<double> HeightToWeight(object value)
		{
			var res = DefautResult();

			//Do Something here...

			return res;
		}

		#endregion

		public WomenMathModel() : base(WomenClothesCount)
		{
			ClothesIndices = Clothes.Zip(Enumerable.Range(0, ClothesCount), (str, num) => new { key = str, value = num })
				.ToDictionary(x => x.key, x => x.value);

			ParamFunctions = new List<Func<object, List<double>>>() { BodyType, Height, HeightToWeight };

			//Let all weights sum up to 1
			ParamWeights = new List<double>() { 0.7, 0.15, 0.25 };
		}
	}
}

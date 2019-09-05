using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RecommendationsModel
{
	public abstract class MathModel
	{
		protected enum WeightValue
		{
			VeryGood = 5,
			MediumGood = 4,
			Default = 3,
			MediumBad = 2,
			VeryBad = 1
		}

		protected readonly List<Func<object, List<double>>> ParamFunctions;
		protected readonly List<double> ParamWeights;

		public bool TryComputePredictions(IParameterExtractor extractor, out List<double> Predictions)
		{
			Predictions = null;
			//Throw an exception
			if (ParamFunctions.Count != extractor.ParamsCount)
				throw new ArgumentException("Extractor doesn't combine with model");

			var functionResults = new List<List<int>>();
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
		public const int ClothesCount = 8 + 4 + 2; //Аксессуарный ряд + 8

		private readonly string[] Clothes = new string[]
		{
			"Жакет", "Кардиган", "Жилет", "Платье", "Рубашка",
			"Футболка", "Топ", "Туника", "Брюки", "Джинсы", "Юбка",
			"Шорты", "Туфли", "Удобная убовь"
		};

		//This will be set on construction
		public readonly Dictionary<string, int> ClothesIndices;

		#region ParamFunctions

		private List<double> BodyType(object value)
		{
			string name = value.ToString();

			switch (name)
			{
				case "Песочные часы":
					break;
				case "Треугольник (Груша)":
					break;
				case "Перевернутый треугольник":
					break;
				case "Прямоугольник":
					break;
				case "Круг":
					break;
			}

			return null;
		}

		private List<double> Height(object value)
		{
			double height = (double)value;

			if (height < 165.0) //Низкий рост
			{

			}

			if (height > 175.0) //Высокий рост 
			{

			}

			return null;
		}

		private List<double> HeightToWeight(object value)
		{

		}

		#endregion

		public WomenMathModel()
		{
			ClothesIndices = Clothes.Zip(Enumerable.Range(0, ClothesCount), (str, num) => new { key = str, value = num })
				.ToDictionary(x => x.key, x => x.value);

			ParamFunctions = new List<Func<object, List<double>>>() { BodyType };
		}
	}
}

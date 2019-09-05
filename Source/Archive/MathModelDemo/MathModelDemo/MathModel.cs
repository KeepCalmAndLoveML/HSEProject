using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModelDemo
{
	public class MathModel
	{
		public readonly int ClothesCount = 5;
		//TODO: The weights are a very important part of the model
		private enum WeightValue
		{
			VeryGood = 5,
			MediumGood = 4,
			Default = 3,
			MediumBad = 2,
			VeryBad = 1, 
		}

		#region Parameter functions
		//Each parameter function returns a list of length ClothesCount
		//List[i] is the recomendation value for PeaceOfClothing[i]
		//Each parameter gives its recommendation, the final decision is made in the TryComputePredictions method 

		private List<int> HeighToWeight(object value)
		{
			return Enumerable.Repeat((int)WeightValue.Default, ClothesCount).ToList();
		}

		private List<int> HeightToLegs(object value)
		{
			return Enumerable.Repeat((int)WeightValue.Default, ClothesCount).ToList();
		}

		private List<int> BodyType(object value)
		{
			return Enumerable.Repeat((int)WeightValue.Default, ClothesCount).ToList();
		}

		private List<int> Height(object value)
		{
			return Enumerable.Repeat((int)WeightValue.Default, ClothesCount).ToList();
		}

		#endregion

		private readonly List<Func<object, List<int>>> ParamFunctions;
		private readonly List<double> ParamWeights;

		public MathModel()
		{
			//Warning: Order is important
			ParamFunctions = new List<Func<object, List<int>>> { HeighToWeight, HeightToLegs, BodyType, Height};
			ParamWeights = new List<double> { 3, 2, 7, 1 };
		}

		public bool TryComputePredictions(IParameterExtractor extractor, out List<double> Predictions)
		{
			Predictions = null;
			//Throw an exception
			if(ParamFunctions.Count != extractor.ParamsCount)
				return false;
			
			var functionResults = new List<List<int>>();
			for(int index = 0; index < extractor.ParamsCount; index++)
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
			for(int iClothing = 0; iClothing < ClothesCount; iClothing++)
			{
				int last = 0;
				double iPred = functionResults.Sum(x => x[iClothing] * ParamWeights[last++]);
				iPred = iPred / ParamWeights.Sum();

				Predictions.Add(iPred);
			}

			return true;
		}
	}
}

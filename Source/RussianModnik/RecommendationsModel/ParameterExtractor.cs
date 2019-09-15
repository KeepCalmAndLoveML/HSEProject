using System;
using System.Collections.Generic;
using System.Text;

namespace RecommendationsModel
{
	public interface IParameterExtractor
	{
		int ParamsCount { get; }

		bool TryGetValue(int index, out object result);

		bool TryLoadParams();
	}

	public abstract class ParameterExtractor 
	{
		protected readonly int pCount;

		public int ParamsCount { get => pCount; }

		public object[] Preloaded;

		public bool TryGetValue(int index, out object result)
		{
			result = null;
			if (index < ParamsCount && (ParamsCount == Preloaded.Length))
			{
				result = Preloaded[index];
				return true;
			}

			return false;
		}

		public ParameterExtractor(int cnt)
		{
			pCount = cnt;
		}
	}

	public class WomenParameterExtractor : ParameterExtractor
	{
		public WomenParameterExtractor() : base(WomenMathModel.WomenClothesCount)
		{

		}
	}
}

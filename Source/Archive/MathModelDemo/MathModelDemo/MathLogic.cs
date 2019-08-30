using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Globalization;
using System.Xml.Linq;

namespace MathModelDemo
{
	//This is the class that the other classes will use for math predictions 
	public static class MathLogic
	{
		private static MathModel Model;
		private static ParameterExtractor Extractor;

		private static List<double> LastModelPredictions;

		//Make a copy of it in case it is modified...
		public static List<double> GetLastPredictions() => new List<double>(LastModelPredictions);

		public static bool Initialized { get; private set; } = false;
		public static async void Initialize(bool preload = true)
		{
			if(Initialized)
				return;

			Model = new MathModel();
			Extractor = new ParameterExtractor();
			
			if(preload)
			{
				//Make this asynchronous
				bool loaded = await ReloadParams();
				if(loaded)
					await TryComputePredictions();
			}

			Initialized = true;
		}

		public static Task<bool> ReloadParams() => Task.Run(() => Extractor.TryLoadParams());

		public static Task<bool> TryComputePredictions() => Task.Run(() => Model.TryComputePredictions(Extractor, out LastModelPredictions));

		public static Task<bool> TrySavePredictions() => Task.Run(() =>
	    {
			if(LastModelPredictions == null)
				return false;


			return true;
	    });

		private class ParameterExtractor : IParameterExtractor
		{
			public int ParamsCount { get; private set; } = 4;
			public object[] Preloaded;

			public bool TryGetValue(int index, out object result)
			{
				result = null;
				if(index < ParamsCount && ( ParamsCount == Preloaded.Length ))
				{
					result = Preloaded[index];
					return true;
				}

				return false;
			}

			public bool TryLoadParams()
			{
				//If the value of a certain param has not been passed, where should we set it's value to default?

				//The hole code above will be replaced by the following query
				if(!DataManager.IsDocumentLoaded)
					return false;

				Queue<DataManager.DataPathElement> query = new Queue<DataManager.DataPathElement>();
				query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Child, "Params"));
				query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Children, "Parameter"));
				query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Attribute, "Index"));
				query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Attribute, "Value"));

				var result = DataManager.GetValues(query);
				//Index was requested first, then were the values
				List<string> indexes = result.ElementAt(0).ToList();
				List<string> values = result.ElementAt(1).ToList();

				Preloaded = new object[ParamsCount];
				for(int i = 0; i < indexes.Count; i++)
				{
					int index;
					double value;

					if (int.TryParse(indexes[i], out index))
					{
						if(double.TryParse(values[i], out value))
							Preloaded[index - 1] = value;
						else
						{
							//Throw exception here?
						}
					}
					else
					{
						//Throw exception here?
					}
				}
				
				//Preloaded = Enumerable.Repeat(3, ParamsCount).Select(x => (object)x).ToList();
				return true;
			}

			public ParameterExtractor()
			{
			
			}
		}
	}
}

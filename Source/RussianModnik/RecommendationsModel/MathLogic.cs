using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Threading.Tasks;

namespace RecommendationsModel
{
	public static class MathLogic
	{
		public static MathModel Model;
		private static IParameterExtractor extractor;

		private static List<double> LastModelPredictions;

		//Make a copy of it in case it is modified...
		public static List<double> GetLastPredictions() => new List<double>(LastModelPredictions);

		public static bool Initialized { get; private set; } = false;
		public static IParameterExtractor Extractor
		{
			get => extractor;
			set => extractor = value;
		}

		public static async void Initialize(bool preload = true)
		{
			if (Initialized)
				return;

			if (Extractor == null || Model == null)
			{
				if (preload)
					throw new InvalidOperationException("Cannot preload with null extractor or model");

				Initialized = true;
				return;
			}

			if (preload)
			{
				//Make this asynchronous
				bool loaded = await ReloadParams();
				if (loaded)
					await TryComputePredictions();
			}

			Initialized = true;
		}

		public static Task<bool> ReloadParams() => Task.Run(() => Extractor.TryLoadParams());

		public static Task<bool> TryComputePredictions() => Task.Run(() => Model.TryComputePredictions(Extractor, out LastModelPredictions));

		public static Task<bool> TrySavePredictions() => Task.Run(() =>
		{
			if (LastModelPredictions == null)
				return false;


			return true;
		});
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Xml.Linq;

using static MathModelDemo.DataManager;

namespace MathModelDemo
{
	class Program
	{
		public static string PathToXml = "data.xml";
		
		static void Main(string[] args)
		{ 
			bool loaded = DataManager.TryReloadXml().Result;
			if(loaded)
			{
				MathLogic.Initialize(false);
				while(!MathLogic.Initialized)
					Thread.Sleep(10);
				bool paramsLoaded = MathLogic.ReloadParams().Result;
				if(paramsLoaded)
				{
					bool predicted = MathLogic.TryComputePredictions().Result;
					if(predicted)
					{
						var predictions = MathLogic.GetLastPredictions();
						Console.WriteLine("Everything works fine! Printing predictions...");
						foreach(double prediction in predictions)
							Console.Write(prediction + " ");
						Console.WriteLine();
					}
					else
						Console.WriteLine("Could not compute predictions");
				}
				else
					Console.WriteLine("Could not load params");
			}
			else
				Console.WriteLine("Could not load xml");

			Console.WriteLine("End");
			Console.WriteLine("Press enter to continue...");
			Console.ReadLine();
		}
	}
}

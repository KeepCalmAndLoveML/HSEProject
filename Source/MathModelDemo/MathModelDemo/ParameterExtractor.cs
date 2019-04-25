using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModelDemo
{
	public interface IParameterExtractor
	{
		int ParamsCount { get; set; }

		bool TryGetValue(int index, out object result);	
	}
}

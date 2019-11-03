using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

    public class WomenParameterExtractor : ParameterExtractor, IParameterExtractor
    {
        public const int ParamsCount = 3;

        public const int BodyTypeIndex = 0;
        public const int HeightIndex = 1;
        public const int WeightIndex = 2;
        public const int GenderIndex = 3; //This is not a real parameter (useless for math model since we already know the gender)

        public bool TryLoadParams()
        {
            //THIS IS A TEMPORARY SOLUTION FOR THE PROTOTYPE TO WORK.
            //IN THE FUTURE, PARAM VALUES WILL BE STORED IN FILES AND NOT IN SHARED PREFS
            //THE CODE AFTER THE RETURN WILL BE WHAT WILL BE USED
            Preloaded = new object[ParamsCount];

            Preloaded[HeightIndex] = DataManager.GetValue("Height");
            Preloaded[WeightIndex] = DataManager.GetValue("HeightToWeight");
            Preloaded[BodyTypeIndex] = DataManager.GetValue("BodyType");

            return Preloaded.All(x => !string.IsNullOrEmpty(x as string)); //DataManager.GetValue returns an empty string if the value wasn't there


            //If the value of a certain param has not been passed, where should we set it's value to default?

            //The hole code above will be replaced by the following query
            if (!DataManager.IsDocumentLoaded)
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
            for (int i = 0; i < indexes.Count; i++)
            {
                int index;

                if (int.TryParse(indexes[i], out index))
                {
                    if (index == GenderIndex)
                        continue;
                    Preloaded[index] = values[i];
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public WomenParameterExtractor() : base(ParamsCount)
        {

        }
    }
}

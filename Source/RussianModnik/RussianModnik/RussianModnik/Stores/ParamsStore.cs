using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using RecommendationsModel;

namespace RussianModnik.Stores
{
    public class Parameter
    {
        public object Value { get; private set; }
        public int Index { get; private set; }

        public string Key { get; private set; }

        public Parameter(object val, int idx, string k)
        {
            Value = val;
            Index = idx;
            Key = k;
        }
    }

    public class ParamsStore
    {
        public ParamsStore()
        {
            if (!DataManager.IsDocumentLoaded && !DataManager.IsDocumentLoading)
                DataManager.LoadDocument(Settings.SettingsFileName);

            //Set all the default params into preferences
            //Store them here for the prototype to work nicely. Clearly, the final version will use .xml files for settings
            if (!DataManager.HasKey("Height"))
                DataManager.SetValue("Height", "175");
            if (!DataManager.HasKey("HeightToWeight"))
                DataManager.SetValue("HeightToWeight", (175 / 55).ToString());
            if (!DataManager.HasKey("BodyType"))
                DataManager.SetValue("BodyType", "Песочные часы");
            if (!DataManager.HasKey("GenderIsMan"))
                DataManager.SetValue("GenderIsMan", "0");
            if (!DataManager.HasKey("FeetLength"))
                DataManager.SetValue("FeetLength", "25");
        }

        public Task<Parameter> GetItemAsync(string id)
        {
            Queue<DataManager.DataPathElement> query = new Queue<DataManager.DataPathElement>();
            //Get root element for all parameters
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Child, "Params"));

            //Get all children with name "Parameter"
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Children, "Parameter"));

            //Get their needed attributes
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Attribute, "Value"));
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Attribute, "Index"));
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Attribute, "Key"));

            return Task<Parameter>.Factory.StartNew(
            () =>
            {
                var result = DataManager.GetValues(query);
                //Order is same as in query
                List<string> values = result.ElementAt(0).ToList();
                List<int> indices = result.ElementAt(1).Select(x => int.Parse(x)).ToList();
                List<string> keys = result.ElementAt(2).ToList();

                int index = keys.IndexOf(id);
                return new Parameter(values[index], indices[index], keys[index]);
            });
        }

        public Task<IEnumerable<Parameter>> GetItemsAsync(bool forceRefresh = false)
        {
            Queue<DataManager.DataPathElement> query = new Queue<DataManager.DataPathElement>();
            //Get root element for all parameters
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Child, "Params"));

            //Get all children with name "Parameter"
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Children, "Parameter"));

            //Get their needed attributes
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Attribute, "Value"));
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Attribute, "Index"));
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Attribute, "Key"));

            return Task<IEnumerable<Parameter>>.Factory.StartNew(() =>
            {
                var result = DataManager.GetValues(query);
                //Order is same as in query
                List<string> values = result.ElementAt(0).ToList();
                List<int> indices = result.ElementAt(1).Select(x => int.Parse(x)).ToList();
                List<string> keys = result.ElementAt(2).ToList();

                List<Parameter> res = new List<Parameter>();
                for (int i = 0; i < values.Count; i++)
                    res.Add(new Parameter(values[i], indices[i], keys[i]));

                return res;
            });
        }

        //THIS IS A TEMPORARY SOLUTION FOR THE PROTOTYPE TO WORK.
        //IN THE FUTURE, PARAM VALUES WILL BE STORED IN FILES AND NOT IN SHARED PREFS
        public IEnumerable<Parameter> GetItems()
        {
            string height = DataManager.GetValue("Height");
            string weight = DataManager.GetValue("HeightToWeight");
            string bodyType = DataManager.GetValue("BodyType");
            string gender = DataManager.GetValue("GenderIsMan");
            string feetLength = DataManager.GetValue("FeetLength");

            yield return new Parameter(bodyType, 0, "BodyType");
            yield return new Parameter(height, 1, "Height");
            yield return new Parameter(weight, 2, "HeightToWeight");
            yield return new Parameter(gender, 3, "GenderIsMan");
            yield return new Parameter(feetLength, 4, "FeetLength");
        }

        //Change Value of a Parameter
        public Task<bool> UpdateItemAsync(Parameter item)
        {
            Queue<DataManager.DataPathElement> query = new Queue<DataManager.DataPathElement>();
            //Get root element for all parameters
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Child, "Params"));

            //Amongst all children, find one whose index matches the needed one 
            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Children, "Parameter",
                x => x.Attribute("Index").Value.ToString() == item.Index.ToString()));

            query.Enqueue(new DataManager.DataPathElement(DataManager.DataType.Attribute, "Value"));

            return Task<bool>.Factory.StartNew
            (
                () =>
                {
                    try
                    {
                        DataManager.SetValue(query, item.Value.ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            );

        }

        //THIS IS A TEMPORARY SOLUTION FOR THE PROTOTYPE TO WORK.
        //IN THE FUTURE, PARAM VALUES WILL BE STORED IN FILES AND NOT IN SHARED PREFS
        public bool UpdateItem(Parameter item)
        {
            DataManager.SetValue(item.Key, item.Value.ToString());

            return true;
        }

        public async void SaveData()
        {
            //Uncomment this after prototype is shown 
            //await DataManager.SaveXml(Settings.SettingsFileName);
        }
    }
}

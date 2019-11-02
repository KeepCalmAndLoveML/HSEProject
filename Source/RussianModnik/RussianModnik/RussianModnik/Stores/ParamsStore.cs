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

        public async void SaveData()
        {
            await DataManager.SaveXml(Settings.SettingsFileName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;
using System.IO;

using System.Reflection;

using Xamarin.Essentials;

namespace RecommendationsModel
{
    //This is needed because different data may be managed differently depending on the platform
    //Also, all data management should be run asynchronously
    public static class DataManager
    {
        public readonly static string BaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        private static XDocument LastDocument;

        public static bool IsDocumentLoaded => LastDocument != null && !IsDocumentLoading;

        public static bool IsDocumentLoading { get; private set; }

        public async static void LoadDocument(string path)
        {
            IsDocumentLoading = true;

            path = Path.Combine(BaseFolder, path);
            try
            {
                LastDocument = XDocument.Load(path);
            }
            catch //Wrong path, load default setting
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(DataManager)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("RussianModnik.settings.xml");
                LastDocument = XDocument.Load(stream);
            }

            IsDocumentLoading = false;
            System.Diagnostics.Debug.WriteLine("done");
        }

        //Gets value for shared preference
        public static string GetValue(string key)
        {
            return Preferences.Get(key, string.Empty);
        }

        public static void SetValue(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public static bool HasKey(string key)
        {
            return Preferences.ContainsKey(key);
        }

        public static List<string> GetValue(Queue<DataPathElement> query)
        {
            if (!IsDocumentLoaded)
                throw new InvalidOperationException("Data was not loaded");

            List<string> res = new List<string>();
            XElement child = LastDocument.Root;
            while (query.Count > 0)
            {
                DataPathElement next = query.Dequeue();

                switch (next.DataType)
                {
                    case DataType.Child:
                        child = child.Element(next.Key);
                        break;
                    case DataType.Attribute:
                        res.Add(child.Attribute(next.Key).Value);
                        break;
                    case DataType.Value:
                        res.Add(child.Value);
                        break;
                    default:
                        throw new NotImplementedException($"Datatype {next.DataType} not implemented");
                }
            }

            return res;
        }

        public static List<IEnumerable<string>> GetValues(Queue<DataPathElement> query)
        {
            if (!IsDocumentLoaded)
                throw new InvalidOperationException("Data was not loaded");

            List<IEnumerable<string>> res = new List<IEnumerable<string>>();

            XElement child = LastDocument.Root;
            List<XElement> children = null;
            while (query.Count > 0)
            {
                DataPathElement next = query.Dequeue();

                //Console.WriteLine($"{next.Key} {next.DataType}");
                switch (next.DataType)
                {
                    case DataType.Children:
                        if (child == null)
                            throw new NotImplementedException("Getting 2D children array is not implemented");

                        children = child.Elements(next.Key).ToList();
                        child = null;
                        break;
                    case DataType.Child:
                        if (child == null)
                            children = children.Select(x => x.Element(next.Key)).ToList();
                        else if (children == null)
                        {
                            //This statement is for when the document only has one collection of elements
                            //So the root of the document automatically becomes the root of this collection
                            //Which breaks the query
                            if (next.Key != child.Name)
                                child = child.Element(next.Key);
                        }
                        else
                            throw new Exception("Execution error");

                        break;
                    case DataType.Attribute:
                        if (children == null)
                            res.Add(new List<XElement>() { child }.Select(x => x.Attribute(next.Key).Value));
                        else
                            res.Add(children.Select(x => x.Attribute(next.Key).Value));
                        break;
                    case DataType.Value:
                        if (children == null)
                            res.Add(new List<XElement>() { child }.Select(x => x.Value));
                        else
                            res.Add(children.Select(x => x.Value));
                        break;
                    default:
                        throw new NotImplementedException($"Datatype {next.DataType} not implemented");
                }

                if (child == null && children == null)
                    throw new InvalidOperationException("Invalid query");
            }

            return res;
        }

        public static void SetValue(Queue<DataPathElement> query, string value)
        {
            if (!IsDocumentLoaded)
                throw new InvalidOperationException("Data was not loaded");

            XElement child = LastDocument.Root;
            List<XElement> children = null;
            while (query.Count > 0)
            {
                DataPathElement next = query.Dequeue();

                switch (next.DataType)
                {
                    case DataType.Children:
                        if (child == null)
                            throw new NotImplementedException("Getting 2D children array is not implemented");

                        children = child.Elements(next.Key).ToList();
                        child = null;
                        break;
                    case DataType.Child:
                        if (child == null)
                            children = children.Select(x => x.Element(next.Key)).ToList();
                        else if (children == null)
                        {
                            //This statement is for when the document only has one collection of elements
                            //So the root of the document automatically becomes the root of this collection
                            //Which breaks the query
                            if (next.Key != child.Name)
                                child = child.Element(next.Key);
                        }
                        break;

                    case DataType.Value:
                        if (child == null)
                            children.ForEach(x => x.SetValue(value));
                        else
                            child.SetValue(value);
                        break;
                    case DataType.Attribute:
                        if (child == null)
                            children.ForEach(x => x.Attribute(next.Key).SetValue(value));
                        else
                            child.Attribute(next.Key).SetValue(value);
                        break;
                    default:
                        throw new NotImplementedException($"Datatype {next.DataType} not implemented");
                }

                if (children != null && next.Filter != null)
                    children = children.Where(x => next.Filter.Invoke(x)).ToList();

                if (child == null && children == null)
                    throw new InvalidOperationException("Invalid query");
            }
        }

        public static Task SaveXml(string path)
        {
            return Task.Run(() =>
            {
                string backupPath = $"Backup_{path}";
                path = Path.Combine(BaseFolder, path);
                backupPath = Path.Combine(BaseFolder, path);
                
                /*
                if (File.Exists(path))
                {
                    System.Diagnostics.Debug.WriteLine($"Warning, overwriting file at {path}");
                    File.Create(backupPath);
                    File.Copy(path, backupPath);
                    File.Delete(path);
                }
                */

                try
                {
                    LastDocument.Save(path);
                    System.Diagnostics.Debug.WriteLine($"Successfully saved file at {path}.");
                }
                catch 
                {
                    System.Diagnostics.Debug.WriteLine($"Exception thrown while trying to save file at {path}. Reverting to backup");
                    if (!File.Exists(path))
                        File.Create(path);

                    File.Copy(backupPath, path);
                }
                finally
                {
                    File.Delete(backupPath);
                }
            });
        }

        public enum DataType
        {
            Child,
            Children,
            Attribute,
            Value,
        }

        public struct DataPathElement
        {
            public string Key { get; set; }
            public DataType DataType { get; set; }

            public Func<XElement, bool> Filter { get; set; }

            public DataPathElement(DataType type, string k = null, Func<XElement, bool> filter = null)
            {
                if (string.IsNullOrEmpty(k))
                {
                    if (type != DataType.Value)
                        throw new ArgumentException("Key cannot be null or empty");

                    Key = string.Empty;
                }
                else
                    Key = k;

                DataType = type;
                Filter = filter;
            }
        }
    }
}

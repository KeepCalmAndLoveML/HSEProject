using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

namespace MathModelDemo
{
	//This is needed because different data may be managed differently depending on the platform
	//Also, all data management should be run asynchronously
	public static class DataManager
	{
		private static XDocument LastDocument;

		public static bool IsDocumentLoaded => LastDocument != null && !IsDocumentLoading;

		public static bool IsDocumentLoading { get; private set; } 
		//This will probably have a switch statement which would depend on the platform
		private static bool LoadDocument()
		{
			IsDocumentLoading = true;
			bool result;
			try
			{
				LastDocument = XDocument.Load(Program.PathToXml);
				result = true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				result = false;
			}

			IsDocumentLoading = false;
			return result;
		}

		public static List<string> GetValue(Queue<DataPathElement> query)
		{
			if(!IsDocumentLoaded)
				throw new InvalidOperationException("Data was not loaded");

			List<string> res = new List<string>();
			XElement child = LastDocument.Root;
			while(query.Count > 0)
			{
				DataPathElement next = query.Dequeue();

				switch(next.DataType)
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
			if(!IsDocumentLoaded)
				throw new InvalidOperationException("Data was not loaded");

			List<IEnumerable<string>> res = new List<IEnumerable<string>>();

			XElement child = LastDocument.Root;
			List<XElement> children = null;
			while(query.Count > 0)
			{
				DataPathElement next = query.Dequeue();

				//Console.WriteLine($"{next.Key} {next.DataType}");
				switch(next.DataType)
				{
					case DataType.Children:
						if(child == null)
							throw new NotImplementedException("Getting 2D children array is not implemented");

						children = child.Elements(next.Key).ToList();
						child = null;
						break;
					case DataType.Child:
						if(child == null)
							children = children.Select(x => x.Element(next.Key)).ToList();
						else if(children == null)
						{
							//This statement is for when the document only has one collection of elements
							//So the root of the document automatically becomes the root of this collection
							//Which breaks the query
							if(next.Key != child.Name)
								child = child.Element(next.Key);
						}
						else
							throw new Exception("Execution error");

						break;
					case DataType.Attribute:
						if(children == null)
							res.Add(new List<XElement>() { child }.Select(x => x.Attribute(next.Key).Value));
						else
							res.Add(children.Select(x => x.Attribute(next.Key).Value));
						break;
					case DataType.Value:
						if(children == null)
							res.Add(new List<XElement>() { child }.Select(x => x.Value));
						else
							res.Add(children.Select(x => x.Value));
						break;
					default:
						throw new NotImplementedException($"Datatype {next.DataType} not implemented");
				}

				if(child == null && children == null)
					throw new InvalidOperationException("Invalid query");
			}

			return res;
		}


		public static Task<bool> TryReloadXml() => Task.Run(() => LoadDocument());
		
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

			public DataPathElement(DataType type, string k = null)
			{
				if(string.IsNullOrEmpty(k))
				{
					if(type != DataType.Value)
						throw new ArgumentException("Key cannot be null or empty");

					Key = string.Empty;
				}
				else
					Key = k;

				DataType = type;
			}
		}

		public struct DataElement
		{
			public IReadOnlyDictionary<string, string> AttributeValues;

			public string Name { get; set; }

			public DataElement(string name, IReadOnlyDictionary attributes)
			{
				Name = name;
				AttributeValues = attributes;
			}
		}
	}
}

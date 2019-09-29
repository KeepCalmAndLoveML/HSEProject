using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

using Xamarin.Forms;
using SkiaSharp.Views.Forms;

namespace RussianModnik
{
	// Extensions for SkiaSharp
	public static class SkiaSharpTools
	{
		public static Rectangle FromPixels(this SKCanvasView skCanvasView, Rectangle rc) =>
			new Rectangle(skCanvasView.FromPixels(rc.Location), skCanvasView.FromPixels(rc.Size));

		public static Size FromPixels(this SKCanvasView skCanvasView, Size rc) =>
			(Size)skCanvasView.FromPixels(new Point(rc.Width, rc.Height));

		public static Point FromPixels(this SKCanvasView skCanvasView, Point pt)
		{
			double wf = skCanvasView.CanvasSize.Width / skCanvasView.Width;
			double hf = skCanvasView.CanvasSize.Height / skCanvasView.Height;
			return new Point(pt.X * wf, pt.Y * hf);
		}
	}

	//Extensions for Xamarin.Forms controls
	public static class UIExtensions
	{
		public static bool TryGetSelectedItem<T>(this ListView ls, SelectedItemChangedEventArgs eventArgs, out T result) where T : class
		{
			result = eventArgs.SelectedItem as T;
			if (result == null)
				return false;

			// Manually deselect item.
			ls.SelectedItem = null;
			return true;
		}
	}

	//Extensions for basic C# classes
	public static class SimpleExtensions
	{
		public static string[] Ressources = typeof(SimpleExtensions).Assembly.GetManifestResourceNames();

		public static string ToImageRessourceId(this string filename)
		{
			//Those checks might be slow, redundant and unnecessary...
			//But this ressource thing seems scary not throwing exceptions, so I might as well leave them here

			if (string.IsNullOrEmpty(filename))
				throw new ArgumentException("Ressource name cannot be null or empty");
			//I decided jpg might also be nice for memory saving.
			//if (Path.GetExtension(filename) != "png")
			//throw new ArgumentException("Are you sure you want to use non png files?");
			if (Ressources.Where(x => x.Contains(filename)).Count() == 0)
				throw new ArgumentException("Invalid ressource name");

			return $"{typeof(SimpleExtensions).Assembly.GetName().Name}.Images.{filename}";
		}

		public static TimeSpan Multiply(this TimeSpan timeSpan, double coef)
		{
			return TimeSpan.FromMilliseconds(timeSpan.TotalMilliseconds * coef);
		}

		public static string GetHexString(this Xamarin.Forms.Color color)
		{
			var red = (int)(color.R * 255);
			var green = (int)(color.G * 255);
			var blue = (int)(color.B * 255);
			var alpha = (int)(color.A * 255);
			var hex = $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";

			return hex;
		}
	}

	public static class ArrayExtensions
	{
		public class ArrayTraverse
		{
			public int[] Position;
			private int[] maxLengths;

			public ArrayTraverse(Array array)
			{
				maxLengths = new int[array.Rank];
				for (int i = 0; i < array.Rank; ++i)
				{
					maxLengths[i] = array.GetLength(i) - 1;
				}
				Position = new int[array.Rank];
			}

			public bool Step()
			{
				for (int i = 0; i < Position.Length; ++i)
				{
					if (Position[i] < maxLengths[i])
					{
						Position[i]++;
						for (int j = 0; j < i; j++)
						{
							Position[j] = 0;
						}
						return true;
					}
				}
				return false;
			}
		}

		public static void ForEach(this Array array, Action<Array, int[]> action)
		{
			if (array.LongLength == 0) return;
			ArrayTraverse walker = new ArrayTraverse(array);
			do action(array, walker.Position);
			while (walker.Step());
		}
	}

	public static class ObjectExtensions
	{
		private static readonly MethodInfo CloneMethod = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

		public static bool IsPrimitive(this Type type)
		{
			if (type == typeof(String)) return true;
			return (type.IsValueType & type.IsPrimitive);
		}

		public static Object Copy(this Object originalObject)
		{
			return InternalCopy(originalObject, new Dictionary<Object, Object>(new ReferenceEqualityComparer()));
		}
		private static Object InternalCopy(Object originalObject, IDictionary<Object, Object> visited)
		{
			if (originalObject == null) return null;
			var typeToReflect = originalObject.GetType();
			if (IsPrimitive(typeToReflect)) return originalObject;
			if (visited.ContainsKey(originalObject)) return visited[originalObject];
			if (typeof(Delegate).IsAssignableFrom(typeToReflect)) return null;
			var cloneObject = CloneMethod.Invoke(originalObject, null);
			if (typeToReflect.IsArray)
			{
				var arrayType = typeToReflect.GetElementType();
				if (IsPrimitive(arrayType) == false)
				{
					Array clonedArray = (Array)cloneObject;
					clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
				}

			}
			visited.Add(originalObject, cloneObject);
			CopyFields(originalObject, visited, cloneObject, typeToReflect);
			RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
			return cloneObject;
		}

		private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
		{
			if (typeToReflect.BaseType != null)
			{
				RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
				CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
			}
		}

		private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
		{
			foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
			{
				if (filter != null && filter(fieldInfo) == false) continue;
				if (IsPrimitive(fieldInfo.FieldType)) continue;
				var originalFieldValue = fieldInfo.GetValue(originalObject);
				var clonedFieldValue = InternalCopy(originalFieldValue, visited);
				fieldInfo.SetValue(cloneObject, clonedFieldValue);
			}
		}
		public static T Copy<T>(this T original)
		{
			return (T)Copy((Object)original);
		}
	}

	public class ReferenceEqualityComparer : EqualityComparer<Object>
	{
		public override bool Equals(object x, object y)
		{
			return ReferenceEquals(x, y);
		}
		public override int GetHashCode(object obj)
		{
			if (obj == null) return 0;
			return obj.GetHashCode();
		}
	}
}

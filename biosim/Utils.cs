using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Biosim
{
	public static class Utils
	{
		static Utils()
		{
		}

		public static Stream LoadResource(string name)
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
		}

		public static void Serialize(object o)
		{
			PropertyInfo[] pi = o.GetType().GetProperties();
			foreach (var p in pi) {
				object[] attrs = p.GetCustomAttributes(true);
				foreach (object attr in attrs) {
					SerializableAttribute serAttr = attr as SerializableAttribute;
					if (serAttr != null) {
						string propName = p.Name;
						System.Console.Write(propName + "; ");
						System.Console.WriteLine(serAttr.ToString());
					}
				}
			}
		}
	}
}


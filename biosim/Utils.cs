using System;
using System.Drawing;
using System.IO;

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
	}
}


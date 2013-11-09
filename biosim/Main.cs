using System;
using System.Windows.Forms;
using Biosim.UI;
using Biosim.Abstraction;
using Biosim.Implementation;

namespace Biosim
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.Run(new MainWindow());
		}
	}
}

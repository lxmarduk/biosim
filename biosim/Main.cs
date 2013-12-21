using System.Windows.Forms;
using Biosim.UI;
using Biosim.Implementation;
using System.Drawing;

namespace Biosim
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Cell c = new Cell("Dummy");
			c.Properties ["Alive"].Set();
			c.Shape = Cell.CellShape.Circle;
			Utils.Serialize(c, "Dummy");

			Cell d = new Cell("Red mist");
			d.Properties ["Alive"].Set();
			d.Shape = Cell.CellShape.Diamond;
			d.Color = Color.Red;
			Utils.Serialize(d, "Red mist");

			Application.EnableVisualStyles();
			Application.Run(new MainWindow());
		}
	}
}

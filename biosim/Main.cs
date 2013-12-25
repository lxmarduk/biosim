using System;
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
			/*Form f = new Form();
			f.FormBorderStyle = FormBorderStyle.FixedDialog;
			f.AutoSize = true;
			f.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			f.Margin = new Padding(8);
			f.StartPosition = FormStartPosition.CenterScreen;
			Cell c = new Cell("Test cell");
			RuleLogicalWidget r = new RuleLogicalWidget();
			r.Parent = f;
			r.EditableCell = (Cell)c.Clone();
			Application.Run(f);
			Console.WriteLine(r.Rule);
			//*/
			Application.EnableVisualStyles();
			Application.Run(new MainWindow());
			//*/
		}
	}
}

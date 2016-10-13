using System;
using System.Windows.Forms;
using Biosim.UI;
using Biosim.Implementation;
using System.Drawing;

namespace Biosim
{
	class MainClass
	{
        [STAThread]
		public static void Main(string[] args)
		{
            /*Map m = new Map(10, 10);
			m.Type = Map.MapType.Continuous;
			EditMapPropertiesDialog d = new EditMapPropertiesDialog();
			d.EditableMap = m;
			d.Show();
			d.Dispose();//*/

            Application.EnableVisualStyles();
			Application.Run(new MainWindow());
		}
	}
}

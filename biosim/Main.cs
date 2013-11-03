using System;
using Gtk;

namespace Biosim
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Biosim.Implementation.PropertyCollection coll = new Biosim.Implementation.PropertyCollection();
			coll.Add(new Biosim.Implementation.StringProperty("String", "StringValue"));
			coll.Add(new Biosim.Implementation.IntProperty("Integer", 665));
			coll.Add(new Biosim.Implementation.FloatProperty("Float", 3.1415f));
			coll.Add(new Biosim.Implementation.BoolProperty("Bool", true));
			
			foreach(var i in coll) {
				Debug.PrintInfo(i.Value.ToString());
			}
			
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}

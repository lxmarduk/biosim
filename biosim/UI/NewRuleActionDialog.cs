using System;
using System.Windows.Forms;
using System.Drawing;

namespace Biosim
{
	public static class NewRuleActionDialog
	{
		static Form f;

		static NewRuleActionDialog()
		{
			initializeUI();
		}

		static void initializeUI()
		{
			f = new Form();
			f.FormBorderStyle = FormBorderStyle.FixedDialog;
			f.AutoSize = true;
			f.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			f.Margin = new Padding(8);
			f.StartPosition = FormStartPosition.CenterScreen;
		}
	}
}


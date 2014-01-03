using System;
using System.Windows.Forms;
using System.Drawing;

namespace Biosim
{
	public sealed class NewRuleActionDialog : Form
	{
		public NewRuleActionDialog()
		{
			initializeUI();
		}

		void initializeUI()
		{
			FormBorderStyle = FormBorderStyle.FixedDialog;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Margin = new Padding(8);
			StartPosition = FormStartPosition.CenterScreen;
		}
	}
}


using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Biosim.Implementation;
using System.IO;

namespace Biosim.UI
{
	public class SideBar : Panel
	{
		CellView view;
		Button btnAddNewCell;
		Button btnRemoveCell;
		Button btnCellProps;

		public CellView Cells {
			get {
				return view;
			}
		}

		public SideBar(Control control) : base()
		{
			Parent = control;
			Size = new Size(300, 150);
			AutoScroll = true;

			initializeUI();
		}

		void initializeUI()
		{
			view = new CellView(this);

            btnAddNewCell = new Button();
			btnAddNewCell.Image = new Bitmap(Utils.LoadResource("list-add"));
			btnAddNewCell.Location = new Point(3, view.Bottom);
			btnAddNewCell.Width = 36;
			btnAddNewCell.Height = 36;
			btnAddNewCell.FlatStyle = FlatStyle.Standard;
			btnAddNewCell.Parent = this;
			btnAddNewCell.Click += (sender, e) => {
				Cell c;
				NewCellDialog newCellDlg = new NewCellDialog();
				if (newCellDlg.Show() == DialogResult.OK) {
					if (newCellDlg.Cell != null) {
						c = (Cell)newCellDlg.Cell.Clone();
						Utils.Serialize(c, c.Properties ["Ім'я"].Value.ToString());
						view.Reload();
					} else {
						MessageBox.Show("Чогось не можу створити нову клітину.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				newCellDlg.Dispose();
			};

			btnRemoveCell = new Button();
			btnRemoveCell.Image = new Bitmap(Utils.LoadResource("list-remove"));
			btnRemoveCell.Width = 36;
			btnRemoveCell.Height = 36;
			btnRemoveCell.Location = new Point(btnAddNewCell.Right + 5, btnAddNewCell.Top);
			btnRemoveCell.FlatStyle = FlatStyle.Standard;
			btnRemoveCell.Parent = this;
			btnRemoveCell.Click += (sender, e) => {
				int index = view.View.SelectedIndex;
				if (index != -1) {
					if (MessageBox.Show("Ви дійсно хочете видалити клітину? Клітина зникне назавжди.", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
						String path = "cells/" + view.SelectedCell.Properties ["Ім'я"].Value + ".bin";
						if (File.Exists(path)) {
							File.Delete(path);
						} else {
							view.RemoveCell(index);
						}
						view.Reload();
					}
				}
			};

			btnCellProps = new Button();
			btnCellProps.Image = new Bitmap(Utils.LoadResource("document-properties"));
			btnCellProps.Width = 36;
			btnCellProps.Height = 36;
			btnCellProps.Location = new Point(view.Right - btnCellProps.Width, btnRemoveCell.Top);
			btnCellProps.FlatStyle = FlatStyle.Standard;
			btnCellProps.Parent = this;
			btnCellProps.Click += (sender, e) => {
				int index = view.View.SelectedIndex;
				if (index != -1) {
					Cell editableCell = (Cell)view.View.Items [index];
					string prevName = editableCell.Properties ["Ім'я"].Value.ToString();
					EditCellDialog edtCell = new EditCellDialog();
					edtCell.EditableCell = editableCell;
					if (edtCell.Show() == DialogResult.OK) {
						Cell c = (Cell)edtCell.EditableCell.Clone();
						if (!c.Properties ["Ім'я"].Equ(prevName)) {
							File.Delete("cells/" + prevName + ".bin");
						}
						Utils.Serialize(c, c.Properties ["Ім'я"].Value.ToString());
						view.Reload();
						for (int i = 0; i < view.View.Items.Count; ++i) {
							Cell t = (Cell)view.View.Items [i];
							if (t.Properties ["Ім'я"].Equ(c.Properties ["Ім'я"].Value)) {
								view.View.SelectedIndex = i;
								break;
							}
						}
					}
					edtCell.Dispose();
					view.Reload();
				}
			};
		}
	}
}


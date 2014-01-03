using System;
using System.Windows.Forms;
using Biosim.Abstraction;
using Biosim.Implementation;
using System.Drawing;

namespace Biosim.UI
{
	public sealed class EditCellDialog : Form
	{
		Cell editableCell;
		CellPreviewWidget preview;
		ListBox propsList;
		EditPropertyWidget propEdit;
		Button acceptEdit;
		ComboBox cb_shape;
		ColorWidget color;
		Button btnAdd;
		Button btnRemove;
		Button allGood /*australian style*/;
		Button cancel;

		public Cell EditableCell {
			get {
				return (Cell)editableCell.Clone();
			}
			set {
				editableCell = (Cell)value.Clone();
			}
		}

		public EditCellDialog()
		{
			initializeUI();
		}

		void initializeUI()
		{
			Text = "Редагувати клітину";
			FormBorderStyle = FormBorderStyle.FixedDialog;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Margin = new Padding(8);
			StartPosition = FormStartPosition.CenterScreen;


			propsList = new ListBox();
			propsList.Location = new Point(4, 4);
			propsList.Width = 150;
			propsList.Height = 200;
			propsList.Parent = this;
			propsList.Sorted = true;
			propsList.SelectedIndexChanged += (sender, e) => {
				if (propsList.SelectedIndex == -1) {
					return;
				}

				propEdit.Property = editableCell.Properties [propsList.Items [propsList.SelectedIndex].ToString()];
			};

			propEdit = new EditPropertyWidget();
			propEdit.Location = new Point(propsList.Right + 15, propsList.Top);
			propEdit.Parent = this;
			propEdit.CanSaveDataChanged += (sender, e) => acceptEdit.Enabled = propEdit.CanSaveData;

			acceptEdit = new Button();
			acceptEdit.Text = "Прийняти";
			acceptEdit.Location = new Point(propEdit.Right - acceptEdit.Width, propEdit.Bottom + 5);
			acceptEdit.Parent = this;
			acceptEdit.Click += (sender, e) => {
				propEdit.SaveData();
				string oldParamName = propsList.SelectedItem.ToString();
				editableCell.Properties.Remove(oldParamName);
				editableCell.Properties.Add(propEdit.Property.Clone());
				string newName = propEdit.Property.Name;
				loadProps();
				for (int i = 0; i < propsList.Items.Count; ++i) {
					if (propsList.Items [i].ToString().Equals(newName)) {
						propsList.SelectedIndex = i;
						break;
					}
				}
			};

			btnAdd = new Button();
			btnAdd.Image = new Bitmap(Utils.LoadResource("list-add"));
			btnAdd.Size = new Size(36, 36);
			btnAdd.Location = new Point(propsList.Right + 10, propsList.Bottom - btnAdd.Height);
			btnAdd.Parent = this;
			btnAdd.Click += AddPropClick;

			btnRemove = new Button();
			btnRemove.Image = new Bitmap(Utils.LoadResource("list-remove"));
			btnRemove.Size = new Size(36, 36);
			btnRemove.Location = new Point(btnAdd.Right + 10, btnAdd.Top);
			btnRemove.Parent = this;
			btnRemove.Click += RemovePropClick;

			preview = new CellPreviewWidget();
			preview.Location = new Point(4, propsList.Bottom + 16);
			preview.Parent = this;

			Label lbl_shape = new Label();
			lbl_shape.Text = "Форма:";
			lbl_shape.Location = new Point(preview.Right + 5, preview.Top + 2);
			lbl_shape.Parent = this;
			cb_shape = new ComboBox();
			cb_shape.Items.Add("Квадрат");
			cb_shape.Items.Add("Круг");
			cb_shape.Items.Add("Трикутник");
			cb_shape.Items.Add("Ромб");
			cb_shape.Width = 150;
			cb_shape.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_shape.SelectedIndex = 0;
			cb_shape.Location = new Point(lbl_shape.Right + 5, lbl_shape.Top - 2);
			cb_shape.Parent = this;
			cb_shape.SelectedIndexChanged += HandleSelectedIndexChanged;

			color = new ColorWidget();
			color.Location = new Point(lbl_shape.Left, lbl_shape.Bottom + 5);
			color.Parent = this;
			color.ColorChanged += (sender, e) => {
				editableCell.Color = color.Color;
				preview.PreviewCell = editableCell;
			};

			allGood = new Button();
			allGood.Text = "OK";
			allGood.Location = new Point(Width / 2 + 5, color.Bottom + 5);
			allGood.DialogResult = DialogResult.OK;
			allGood.Parent = this;

			cancel = new Button();
			cancel.Text = "Cancel";
			cancel.Location = new Point(Width / 2 - cancel.Width - 5, color.Bottom + 5);
			cancel.DialogResult = DialogResult.Cancel;
			cancel.Parent = this;
		}

		void loadProps()
		{
			if (EditableCell == null) {
				return;
			}
			propsList.Items.Clear();
			foreach (AbstractProperty p in editableCell.Properties) {
				propsList.Items.Add(p.Name);
			}
			propsList.SelectedIndex = 0;
		}

		public new DialogResult Show()
		{
			preview.PreviewCell = editableCell;
			color.Color = editableCell.Color;
			cb_shape.SelectedIndex = (int)editableCell.Shape;
			loadProps();
			return ShowDialog();
		}

		void HandleSelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cb_shape.SelectedIndex) {
				case 0: //square
					editableCell.Shape = Cell.CellShape.Square;
					break;
				case 1: //circle
					editableCell.Shape = Cell.CellShape.Circle;
					break;
				case 2: //triangle
					editableCell.Shape = Cell.CellShape.Triangle;
					break;
				case 3: //diamond
					editableCell.Shape = Cell.CellShape.Diamond;
					break;
			}
			preview.PreviewCell = editableCell;
		}

		void AddPropClick(object sender, EventArgs e)
		{
			NewPropertyDialog newProp = new NewPropertyDialog();
			if (newProp.Show() == DialogResult.OK) {
				AbstractProperty p = newProp.Property;
				if (p != null) {
					if (!editableCell.Properties.HasProperty(p.Name)) {
						editableCell.Properties.Add(p.Clone());
						loadProps();
						for (int i = 0; i < propsList.Items.Count; ++i) {
							if (propsList.Items [i].ToString().Equals(p.Name)) {
								propsList.SelectedIndex = i;
								break;
							}
						}
					} else {
						MessageBox.Show("Дана клітина вже має властивість з таким іменем. Нічого не додано.", "Зауваження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
			}
			newProp.Dispose();
		}

		void RemovePropClick(object sender, EventArgs e)
		{
			int index = propsList.SelectedIndex;
			if (index == -1) {
				return;
			}
			string name = propsList.SelectedItem.ToString();
			if (name.Equals("Ім'я") || name.Equals("Жива") || name.Equals("Сусіди")) {
				MessageBox.Show("Неможливо видалити цю властивість, вона призначена для магії.", "Зауваження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			editableCell.Properties.Remove(propsList.SelectedItem.ToString());
			propsList.Items.RemoveAt(propsList.SelectedIndex);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (Controls != null) {
					for (int i = 0; i < Controls.Count; ++i) {
						Controls [i].Dispose();
					}
				}
			}
			base.Dispose(disposing);
		}
	}
}


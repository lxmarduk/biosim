﻿using System;
using System.Drawing;
using Biosim.Implementation;
using System.Text;

namespace Biosim.Abstraction
{
	[Serializable]
	public abstract class AbstractCell
	{
		readonly PropertyCollection props;

		public PropertyCollection Properties {
			get {
				return props;
			}
		}

		protected AbstractCell(String name)
		{
			props = new PropertyCollection();
			props.Add(new StringProperty("Ім'я", name));
			props.Add(new BoolProperty("Жива", false));
			props.Add(new IntProperty("Сусіди", (int)0));
		}

		public abstract AbstractCell Clone();

		public abstract void Draw(Graphics g, Rectangle bounds);

		public override string ToString()
		{
			StringBuilder b = new StringBuilder();
			b.Append(this.GetType().Name);
			b.Append("\n");
			foreach (AbstractProperty p in Properties) {
				b.Append("\t");
				b.Append(p.Name);
				b.Append(": ");
				b.Append(p.Value.ToString());
				b.Append("\n");
			}
			return b.ToString();
		}
	}
}

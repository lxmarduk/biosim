using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	public class Cell : AbstractCell
	{
		public Cell(string name) : base(name)
		{
		}

		#region implemented abstract members of Biosim.Abstraction.AbstractCell
		public override AbstractCell Clone()
		{
			Cell result = new Cell(this.Properties ["Name"].Value.ToString());
			foreach (AbstractProperty prop in Properties) {
				result.Properties.Add(prop.Clone());
			}
			return result;
		}
		#endregion

	}
}


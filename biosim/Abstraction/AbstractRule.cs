using System;

namespace Biosim.Abstraction
{
	[Serializable]
	public abstract class AbstractRule
	{
		protected readonly String property;
		protected readonly object equalityValue;

		public String Property {
			get {
				return property;
			}
		}

		protected AbstractRule(String property, object expr)
		{
			this.property = property;
			equalityValue = expr;
		}

		public virtual bool Check(AbstractCell cell)
		{
			return false;
		}
	}
}


using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public class RuleEvaluation : IRule
	{
		readonly String property;
		readonly object equalityValue;

		public String Property {
			get {
				return property;
			}
		}

		public object EqualityValue {
			get {
				return equalityValue;
			}
		}

		public RuleEvaluation(String propName, object equValue)
		{
			property = propName;
			equalityValue = equValue;
		}

		#region IRule implementation

		public bool Check(AbstractCell cell)
		{
			return cell.Properties.HasProperty(property) && Compare(cell);
		}

		#endregion

		protected virtual bool Compare(AbstractCell cell)
		{
			return false;
		}
	}
}


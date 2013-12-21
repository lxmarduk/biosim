using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public class RuleLess : RuleEvaluation
	{
		public RuleLess(String p, object value) : base(p, value)
		{
		}

		protected override bool Compare(AbstractCell cell)
		{
			if (cell.Properties [Property].GetType().Equals(typeof(IntProperty))) {
				return (int)cell.Properties [Property].Value < (int)EqualityValue;
			}
			if (cell.Properties [Property].GetType().Equals(typeof(FloatProperty))) {
				return (float)cell.Properties [Property].Value < (float)EqualityValue;
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("{0} < {1}", Property, EqualityValue);
		}
	}
}

using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public class RuleGreater : AbstractRule
	{
		public RuleGreater(String p, object value) : base(p, value)
		{
		}

		public override bool Check(AbstractCell cell)
		{
			if (cell.Properties [property].GetType().Equals(typeof(IntProperty))) {
				int eq = (int)equalityValue;
				int v = (int)cell.Properties [property].Value;
				return v > eq;
				//return (int)equalityValue > (int)cell.Properties [property].Value;
			} else if (cell.Properties [property].GetType().Equals(typeof(FloatProperty))) {
				return (float)cell.Properties [property].Value > (float)equalityValue;
			} else {
				return false;
			}
		}
	}
}


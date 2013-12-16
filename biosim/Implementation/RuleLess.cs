using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public class RuleLess : AbstractRule
	{
		public RuleLess(String p, object value) : base(p, value)
		{
		}

		public override bool Check(AbstractCell cell)
		{
			if (cell.Properties [property].GetType().Equals(typeof(IntProperty))) {
				return (int)cell.Properties [property].Value < (int)equalityValue;
			} else if (cell.Properties [property].GetType().Equals(typeof(FloatProperty))) {
				return (float)cell.Properties [property].Value < (float)equalityValue;
			} else {
				return false;
			}
		}
	}
}

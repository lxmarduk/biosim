using System;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public class RuleNotEquals : RuleEvaluation
	{
		public RuleNotEquals(String prop, object value) : base(prop, value)
		{
		}

		protected override bool Compare(AbstractCell cell)
		{
			return !cell.Properties [Property].Equ(EqualityValue);
		}

		public override string ToString()
		{
			return string.Format("{0} ≠ {1}", Property, EqualityValue);
		}
	}
}


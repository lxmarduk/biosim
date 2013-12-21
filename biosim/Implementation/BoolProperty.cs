using Biosim.Abstraction;
using System;

namespace Biosim.Implementation
{
	[Serializable]
	public sealed class BoolProperty : AbstractProperty
	{
		public BoolProperty(string name)
            : this(name, null)
		{
		}

		public BoolProperty(string name, object value)
            : base(name, value)
		{
			if (value == null || !checkValue(Value)) {
				Debug.PrintWarning("Value doesn't \"bool\". Set to default.");
				Value = false;
			}
		}

		bool checkValue(object value)
		{
			if (value != null) {
				return value.GetType().Equals(typeof(bool));
			}
			return false;
		}

		public override void Increment()
		{
			// set to true
			Value = true;
		}

		public override void Increment(object amount)
		{
			// set to true
			Value = true;
		}

		public override void Decrement()
		{
			// set to false
			Value = false;
		}

		public override void Decrement(object amount)
		{
			// set to false
			Value = false;
		}

		public override void Set()
		{
			// set to false
			Value = true;
		}

		public override void Set(object value)
		{
			if (checkValue(value)) {
				Value = (bool)value;
			} else {
				Debug.PrintWarning("Value doesn't \"bool\". Not setted.");
			}
		}

		public override void Unset()
		{
			Value = false;
		}

		public override AbstractProperty Clone()
		{
			return new BoolProperty(this.Name, (bool)this.Value);
		}

		public override bool Equ(object obj)
		{
			if (!checkValue(obj)) {
				return false;
			}
			return (bool)obj == (bool)Value;
		}
	}
}

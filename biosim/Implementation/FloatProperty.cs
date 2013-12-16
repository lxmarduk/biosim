using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	public sealed class FloatProperty : AbstractProperty
	{
		public FloatProperty(string name)
            : this(name, null)
		{
		}

		public FloatProperty(string name, object value)
            : base(name, value)
		{
			if (value == null || !checkValue(value)) {
				Debug.PrintWarning("Value doesn't \"string\". Set to default.");
				Value = 0.0f;
			}
		}

		private bool checkValue(object value)
		{
			if (value != null) {
				return value.GetType().Equals(typeof(float));
			}
			return false;
		}

		public override void Increment()
		{
			if (checkValue(Value)) {
				Value = (float)Value + 1.0f;
			} else {
				Debug.PrintWarning("Value doesn't \"float\". Not incremented.");
			}
		}

		public override void Increment(object amount)
		{
			if (checkValue(Value)) {
				if (checkValue(amount)) {
					Value = (float)Value + (float)amount;
				} else {
					Debug.PrintWarning("Value of amount doesn't \"float\". Not incremented.");
				}
			} else {
				Debug.PrintWarning("Value doesn't \"float\". Not incremented.");
			}
		}

		public override void Decrement()
		{
			if (checkValue(Value)) {
				Value = (float)Value - 1.0f;
			} else {
				Debug.PrintWarning("Value doesn't \"float\". Not decremented.");
			}
		}

		public override void Decrement(object amount)
		{
			if (checkValue(Value)) {
				if (checkValue(amount)) {
					Value = (float)Value - (float)amount;
				} else {
					Debug.PrintWarning("Value of amount doesn't \"float\". Not decremented.");
				}
			} else {
				Debug.PrintWarning("Value doesn't \"float\". Not decremented.");
			}
		}

		public override void Set()
		{
			Value = 1.0f;
		}

		public override void Set(object value)
		{
			if (checkValue(value)) {
				Value = (float)value;
			} else {
				Debug.PrintWarning("Value doesn't \"float\". Not setted.");
			}
		}

		public override void Unset()
		{
			Value = 0.0f;
		}

		public override AbstractProperty Clone()
		{
			return new FloatProperty(this.Name, (float)this.Value);
		}

		public override bool Equ(object obj)
		{
			if (!checkValue(obj)) {
				return false;
			}
			return Math.Abs((float)Value - (float)obj) <= 0.0000001f;
		}
	}
}

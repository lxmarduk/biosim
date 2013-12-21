using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public sealed class StringProperty : AbstractProperty
	{
		public StringProperty(string name)
            : this(name, null)
		{
		}

		public StringProperty(string name, object value)
            : base(name, value)
		{
			if (value == null || !value.GetType().Equals(typeof(string))) {
				Debug.PrintWarning("Value doesn't \"string\". Set to default.");
				Value = String.Empty;
			}
		}

		public override void Increment()
		{
			// doesn't need for String;
			return;
		}

		public override void Increment(object amount)
		{
			// doesn't need for String;
			return;
		}

		public override void Decrement()
		{
			// doesn't need for String;
			return;
		}

		public override void Decrement(object amount)
		{
			// doesn't need for String;
			return;
		}

		public override void Set()
		{
			// doesn't need for String;
			return;
		}

		public override void Set(object value)
		{
			if (value.GetType().Equals(typeof(String))) {
				Value = value;
			} else {
				Debug.PrintWarning("Try to set non-string value to string object.");
				Value = value.ToString();
			}
		}

		public override void Unset()
		{
			// just remove string value;
			Value = String.Empty;
		}

		public override AbstractProperty Clone()
		{
			return new StringProperty(this.Name, (string)this.Value);
		}

		public override bool Equ(object obj)
		{
			if (!obj.GetType().Equals(typeof(String))) {
				return false;
			}
			return ((string)obj).Equals((string)Value);
		}
	}
}

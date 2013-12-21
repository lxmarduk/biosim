using System;

namespace Biosim.Abstraction
{
	[Serializable]
	public abstract class AbstractProperty
	{
		public String Name {
			get;
			set;
		}

		public Object Value {
			get;
			set;
		}

		protected AbstractProperty(String name) :
			this(name, null)
		{
		}

		protected AbstractProperty(String name, Object value)
		{
			Name = name;
			Value = value;
		}

		public abstract void Increment();

		public abstract void Increment(object amount);

		public abstract void Decrement();

		public abstract void Decrement(object amount);

		public abstract void Set();

		public abstract void Set(object value);

		public abstract void Unset();

		public abstract bool Equ(object obj);

		public virtual AbstractProperty Clone()
		{
			return null;
		}

		public override string ToString()
		{
			return string.Format("{0} = {1}", Name, Value);
		}
	}
}

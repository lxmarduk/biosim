using System;
using System.Collections.Generic;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	[Serializable]
	public sealed class PropertyCollection : ICollection<AbstractProperty>
	{
		List<AbstractProperty> items;

		public PropertyCollection()
		{
			items = new List<AbstractProperty>();
		}

		public void Add(AbstractProperty item)
		{
			if (HasProperty(item.Name)) {
				for (int i = 0; i < items.Count; ++i) {
					if (items [i].Name.Equals(item.Name)) {
						items [i].Value = item.Value;
						break;
					}
				}
			} else {
				items.Add(item);
			}
		}

		public void Clear()
		{
			items.Clear();
		}

		public bool Contains(AbstractProperty item)
		{
			return items.Contains(item);
		}

		public void CopyTo(AbstractProperty[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		public int Count {
			get {
				return items.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public bool Remove(AbstractProperty item)
		{
			return items.Remove(item);
		}

		public bool Remove(string name)
		{
			for (int i = 0; i < items.Count; ++i) {
				if (items [i].Name.Equals(name)) {
					return items.Remove(items [i]);
				}
			}
			return false;
		}

		public IEnumerator<AbstractProperty> GetEnumerator()
		{
			return new PropertyEnumerator(this);
		}

		public AbstractProperty this [int index] {
			get {
				return items [index];
			}
			set {
				items [index] = value.Clone();
			}
		}

		public AbstractProperty this [string key] {
			get {
				int len = items.Count;
				for (int i = 0; i < len; ++i) {
					if (items [i].Name.Equals(key)) {
						return items [i];
					}
				}
				return null;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new PropertyEnumerator(this);
		}

		public bool HasProperty(string name)
		{
			foreach (AbstractProperty prop in items) {
				if (prop.Name.Equals(name)) {
					return true;
				}
			}
			return false;
		}

		public int IndexOf(AbstractProperty abstractProperty)
		{
			return items.IndexOf(abstractProperty);
		}
	}
}

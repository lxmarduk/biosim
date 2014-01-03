using System;
using System.Collections.Generic;
using Biosim.Abstraction;
using System.Drawing;

namespace Biosim.Implementation
{
	public sealed class SeriesCollection : ICollection<double>
	{
		readonly List<double> items;

		public Color Color {
			get;
			set;
		}

		public String Name {
			get;
			set;
		}

		public double MaxValue {
			get;
			private set;
		}

		public SeriesCollection(String name)
		{
			items = new List<double>();
			Color = Utils.RandomColor();
			MaxValue = 0.0;
			Name = name;
		}

		public void Add(double item)
		{
			items.Add(item);
			if (item > MaxValue) {
				MaxValue = item;
			}
		}

		public void Clear()
		{
			items.Clear();
		}

		public bool Contains(double item)
		{
			return items.Contains(item);
		}

		public void CopyTo(double[] array, int arrayIndex)
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

		public IEnumerator<double> GetEnumerator()
		{
			return new SeriesEnumerator(this);
		}

		public double this [int index] {
			get {
				return items [index];
			}
			set {
				items [index] = value;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new SeriesEnumerator(this);
		}

		public bool Remove(double item)
		{
			int i = 0;
			bool anyRemoved = false;
			while (i < items.Count) {
				if (items [i].Equals(item)) {
					items.RemoveAt(i);
					anyRemoved = true;
					continue;
				}
				++i;
			}
			return anyRemoved;
		}

		public int IndexOf(double item)
		{
			return items.IndexOf(item);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biosim.Abstraction;

namespace Biosim.Implementation
{
	public sealed class SeriesEnumerator : IEnumerator<double>
	{
		private int current;
		private SeriesCollection collection;

		public SeriesEnumerator(SeriesCollection collection)
		{
			current = -1;
			this.collection = collection;
		}

		public double Current {
			get {
				return collection [current];
			}
		}

		object System.Collections.IEnumerator.Current {
			get {
				return collection [current];
			}
		}

		public void Dispose()
		{
			this.collection = null;
		}

		public bool MoveNext()
		{
			++current;
			if (current < collection.Count) {
				return true;
			} else {
				return false;
			}
		}

		public void Reset()
		{
			current = -1;
		}
	}
}

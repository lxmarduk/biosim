using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using biosim.Abstraction;

namespace biosim.Implementation {

    [Serializable]
    public class PropertyCollection : ICollection<AbstractProperty> {

        List<AbstractProperty> items;

        public PropertyCollection() {
            items = new List<AbstractProperty>();
        }

        public void Add(AbstractProperty item) {
            items.Add(item);
        }

        public void Clear() {
            items.Clear();
        }

        public bool Contains(AbstractProperty item) {
            return items.Contains(item);
        }

        public void CopyTo(AbstractProperty[] array, int arrayIndex) {
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

        public bool Remove(AbstractProperty item) {
            return items.Remove(item);
        }

        public IEnumerator<AbstractProperty> GetEnumerator() {
            return new PropertyEnumerator(this);
        }

        public AbstractProperty this[int index] {
            get {
                return items[index];
            }
        }
    }
}

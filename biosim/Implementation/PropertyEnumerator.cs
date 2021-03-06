﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biosim.Abstraction;

namespace Biosim.Implementation {
    public sealed class PropertyEnumerator : IEnumerator<AbstractProperty> {

        private int current;
        private PropertyCollection collection;

        public PropertyEnumerator(PropertyCollection collection) {
            current = -1;
            this.collection = collection;
        }

        public AbstractProperty Current {
            get {
                return collection[current];
            }
        }

        object System.Collections.IEnumerator.Current {
            get {
                return collection[current];
            }
        }

        public void Dispose() {
            this.collection = null;
        }

        public bool MoveNext() {
            ++current;
            if (current < collection.Count) {
                return true;
            } else {
                return false;
            }
        }

        public void Reset() {
            current = -1;
        }
    }
}

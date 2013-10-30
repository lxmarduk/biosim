using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace biosim.Abstraction {
    [Serializable]
    public abstract class AbstractProperty {

        public String Name {
            get;
            set;
        }

        private object value;
        public Object Value {
            get;
            set {
                this.value = value;
            }
        }

        [NonSerialized]
        public Type Type {
            get {
                return Value.GetType();
            }
        }

        public AbstractProperty(String name) :
            this(name, null) {
        }

        public AbstractProperty(String name, Object value) {
            this.Name = name;
            this.Value = value;
        }

        public abstract void Increment();
        public abstract void Increment(object amount);
        public abstract void Decrement();
        public abstract void Decrement(object amount);
        public abstract void Set();
        public abstract void Set(object value);
        public abstract void Unset();
    }
}

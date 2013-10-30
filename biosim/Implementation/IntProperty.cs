using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biosim.Abstraction;

namespace Biosim.Implementation {
    public sealed class IntProperty : AbstractProperty {
        public IntProperty(string name)
            : base(name) {
        }
        public IntProperty(string name, object value)
            : base(name, value) {
                if (!checkValue(Value)) {
                    Debug.PrintWarning("Value doesn't \"int\". Set to default.");
                    Value = 0;
                }
        }

        private bool checkValue(object value) {
            return value.GetType().Equals(typeof(int));
        }

        public override void Increment() {
            if (checkValue(Value)) {
                Value = (int)Value + 1;
            } else {
                Debug.PrintWarning("Value type of property doesn't \"int\". Not incremented.");
            }
        }

        public override void Increment(object amount) {
            if (checkValue(Value)) {
                if (checkValue(amount)) {
                    Value = (int)Value + (int)amount;
                } else {
                    Debug.PrintWarning("Value of increment amount doesn't \"int\". Not incremented.");
                }
            } else {
                Debug.PrintWarning("Value type of property doesn't \"int\". Not incremented.");
            }
        }

        public override void Decrement() {
            if (checkValue(Value)) {
                Value = (int)Value - 1;
            } else {
                Debug.PrintWarning("Value type of property doesn't \"int\". Not decremented.");
            }
        }

        public override void Decrement(object amount) {
            if (checkValue(Value)) {
                if (checkValue(amount)) {
                    Value = (int)Value - (int)amount;
                } else {
                    Debug.PrintWarning("Value of increment amount doesn't \"int\". Not decremented.");
                }
            } else {
                Debug.PrintWarning("Value type of property doesn't \"int\". Not decremented.");
            }
        }

        public override void Set() {
            // just set to 1
            Value = 1;
        }

        public override void Set(object value) {
            if (checkValue(value)) {
                Value = (int)value;
            } else {
                Debug.PrintWarning("Value to set doesn't \"int\". Not setted.");
            }
        }

        public override void Unset() {
            // just set to 0
            Value = 0;
        }
    }
}

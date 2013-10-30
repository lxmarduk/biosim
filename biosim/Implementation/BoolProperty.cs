using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biosim.Abstraction;

namespace Biosim.Implementation {
    public sealed class BoolProperty : AbstractProperty {
        public BoolProperty(string name)
            : base(name) {
        }

        public BoolProperty(string name, object value)
            : base(name, value) {
                if (!checkValue(Value)) {
                    Debug.PrintWarning("Value doesn't \"bool\". Set to default.");
                    Value = false;
                }
        }

        private bool checkValue(object value) {
            return value.GetType().Equals(typeof(bool));
        }

        public override void Increment() {
            // set to true
            Value = true;
        }

        public override void Increment(object amount) {
            // set to true
            Value = true;
        }

        public override void Decrement() {
            // set to false
            Value = false;
        }

        public override void Decrement(object amount) {
            // set to false
            Value = false;
        }

        public override void Set() {
            // set to false
            Value = true;
        }

        public override void Set(object value) {
            if (checkValue(value)) {
                Value = (bool)value;
            } else {
                Debug.PrintWarning("Value doesn't \"bool\". Not setted.");
            }
        }

        public override void Unset() {
            Value = false;
        }
    }
}

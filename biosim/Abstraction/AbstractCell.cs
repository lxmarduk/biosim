﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biosim.Implementation;

namespace Biosim.Abstraction {

    [Serializable]
    public abstract class AbstractCell {

        PropertyCollection props;
        public PropertyCollection Properties {
            get {
                return props;
            }
        }

        protected AbstractCell(String name) {
            props = new PropertyCollection();
            props.Add(new StringProperty("Name", name));
        }

        public bool HasProperty(string name) {
            foreach (AbstractProperty p in props) {
                if (p.Name.Equals(name)) {
                    return true;
                }
            }
            return false;
        }
    }
}

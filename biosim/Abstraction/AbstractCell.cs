using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using biosim.Implementation;

namespace biosim.Abstraction {

    [Serializable]
    public abstract class AbstractCell {

        public PropertyCollection Properties {
            get;
            set;
        }

        public AbstractCell(String name) {
            Properties = new PropertyCollection();
            Properties.Add(new StringProperty("Name", name));
        }
    }
}

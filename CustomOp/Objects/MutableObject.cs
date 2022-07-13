using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomOp.Objects
{
    internal class MutableObject
    {
        Type t;
        Object o;

        public MutableObject(Object o)
        {
            t = o.GetType();
            this.o = o;
        }

        public Type getType()
        {
            return t;
        }

        public Object getData()
        {
            return o;
        }
    }
}

using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class SetVarOperation : Operation
    {
        public SetVarOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            data.put(data.getString("OutputID"), Operation.parseVars(data.getString("ObjectID"), data, null));
        }

    }
}

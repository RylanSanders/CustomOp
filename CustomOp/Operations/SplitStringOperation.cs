using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class SplitStringOperation : Operation
    {
        public SplitStringOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            data.put("SplitStringList", new List<string>(data.getString("StringToSplit").Split(data.getString("SplitItem"))));
        }
    }
}

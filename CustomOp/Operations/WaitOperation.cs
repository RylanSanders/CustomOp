using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class WaitOperation : Operation
    {
        public WaitOperation(XElement config) : base(config)
        {

        }

        public override void execute(OpData data)
        {
            base.execute(data);

            Thread.Sleep(data.getInt("WaitTime"));
        }
    }
}

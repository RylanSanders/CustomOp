using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Process = System.Diagnostics.Process;

namespace CustomOp.Operations
{
    internal class StartProcessOperation : Operation
    {
        public StartProcessOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            Process.Start(data.getString("ToStartProcess"));
        }
    }
}

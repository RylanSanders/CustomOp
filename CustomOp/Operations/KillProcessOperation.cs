using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;
using Process = System.Diagnostics.Process;

namespace CustomOp.Operations
{
    internal class KillProcessOperation : Operation
    {
        public KillProcessOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            Process[] processes = Process.GetProcessesByName(data.getString("ToKillProcess"));
            foreach(Process p in processes)
            {
                p.Kill();
            }
        }
    }
}

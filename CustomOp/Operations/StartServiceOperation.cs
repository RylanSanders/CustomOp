using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Process = System.Diagnostics.Process;

namespace CustomOp.Operations
{
    internal class StartServiceOperation : Operation
    {
        public StartServiceOperation(XElement config) : base(config)
        {
            
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            ServiceController service = new ServiceController(data.getString("ToStartService"));
            service.Start();
        }
    }
}

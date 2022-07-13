using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class StopServiceOperation : Operation
    {
        public StopServiceOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            ServiceController service = new ServiceController(data.getString("ToStopService"));
            service.Stop();
        }
    }
}

using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class LogOutputOperation : Operation
    {
        public LogOutputOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            Logger.log.Info(data.getString("LogOutputText"));
        }
    }
}

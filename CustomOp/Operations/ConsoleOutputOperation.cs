using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class ConsoleOutputOperation : Operation
    {
        public ConsoleOutputOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            File.AppendAllText("FakeLog.txt",data.getString("ConsoleOutputText"));
        }
    }
}

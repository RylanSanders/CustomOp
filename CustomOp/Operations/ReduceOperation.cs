using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class ReduceOperation : Operation
    {
        string accumulator = "";
        string start_value = "";
        public ReduceOperation(XElement config) : base(config)
        {
            XElement e = config.Element("Accumulator");
            if (e == null || e.Value==null)
            {
                throw new Exception("ReduceOperation is Missing Accumulator Config!");
            }
            accumulator = e.Value.ToString();
        }

        public override void execute(OpData data)
        {
            base.execute(data);
            List<string> valueList = data.getStringList("ToReduceList");
            data.put("ReducedListString", Accumulators.strAccumulate(valueList, accumulator, start_value));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;
namespace CustomOp.Operations
{
    internal class GenerateIntListOperation : Operation
    {
        public GenerateIntListOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            List<int> toPut = new List<int>();
            int x = data.getInt("StartInt");
            int finish = data.getInt("EndInt");
            int step = data.getInt("Step");
            while (x < finish)
            {
                toPut.Add(x);
                x += step;
            }

            data.put("IntListOutput", toPut);
        }
    }
}

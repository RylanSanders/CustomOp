using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class GetMapKeyOperation : Operation
    {
        public GetMapKeyOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            int index = 0;
            if (data.contains("MapKeyIndex"))
            {
                index = data.getInt("MapKeyIndex");
            }

            Dictionary<string, string> map = data.getMap("KeyMap");
            data.put("KeyFromMap",map.Keys.ToList()[index]);
        }
    }
}

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

            //TODO should have a parent object - Hashable for lists and maps and stuff
            Object o = data.getObject("KeyMap");
            if (o.GetType() == typeof(Dictionary<string, string>))
            {
                data.put("KeyFromMap", ((Dictionary<string, string>)o).Keys.ToList()[index]);
            }
            else if(o.GetType() == typeof(JSONObject))
            {
                data.put("KeyFromMap", ((JSONObject)o).map.Keys.ToList()[index]);
            }
            else
            {
                throw new Exception("Error in GetMapKey Operation could not find the KeyMap variable");
            }
        }
    }
}

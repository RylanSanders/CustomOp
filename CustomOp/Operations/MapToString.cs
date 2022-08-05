using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;
namespace CustomOp.Operations
{
    internal class MapToStringOperation : Operation
    {
        public MapToStringOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            Dictionary<string, string> map = data.getMap("MapToString");
            StringBuilder sb = new StringBuilder();
            foreach(string key in map.Keys)
            {
                sb.Append($"{key}: {map[key]}\n");
            }
            data.put("MappedString",sb.ToString());
        }
    }
}

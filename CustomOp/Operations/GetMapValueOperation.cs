using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class GetMapValueOperation : Operation
    {
        public GetMapValueOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string key = data.getString("MapKey");
            Dictionary<string, string> map = data.getMap("MapToRead");

            data.put("MapKeyValue", map[key]);
        }
    }
}

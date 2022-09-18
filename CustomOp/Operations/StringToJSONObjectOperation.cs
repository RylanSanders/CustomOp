using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class StringToJSONObjectOperation : Operation
    {
        public StringToJSONObjectOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string json = data.getString("JSonString");
            JSONObject obj = new JSONObject(json);

            data.put("ParsedJSON", obj);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class JSONToMapOperation : Operation
    {
        public JSONToMapOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string json = data.getString("JSonString");
            JSONObject map = new JSONObject(json);
            Dictionary<string, string> jsonMap = new Dictionary<string, string>();
            foreach(string key in map.map.Keys)
            {
                jsonMap.Add(key, map.map[key].ToString());
            }
            
            data.put("DeserializedJSonMap", jsonMap);
        }
           


    }
}

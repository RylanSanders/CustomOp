using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;
namespace CustomOp.Operations
{
    internal class GenerateMapOperation : Operation
    {
        Dictionary<string, string> map;
        public GenerateMapOperation(XElement config) : base(config)
        {
            map = new Dictionary<string, string>();
            var inputs = from input in config.Descendants("Data")
                        select new
                        {
                            Name = input.Attribute("name").Value.ToString(),
                            Value = input.Attribute("value").Value.ToString(),
                        };

            foreach (var input in inputs)
            {
                map.Add(input.Name, input.Value);
            }
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            data.put("GeneratedMap", map);
        }
    }
}

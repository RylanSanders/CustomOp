using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class SetVarOperation : Operation
    {
        private OpData toSetData;
        public SetVarOperation(XElement config) : base(config)
        {
            toSetData = new OpData();
            toSetData = parseInput(config, toSetData);
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            data.merge(toSetData);
        }

        public OpData parseInput(XElement element, OpData data)
        {
            var inputs = from input in element.Descendants("Data")
                         select new
                         {
                             Name = input.Attribute("name").Value.ToString(),
                             Value = input.Attribute("value").Value.ToString(),
                             Type = input.Attribute("type").Value.ToString()
                         };

            foreach (var input in inputs)
            {
                if (input.Type.Equals("string"))
                {
                    data.put(input.Name, input.Value);
                }
                else if (input.Type.Equals("int"))
                {
                    data.put(input.Name, Int32.Parse(input.Value));
                }
            }
            return data;
        }
    }
}

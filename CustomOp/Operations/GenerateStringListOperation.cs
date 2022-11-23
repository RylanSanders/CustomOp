using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class GenerateStringListOperation : Operation
    {
        List<string> toStoreList;
        public GenerateStringListOperation(XElement config) : base(config)
        {
            IEnumerable<XElement> valElements = config.Element("Values").Elements("Val");
            toStoreList = valElements.Select((x)=>x.Value).ToList();
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            data.put("GeneratedStringList", toStoreList);
        }
    }
}

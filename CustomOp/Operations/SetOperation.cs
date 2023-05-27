using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
// Expand upon this class to make it similar to the ModifyString Operation to make it super easy to modify lists
//TODO remove this - already did this in Join Lists
    internal class SetOperation : Operation
    {
        List<string> SubOperations = new List<string>();
        public SetOperation(XElement config) : base(config)
        {
            foreach(XElement element in config.Element("SetOperations").Elements("SubOp"))
            {
                SubOperations.Add( element.Attribute("list2").Value.ToString());
            }
        }

        public override void execute(OpData data)
        {
            base.execute(data);
            List<string> inputList = data.getStringList("InputSetList");
            processSubOps(data, inputList);
            data.put("SetList", inputList);
        }

        private void processSubOps(OpData data, List<string> inputList)
        {
            foreach(string listName in SubOperations)
            {
                List<string> subList = data.getStringList(listName);
                inputList = inputList.FindAll((x) => !subList.Contains(x));
            }
        }
    }
}

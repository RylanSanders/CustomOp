using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class JoinListOperation : Operation
    {
        string joinType;
        string leftList;
        string rightList;
        public JoinListOperation(XElement config) : base(config)
        {
            XElement joinElement = config.Element("Join");
            joinType = joinElement.Attribute("type").Value;
            leftList = joinElement.Attribute("leftList").Value;
            rightList = joinElement.Attribute("rightList").Value;
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            List<string> left = data.getStringList(leftList);
            List<string> right = data.getStringList(rightList);
            List<string> joinedList = new List<string>();
            if (joinType == "Subtract")
            {
                joinedList = left.AsEnumerable().Where(x => !right.Contains(x)).ToList();
            }

            data.put("JoinedList", joinedList);
        }
    }
}

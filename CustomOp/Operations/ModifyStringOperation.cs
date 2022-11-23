using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class ModifyStringOperation : Operation
    {
        //TODO decide on a variable script - Should I just put $$ around all variable and implement it across all operations
        List<XElement> appenders;
        public ModifyStringOperation(XElement config) : base(config)
        {
            appenders = new List<XElement>();
            if (config.Element("StringOps")==null)
            {
                throw new Exception("Error in ModifyString Config: Requires StringOps element and atleast one StringOp");
            }
            foreach (XElement e in config.Element("StringOps").Elements("StringOp"))
            {
                appenders.Add(e);
            }
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string start ="";
            foreach(XElement op in appenders)
            {
                if (op.Attribute("type").Value == "append")
                {
                    start = append(op, start, data);
                }else if(op.Attribute("type").Value == "remove")
                {
                    start = remove(op, start, data);
                }
            }

            data.put("ModifiedString", start);
        }

        private string append(XElement appendOp, string current, OpData data)
        {
            string str1 = varString(appendOp.Attribute("string1").Value, current, data);
            string str2 = varString(appendOp.Attribute("string2").Value, current, data);
            return str1 + str2;
        }

        private string remove(XElement appendOp, string current, OpData data)
        {
            string main = varString(appendOp.Attribute("base").Value, current, data);
            string remove = varString(appendOp.Attribute("removeVal").Value, current, data);
            return main.Replace(remove, "");
        }

        private string varString(string st, string current, OpData data)
        {
            if (st == "$current$")
            {
                return current;
            }
            else if(data.containsType(st, typeof(String)))
            {
                return data.getString(st);
            }
            else if (st.Contains("["))
            {
                return (string)Operation.parseVars(st, data, null);
            }
            return st;
        }
    }
}

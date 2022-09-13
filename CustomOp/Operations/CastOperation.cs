using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class CastOperation : Operation
    {
        string castType;
        //TODO make this autodetect types and do some default casts, will still need the config to specify if there are more than 1 way to cast
        //TODO maybe make this config have an input type and an output type
        public CastOperation(XElement config) : base(config)
        {
            castType = config.Element("CastType").Value;
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            if (castType == "IntToString")
            {
                data.put("CastedString", data.getInt(data.getString("IntID")).ToString());
            }
            if (castType == "IntListToStringList")
            {
                List<int> inputList = data.getIntList(data.getString("IntListID"));
                data.put("CastedStringList",inputList.Select(x => x.ToString()).ToList());
            }
            if (castType == "StringListToIntList")
            {
                List<string> inputList = data.getStringList(data.getString("StringListID"));
                data.put("CastedIntList", inputList.Select(x => int.Parse(x)).ToList());

            }
            if (castType == "StringToStringList")
            {
                string str = data.getString(data.getString("StringID"));
                str = str.Trim();
                str = str.Substring(1, str.Length - 3);
                data.put("CastedStringList",str.Split(",").ToList());
            }
        }

        
    }
}

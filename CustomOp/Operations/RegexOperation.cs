using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class RegexOperation : Operation
    {
        public RegexOperation(XElement config) : base(config)
        {

        }

        public override void execute(OpData data)
        {
            base.execute(data);

            data.put("RegexOutput", Regex.Replace(data.getString("RegexInput"), data.getString("RegexMatch"), data.getString("RegexReplace")));
        }
    }
}

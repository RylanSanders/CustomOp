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
    internal class StrParseListOperation : Operation
    {
        public StrParseListOperation(XElement config) : base(config)
        {

        }

        public override void execute(OpData data)
        {
            base.execute(data);
            string startTag = "";
            string endTag = "";
            data.put("ParseStringList", parseString(data.getString("InputString"), data.getString("StartTag"), data.getString("EndTag")));
        }

        private string parseString(string input, string startTag, string endTag)
        {
            List<String> strings = new List<String>();
            string tempStr = input;
            while (tempStr.Contains(startTag) && tempStr.Contains(endTag))
            {
                    int startIndex = tempStr.IndexOf(startTag);
                    tempStr = tempStr.Remove(0, startIndex);
                    //Note that the tempStr changes length after the above line so don't use startIndex
                    int firstPos = startTag.Length;
                    int endIndex = tempStr.IndexOf(endTag);
                    if (endIndex - firstPos < 0) break;
                    strings.Add(tempStr.Substring(firstPos, endIndex - firstPos));
                    tempStr = tempStr.Substring(endIndex + endTag.Length);
            }
            StringBuilder stringBuilder = new StringBuilder();
            strings.ForEach((s) => stringBuilder.Append(s + "\n"));
            return stringBuilder.ToString();
        }
    }
}
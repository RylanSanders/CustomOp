using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class StringOperation : Operation
    {
        bool doTrim = false;
        bool doRemoveQuotes = false;
        bool doFormatDateTime = false;
        public StringOperation(XElement config) : base(config)
        {
            XElement trim = config.Element("DoTrim");
            XElement removeQuotes = config.Element("RemoveQuotes");
            XElement formatDateTime = config.Element("FormatDateTime");
            if (trim!=null && (trim.Value=="True" || trim.Value == "true"))
            {
                doTrim = true;
            }
            if (removeQuotes != null && (removeQuotes.Value == "True" || removeQuotes.Value == "true"))
            {
                doRemoveQuotes = true;
            }
            if (formatDateTime != null && (formatDateTime.Value == "True" || formatDateTime.Value == "true"))
            {
                doFormatDateTime = true;
            }
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string inputString = data.getObject("InputString").ToString();
            if (doRemoveQuotes)
            {
                inputString = inputString.Replace("\"", "");
            }
            if (doTrim)
            {
                inputString = inputString.Trim();
            }
            if (doFormatDateTime)
            {
                inputString = DateTime.Parse(inputString).ToString();
            }
            data.put("OutputString", inputString);

        }
    }
}

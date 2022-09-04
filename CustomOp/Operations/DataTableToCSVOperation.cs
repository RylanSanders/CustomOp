using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class DataTableToCSVOperation : Operation
    {
        public DataTableToCSVOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string csvfilePath = data.getString("CSVFilePath");
            DataTable dt = data.getDataTable("DataTableToCSV");
            File.WriteAllText(csvfilePath, DTToCSVString(dt));
        }

        public string DTToCSVString(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in dt.colValues.Keys)
            {
                sb.Append($"{handleQuotes(key)},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine();
            for (int i = 0; i < dt.getMaxNumRows(); i++)
            {
                foreach (string key in dt.colValues.Keys)
                {
                    sb.Append($"{handleQuotes(dt.colValues[key][i])},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private string handleQuotes(string str)
        {
            if(str=="" || str == "\"\"")
            {
                return "\"\"";
            }
            string toRet = str;
            toRet = toRet.Replace("\n", "");
            toRet = toRet.Replace("\r", "");
            toRet = toRet[0] + toRet.Substring(1, toRet.Length - 2).Replace("\"", "") + toRet[toRet.Length - 1];

            if (str.Trim()[0] != '\"')
            {
                toRet = "\"" + toRet;
            }
            if (str.Trim()[str.Trim().Length - 1] != '\"')
            {
                toRet = toRet + "\"";
            }
            return toRet;
        }
    }
}

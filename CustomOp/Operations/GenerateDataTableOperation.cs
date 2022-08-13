using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class GenerateDataTableOperation : Operation
    {
        DataTable table;
        public GenerateDataTableOperation(XElement config) : base(config)
        {
            table = new DataTable();
            var inputs = from input in config.Element("Table").Descendants()
                         select new
                         {
                             values = input.Attributes().ToList()
                         };

            foreach (var input in inputs)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach(XAttribute att in input.values)
                {
                    dict.Add(att.Name.ToString(), att.Value);
                }
                table.addRow(dict);
            }
        }

        public override void execute(OpData data)
        {
            base.execute(data);
            data.put("GeneratedDataTable", table);

        }
    }
}

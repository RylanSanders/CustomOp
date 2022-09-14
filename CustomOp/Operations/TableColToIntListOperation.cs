using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class TableColToIntListOperation : Operation
    {
        public TableColToIntListOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);


            string colName = data.getString("ColumnName");
            DataTable dt = data.getDataTable("InputTable");

            List<string> col = dt.getColumn(colName);
            List<int> list = col.Select(x => getInt(x)).ToList();

            data.put("ColIntList", list);
        }

        private int getInt(string s)
        {
            try
            {
                return int.Parse(s);
            }
            catch 
            {

            }
           return 0;
        }
    }
}

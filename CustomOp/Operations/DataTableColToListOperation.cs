using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class DataTableColToListOperation : Operation
    {
        public DataTableColToListOperation(XElement config) : base(config)
        {
           
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string colName = data.getString("ColName");
            DataTable dt = data.getDataTable("DataTableToList");

            data.put("DataTableColList", dt.getColumn(colName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class StringToStringMappingOperation : Operation
    {
        public StringToStringMappingOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            DataTable mapping = data.getDataTable("StringMappingTable");
            string inputCol = data.getString("InputCol");
            string outputCol = data.getString("OutputCol");
            string inputValue = data.getString("InputString");

            data.put("MappedOutputString", mapping.getColumn(outputCol)[mapping.searchRowForValue(inputCol, inputValue)]);
        }
    }
}

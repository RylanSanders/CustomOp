using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class FileAppendOperation : Operation
    {
        public FileAppendOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            File.AppendAllText(data.getString("AppendFilePath"), data.getString("ToWriteText"));
        }
    }
}

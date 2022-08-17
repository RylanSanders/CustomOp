using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class WriteFileOperation : Operation
    {
        public WriteFileOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string path = data.getString("WriteFilePath");
            string txt = data.getString("TextToWrite");
            File.WriteAllText(path, txt);
        }
    }
}

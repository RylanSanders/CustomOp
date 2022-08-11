using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;
namespace CustomOp.Operations
{
    internal class ReadFileOperation : Operation
    {
        public ReadFileOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string input = File.ReadAllText(data.getString("FilePath"));
            data.put("ReadText", input);
        }
    }
}

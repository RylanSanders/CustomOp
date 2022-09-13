using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class GetFilesInDirOperation : Operation
    {
        public GetFilesInDirOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string[] files = Directory.GetFiles(data.getString("DirectoryPath"));
            data.put("DirectoryList",files.ToList());
        }
    }
}

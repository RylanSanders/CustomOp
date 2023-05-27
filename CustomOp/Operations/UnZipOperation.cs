using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO.Compression;

namespace CustomOp.Operations
{
    internal class UnZipOperation : Operation
    {
        public UnZipOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            ZipFile.ExtractToDirectory(data.getString("ZipFolderPath"), data.getString("OutputDirPath"));
        }
    }
}

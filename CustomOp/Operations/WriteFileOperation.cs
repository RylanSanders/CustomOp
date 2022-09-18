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
            //File.WriteAllText(path, txt);

            using (FileStream fs = File.Create(path))
            {
                // writing data in string
                string dataasstring = txt;
                byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
                fs.Write(info, 0, info.Length);

                // writing data in bytes already
                byte[] bitData = new byte[] { 0x0 };
                fs.Write(bitData, 0, bitData.Length);
            }
        }
    }
}

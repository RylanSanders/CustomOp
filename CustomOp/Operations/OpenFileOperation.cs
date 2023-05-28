using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class OpenFileOperation : Operation
    {
        public OpenFileOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);
            if (data.contains("OpenArgs"))
            {
                System.Diagnostics.Process.Start(data.getString("FileToOpen"), data.getString("OpenArgs"));
            }
            else
            {
                OpenWithDefaultProgram(data.getString("FileToOpen"));
            }
            
        }

        private void OpenWithDefaultProgram(string path)
        {
            using System.Diagnostics.Process fileopener = new System.Diagnostics.Process();

            fileopener.StartInfo.FileName = "explorer";
            fileopener.StartInfo.Arguments = "\"" + path + "\"";
            fileopener.Start();
        }
    }
}

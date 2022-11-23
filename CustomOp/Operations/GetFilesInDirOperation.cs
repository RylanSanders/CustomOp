using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
            bool isIntList = new List<string>(files).All(f => {
                if(!f.Contains(".")) return false;
                string[] arr = f.Split(".");
                int lastSlash = arr[0].LastIndexOf("\\");
                Int32 s = 0;
                if (int.TryParse(arr[0].Substring(lastSlash+1), out s)) return true;
                return false;
                });
            if (isIntList)
            {
                files = new List<string>(files).OrderBy((f) =>
                {
                    string[] arr = f.Split(".");
                    int lastSlash = arr[0].LastIndexOf("\\");
                    return int.Parse(arr[0].Substring(lastSlash+1));
                }).ToArray();
            }
            data.put("DirectoryList",files.ToList());
        }
    }
}

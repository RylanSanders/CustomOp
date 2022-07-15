using CustomOp.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Objects
{
    internal static class XMLParser
    {

        public static Operation parseOpXML(XElement element)
        {
            switch (element.Attribute("type").Value)
            {
                case "FileMove": return new FileMoveOperation(element);break;
                case "KillProcess": return new KillProcessOperation(element);break;
                case "Wait": return new WaitOperation(element);break;
                case "StartProcess": return new StartProcessOperation(element); break;
                case "StartService": return new StartServiceOperation(element); break;
                case "StopService": return new StopServiceOperation(element); break;
                case "LogOutput": return new LogOutputOperation(element); break;
                case "SetVar": return new SetVarOperation(element); break;
                default: throw new Exception($"Invalid Operation Type ({element.Attribute("type").Value})");
            }
        }

        public static OpData parseInput(XElement element, OpData data)
        {
            var inputs = from input in element.Descendants("Data")
                         select new
                         {
                             Name = input.Attribute("name").Value.ToString(),
                             Value = input.Attribute("value").Value.ToString(),
                             Type = input.Attribute("type").Value.ToString()
                         };

            foreach(var input in inputs)
            {
                if (input.Type.Equals("string"))
                {
                    data.put(input.Name, input.Value);
                }
                else if(input.Type.Equals("int")){
                    data.put(input.Name, Int32.Parse(input.Value));
                }
            }
            return data;
        }
    }
}

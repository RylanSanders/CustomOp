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
                case "RegexReplace": return new RegexOperation(element); break;
                case "MessageBox": return new MessageBoxOperation(element); break;
                case "GenerateIntList": return new GenerateIntListOperation(element); break;
                case "ProcessList": return new ProcessListOperation(element); break;
                case "HttpRequest": return new HttpOperation(element); break;
                case "StoreMapToDB": return new StoreMapToDBOperation(element); break;
                case "GenerateMap": return new GenerateMapOperation(element); break;
                case "MapToString": return new MapToStringOperation(element); break;
                case "JSONToMap": return new JSONToMapOperation(element); break;
                case "ReadFile": return new ReadFileOperation(element); break;
                case "GenerateTable": return new GenerateDataTableOperation(element);break;
                case "TableToString": return new TableToStringOperation(element);break;
                case "StoreTableToDB": return new StoreTableToDBOperation(element);break;
                case "SQLSelect": return new SQLSelectOperation(element);break;
                case "TableColToIntList": return new TableColToIntListOperation(element);break;
                case "WriteFile": return new WriteFileOperation(element);break;
                case "GetMapValue": return new GetMapValueOperation(element);break;
                case "JSONListToTable": return new JSONListToTableOperation(element);break;
                case "DataTableColToList": return new DataTableColToListOperation(element);break;
                case "Reduce": return new ReduceOperation(element);break;
                case "DataTableToCSV": return new DataTableToCSVOperation(element);break;
                case "DataTableModify": return new DataTableModifyOperation(element);break;
                case "ModifyDTCol": return new ModifyDTColOperation(element);break;
                default: throw new Exception($"Invalid Operation Type ({element.Attribute("type").Value})");
            }
        }

        public static OpData parseDataTags(XElement element, OpData data)
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

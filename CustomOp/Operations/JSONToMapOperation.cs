using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class JSONToMapOperation : Operation
    {
        public JSONToMapOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            string json = data.getString("JSonString");
            JSONObject map = new JSONObject(json);
            Dictionary<string, string> jsonMap = new Dictionary<string, string>();
            foreach(string key in map.map.Keys)
            {
                jsonMap.Add(key, map.map[key].ToString());
            }
            
            data.put("DeserializedJSonMap", jsonMap);
        }


        class JSONObject
        {
            enum objTypes
            {
                String, List, Map
            }
            public string str;
            public List<JSONObject> list;
            public Dictionary<string, JSONObject> map;
            objTypes type;

            public JSONObject(string json)
            {
                string processedJSON = json.Trim();
                if (processedJSON.Length == 0)
                {
                    type = objTypes.String;
                    str = "";
                }
                else if (processedJSON[0] == '{')
                {
                    type = objTypes.Map;
                    map = parseMap(processedJSON);
                }
                else if (processedJSON[0] == '\"')
                {
                    type = objTypes.String;
                    str = processedJSON.Replace("\"", "");
                }
                else if (processedJSON[0] == '[')
                {
                    type = objTypes.List;
                    list = parseList(processedJSON);
                }
                else
                {
                    type = objTypes.String;
                    str = "";
                }
            }

            private Dictionary<string, JSONObject> parseMap(string jsonMap)
            {
                List<string> mapEntries = new List<string>();
                List<int> mapSplits = getMapSplits(jsonMap);
                mapEntries.Add(jsonMap.Substring(0, mapSplits[0]));
                for (int i = 0; i < mapSplits.Count; i++)
                {
                    //Skip the last one
                    if (i + 1 != mapSplits.Count)
                    {
                        mapEntries.Add(jsonMap.Substring(mapSplits[i] + 1, mapSplits[i + 1] - mapSplits[i] - 1));
                    }
                }
                mapEntries.Add(jsonMap.Substring(mapSplits[mapSplits.Count - 1], jsonMap.Length - mapSplits[mapSplits.Count - 1] - 2));

                Dictionary<string, JSONObject> toRet = new Dictionary<string, JSONObject>();
                foreach (string part in mapEntries)
                {
                    string[] parts = parseMapEntry(part);
                    toRet.Add(parts[0], new JSONObject(parts[1]));
                }
                return toRet;
            }

            private string[] parseMapEntry(string entry)
            {
                int firstQuote = entry.IndexOf('"') + 1;
                int secondQuoteLength = entry.Substring(firstQuote + 1).IndexOf('"') + 1;
                string key = entry.Substring(firstQuote, secondQuoteLength);
                string value = entry.Substring(entry.IndexOf(":") + 1);
                return new string[] { key, value };
            }

            class CharPosition
            {
                public char c;
                public int pos;

                public CharPosition(char c, int pos)
                {
                    this.c = c;
                    this.pos = pos;
                }
            }

            private List<int> getMapSplits(string json)
            {
                int pos = 0;
                Stack<CharPosition> containers = new Stack<CharPosition>();
                List<int> lines = new List<int>();
                bool doneParsing = false;
                while (!doneParsing)
                {
                    char c = json[pos];
                    if (c == '{' || c == '[')
                    {
                        containers.Push(new CharPosition(c, pos));
                    }
                    if (c == '}' || c == ']')
                    {
                        containers.Pop();
                    }
                    if (c == '\"')
                    {
                        if (containers.Peek().c == '\"')
                        {
                            CharPosition startPos = containers.Pop();
                            //lines.Add(json.Substring(startPos.pos, pos-startPos.pos+1));
                        }
                        else
                        {
                            containers.Push(new CharPosition(c, pos));
                        }
                    }
                    if (c == ',' && containers.Count == 1)
                    {
                        lines.Add(pos);
                    }
                    pos++;
                    if (pos >= json.Length)
                    {
                        doneParsing = true;
                    }

                }

                return lines;
            }

            private List<JSONObject> parseList(string jsonList)
            {
                List<JSONObject> list = new List<JSONObject>();
                List<int> listSplits = getMapSplits(jsonList);
                list.Add(new JSONObject(jsonList.Substring(1, listSplits[0] - 1)));
                for (int i = 0; i < listSplits.Count; i++)
                {
                    //Skip the last one
                    if (i + 1 != listSplits.Count)
                    {
                        list.Add(new JSONObject(jsonList.Substring(listSplits[i] + 1, listSplits[i + 1] - listSplits[i] - 1)));
                    }
                }
                list.Add(new JSONObject(jsonList.Substring(listSplits[listSplits.Count - 1] + 1, jsonList.Length - listSplits[listSplits.Count - 1] - 2)));
                return list;
            }

            public override string ToString()
            {
                if (type == objTypes.String)
                {
                    return $"\"{str}\"";
                }
                else if (type == objTypes.List)
                {
                    string toRet = "[";
                    foreach (JSONObject o in list)
                    {
                        toRet = toRet + o.ToString() + ",";
                    }
                    toRet = toRet.Remove(toRet.Length - 1);
                    toRet += "]";
                    return toRet;
                }
                else if (type == objTypes.Map)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{");
                    foreach (string key in map.Keys)
                    {
                        sb.AppendLine($"\"{key}\"" + ": " + map[key].ToString() + ",");
                    }
                    sb.Remove(sb.Length - 2, 1);
                    string toRet = sb.ToString();
                    toRet = toRet.Substring(0, toRet.Length - 2) + "}";

                    return toRet;
                }
                return "";
            }


        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomOp.Objects
{
    internal class JSONObject
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
            try
            {


                string processedJSON = json.Trim().Replace("\\\"", "");
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
                    str = json;
                }
            } catch(Exception e)
            {
                if(json.Length > 2000)
                {
                    throw new Exception($"Error Processing JSon: {json.Substring(0,2000)} \n- See the following error:{e.Message} \n {e.StackTrace}");
                }
                else
                {
                    throw new Exception($"Error Processing JSon: {json} \n- See the following error:{e.Message} \n {e.StackTrace}");
                }
                
            }
        }

        public String getString()
        {
            if (type == objTypes.String)
            {
                return str;
            }
            throw new Exception("Error in GetString in JSONObject - The Object type is not str: " + type);
        }

        public List<JSONObject> getList()
        {
            if (type == objTypes.List)
            {
                return list;
            }
            throw new Exception("Error in GetString in JSONObject - The Object type is not List: " + type);
        }

        public Dictionary<String,JSONObject> getMap()
        {
            if (type == objTypes.Map)
            {
                return map;
            }
            throw new Exception("Error in GetString in JSONObject - The Object type is not Map: " + type);
        }

        public JSONObject getMapValue(string id)
        {
            if (type != objTypes.Map || !map.ContainsKey(id))
            {
                throw new Exception("Error in getMapValue in JSONObject - Not map or map does not contain " + id);
            }
            return map[id];
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
            //only has 1 value
            bool has1Val = mapSplits.Count == 1 && mapSplits[0] == jsonMap.Length;
            if (jsonMap.Length - mapSplits[mapSplits.Count - 1] - 2 < 0 && !has1Val)
            {
                mapEntries.Add(jsonMap.Substring(mapSplits[mapSplits.Count - 1], jsonMap.Length - mapSplits[mapSplits.Count - 1]));
            }
            else if(!has1Val)
            {
                mapEntries.Add(jsonMap.Substring(mapSplits[mapSplits.Count - 1], jsonMap.Length - mapSplits[mapSplits.Count - 1] - 2));
            }
            

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
            try
            {
                int firstQuote = entry.IndexOf('"') + 1;
                int secondQuoteLength = entry.Substring(firstQuote + 1).IndexOf('"') + 1;
                string key = entry.Substring(firstQuote, secondQuoteLength);
                string value = entry.Substring(entry.IndexOf(":") + 1);
                //If the entry still is contained in brackets, remove them from the values
                if (entry.StartsWith("{"))
                {
                    int lastBracket = value.LastIndexOf("}");
                    if(lastBracket!=-1)
                        value = value.Remove(lastBracket, 1);
                }
                return new string[] { key, value };
            }
            catch
            {
                Console.WriteLine("A");
            }
            return null;
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
            string trimmedJSon = json.Trim();
            if (!trimmedJSon.Contains(","))
            {
                return new List<int>() { trimmedJSon.Length };
            }
            while (!doneParsing)
            {
                try
                {
                    char c = trimmedJSon[pos];
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
                        if (containers.Count != 0 && containers.Peek().c == '\"')
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
                    if (pos >= trimmedJSon.Length)
                    {
                        doneParsing = true;
                    }
                }
                catch(Exception e)
                {
                    throw new Exception($"Error in getMapSplits: JSON: {json} Type: {e.GetType} StackTrace: {e.StackTrace}");
                }
            }
            if (lines.Count == 0)
            {
                return new List<int>() { trimmedJSon.Length };
            }

            return lines;
        }

        private List<int> getListSplits(string json)
        {
            int pos = 0;
            Stack<CharPosition> containers = new Stack<CharPosition>();
            List<int> lines = new List<int>();
            bool doneParsing = false;
            string trimmedJSon = json.Trim();
            if (!trimmedJSon.Contains(","))
            {
                return new List<int>() { };
            }
            while (!doneParsing)
            {
                char c = trimmedJSon[pos];
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
                    if (containers.Count != 0 && containers.Peek().c == '\"')
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
                if (pos >= trimmedJSon.Length)
                {
                    doneParsing = true;
                }

            }

            return lines;
        }


        private List<JSONObject> parseList(string jsonList)
        {
            List<JSONObject> list = new List<JSONObject>();
            List<int> listSplits = getListSplits(jsonList);
            if (listSplits.Count == 0 && jsonList.Trim() == "[]")
            {
                return new List<JSONObject>();
            }
            else if (listSplits.Count == 0)
            {
                return new List<JSONObject>() { new JSONObject(jsonList.Trim().Substring(1, jsonList.Trim().Length - 2)) };
            }
            list.Add(new JSONObject(jsonList.Substring(1, listSplits[0] - 1)));
            for (int i = 0; i < listSplits.Count; i++)
            {
                //Skip the last one
                if (i + 1 != listSplits.Count)
                {
                    list.Add(new JSONObject(jsonList.Substring(listSplits[i] + 1, listSplits[i + 1] - listSplits[i] - 1)));
                }
            }
            if (jsonList.Contains(","))
            {
                list.Add(new JSONObject(jsonList.Substring(listSplits[listSplits.Count - 1] + 1, jsonList.Length - listSplits[listSplits.Count - 1] - 2)));
            }
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
                if (list.Count == 0)
                    return "[]";
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


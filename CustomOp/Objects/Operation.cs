using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace CustomOp.Objects
{
    internal abstract class Operation
    {
        //TODO build getInputsTypes and getOutputTypes methods here to validate Operation orders
        //TODO also do the validation for the var type to make sure the vars actually exist 
        //So 4 methods for validation - 2 for input and output type and 2 for input output varname

        //Instead of the above could do something weird where I use the op data to store what values I should need whenever I create an operation then have a validate method inside
        //That checks if the both have similar vars with the same types

        Dictionary<string, string> varMappings;
        public string name;
        List<String> tempVars;
        OpData storedVars;
        public Operation(XElement config)
        {
            storedVars = new OpData();
            varMappings = new Dictionary<string, string>();
            tempVars = new List<string>();
            Configure(config);
        }
        public void Configure(XElement element)
        {
            name = element.Attribute("name").Value.ToString();
            if (!(element.Element("Vars") is null))
            {
                var mappings = from mapping in element.Element("Vars").Descendants("Mapping")
                               select new
                               {
                                   Original = mapping.Attribute("varName").Value.ToString(),
                                   New = mapping.Attribute("methodName").Value.ToString()
                               };
                foreach (var mapping in mappings)
                {
                    varMappings.Add(mapping.Original, mapping.New);
                }
                XMLParser.parseDataTags(element.Element("Vars"), storedVars);
            }
        }

        public virtual void execute(OpData data)
        {

        }

        private void processInput(OpData data)
        {
           foreach(string mapping in varMappings.Keys)
            {
                if (data.contains(mapping) || data.contains(mapping.Substring(0, mapping.IndexOf("["))))
                {
                    //varMappings[mapping]=methodVar mapping=varName in config
                    data.put(varMappings[mapping], parseVars(mapping, data, null));
                    //If we are adding a new variable from the var map, remove it in the on exit (if the variable already existed and we are just changing it, keep it)
                    if (!data.contains(varMappings[mapping]))
                    {
                        tempVars.Add(varMappings[mapping]);
                    }
                }
            }
            data.merge(storedVars);
            tempVars.AddRange(storedVars.getIDs());
        }

        public void onError()
        {

        }

        private Object parseVars(string mapping, OpData data, Object preParsedObj)
        {
           //TODO just make parsing way better and more encompassing
            if (mapping.Contains("["))
            {
                //Finds the keys gets the bracket if not immediately follow by another bracket
                MatchCollection keys = Regex.Matches(mapping, "\\[.*\\]\\[");
                string mainKeyName = mapping.Substring(0, mapping.IndexOf("["));
                Object obj = data.getObject(mainKeyName);
                if (obj.GetType().Equals(typeof(Dictionary<String, String>)))
                {
                    return (data.getMap(mainKeyName))[keys.First().ToString().Trim().Replace("[", "").Replace("]", "")];
                }
                else if (obj.GetType().Equals(typeof(JSONObject)))
                {
                    if (keys.Count >= 1 && preParsedObj==null)
                    {
                        //Go one deeper - removes the first mapping 
                        JSONObject oneLayerDeeper = data.getJSONObject(mainKeyName).getMapValue(keys.First().ToString().Trim().Replace("[", "").Replace("]", ""));
                        return parseVars(mapping.Replace(keys.First().ToString().Substring(0, keys.First().ToString().Length-1), ""), data, oneLayerDeeper);
                    } else if (keys.Count >= 1)
                    {
                        return parseVars(mapping.Replace(keys.First().ToString(), ""), data, ((JSONObject)preParsedObj).getMapValue(keys.First().ToString().Trim().Replace("[", "").Replace("]", "")));
                    }
                    else
                    {
                        
                        string key = Regex.Matches(mapping, "\\[.*\\]").First().ToString().Trim().Replace("[", "").Replace("]", "");
                        if (preParsedObj == null)
                        {
                            return (data.getJSONObject(mainKeyName)).getMapValue(key);
                        }
                        else
                        {
                            return ((JSONObject)preParsedObj).getMapValue(key);
                        }
                    }
                }
                else if (obj.GetType().Equals(typeof(List<string>)))
                {
                    string index = keys.First().ToString().Trim().Replace("[", "").Replace("]", "");
                    int intIndex = Int16.Parse(index);
                    return ((List<String>)obj)[intIndex];
                }
            }
            return data.getObject(mapping);
        }

        public void onEnter(OpData data)
        {
            Logger.log.Info($"Starting Operation: {name}");
            processInput(data);
        }

        public void onExit(OpData data)
        {
            foreach(string key in tempVars)
            {
                data.remove(key);
            }
            tempVars.Clear();
            Logger.log.Info($"Finished Operation: {name}");
        }
    }
}

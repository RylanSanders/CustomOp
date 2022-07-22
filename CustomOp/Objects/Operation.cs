using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                if (data.contains(mapping))
                {
                    data.put(varMappings[mapping], data.getObject(mapping));
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

        public void onEnter(OpData data)
        {
            processInput(data);
        }

        public void onExit(OpData data)
        {
            foreach(string key in tempVars)
            {
                data.remove(key);
            }
            tempVars.Clear();
        }
    }
}

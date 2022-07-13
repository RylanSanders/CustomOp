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


        //TODO change the varmap to only work in the space of the operation - otherwise you won't beable to use 2 operations with different inputs, unless you change them which is fine
        Dictionary<string, string> varMappings;
        public string name;

        public Operation(XElement config)
        {
            varMappings = new Dictionary<string, string>();
            Configure(config);
        }
        public void Configure(XElement element)
        {
            name = element.Attribute("name").Value.ToString();
            if (!(element.Element("VarMap") is null))
            {
                var mappings = from mapping in element.Element("VarMap").Descendants("Mapping")
                               select new
                               {
                                   Original = mapping.Attribute("varName").Value.ToString(),
                                   New = mapping.Attribute("methodName").Value.ToString()
                               };
                foreach (var mapping in mappings)
                {
                    varMappings.Add(mapping.Original, mapping.New);
                }
            }
        }

        public virtual void execute(OpData data)
        {
            processInput(data);
        }

        private void processInput(OpData data)
        {
           foreach(string mapping in varMappings.Keys)
            {
                if (data.contains(mapping))
                {
                    data.put(varMappings[mapping], data.getObject(mapping));
                }
            }
        }

        public void onError()
        {

        }
    }
}

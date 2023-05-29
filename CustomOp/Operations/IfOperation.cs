using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class IfOperation : Operation
    {
        Dictionary<string, string> strEqualsCondition = new Dictionary<string, string>();
        Dictionary<string, string> strContainsCondition = new Dictionary<string, string>();
        Dictionary<string, string> mapContainsCondition = new Dictionary<string, string>();
        Dictionary<string, string> listContainsCondition = new Dictionary<string, string>();
        Process trueProcess;
        Process falseProcess;
        public IfOperation(XElement config) : base(config)
        {
            foreach(XElement element in config.Element("Conditions").Elements("strEqual"))
            {
                strEqualsCondition.Add(element.Attribute("varName").Value.ToString(), element.Attribute("value").Value.ToString());
            }
            foreach (XElement element in config.Element("Conditions").Elements("mapContainsKey"))
            {
                mapContainsCondition.Add(element.Attribute("mapName").Value.ToString(), element.Attribute("key").Value.ToString());
            }
            foreach (XElement element in config.Element("Conditions").Elements("listContainsValue"))
            {
                listContainsCondition.Add(element.Attribute("listName").Value.ToString(), element.Attribute("value").Value.ToString());
            }
            foreach (XElement element in config.Element("Conditions").Elements("strContains"))
            {
                strContainsCondition.Add(element.Attribute("stringName").Value.ToString(), element.Attribute("value").Value.ToString());
            }
            XElement staticProcess = config.Element("TrueProcess").Element("Process");
            if (staticProcess == null)
            {
                string processName = config.Element("TrueProcess").Attribute("name").Value.ToString();
                trueProcess = ExecutionController.getNewProcessInstance(processName);
            }
            else
            {
                trueProcess = new Process(config.Element("TrueProcess").Element("Process"));
            }
            if (config.Element("FalseProcess") != null)
            {
                XElement falseStaticProcess = config.Element("FalseProcess").Element("Process");
                if (falseStaticProcess == null)
                {
                    string processName = config.Element("FalseProcess").Attribute("name").Value.ToString();
                    falseProcess = ExecutionController.getNewProcessInstance(processName);
                }
                else
                {
                    falseProcess = new Process(config.Element("FalseProcess").Element("Process"));
                }
            }
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            OpData tempData = new OpData();
            tempData.merge(data);
            Process spawnedThread = null;
            if (meetsCondition(data) && trueProcess!=null)
            {
                spawnedThread = trueProcess.clone();
            }
            else if(falseProcess!=null)
            {
                spawnedThread = falseProcess.clone();
            }
            if (spawnedThread != null)
            {
                spawnedThread.addOpData(tempData);
                ExecutionController.runProcess(spawnedThread);
            }
        }
        

        private bool meetsCondition(OpData data)
        {
            foreach(string varName in strEqualsCondition.Keys)
            {
                string s = Operation.parseVars(varName, data, null).ToString();
                if ( !s.GetType().Equals(typeof(string)) || s != strEqualsCondition[varName])
                {
                    return false;
                }
            }
            foreach(string mapName in mapContainsCondition.Keys)
            {
                Object o = Operation.parseVars(mapName, data, null);
                if (o.GetType() == typeof(Dictionary<string, string>))
                {
                    Dictionary<string, string> map = (Dictionary<string, string>)o;
                    if (!map.ContainsKey(mapContainsCondition[mapName]))
                    {
                        return false;
                    }
                }else if(o.GetType() == typeof(JSONObject))
                {
                    JSONObject map = (JSONObject)o;
                    if (!map.map.ContainsKey(mapContainsCondition[mapName]))
                    {
                        return false;
                    }
                }
                
            }
            foreach (string varName in listContainsCondition.Keys)
            {
                List<string> lst = (List<string>)Operation.parseVars(varName, data, null);
                if (!lst.Contains(listContainsCondition[varName]))
                {
                    return false;
                }
            }
            foreach (string varName in strContainsCondition.Keys)
            {
                string s = Operation.parseVars(varName, data, null).ToString();
                if (!s.GetType().Equals(typeof(string)) || !s.Contains(strContainsCondition[varName]))
                {
                    return false;
                }
            }
            return true;
        }

    }
}

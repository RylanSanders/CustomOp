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
        Process trueProcess;
        Process falseProcess;
        public IfOperation(XElement config) : base(config)
        {
            foreach(XElement element in config.Element("Conditions").Elements("strEqual"))
            {
                strEqualsCondition.Add(element.Attribute("varName").Value.ToString(), element.Attribute("value").Value.ToString());
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
            XElement falseStaticProcess = config.Element("FalseProcess").Element("Process");
            if (staticProcess == null)
            {
                string processName = config.Element("FalseProcess").Attribute("name").Value.ToString();
                falseProcess = ExecutionController.getNewProcessInstance(processName);
            }
            else
            {
                falseProcess = new Process(config.Element("FalseProcess").Element("Process"));
            }
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            OpData tempData = new OpData();
            tempData.merge(data);
            Process spawnedThread;
            if (meetsCondition(data))
            {
                spawnedThread = trueProcess.clone();
            }
            else
            {
                spawnedThread = falseProcess.clone();
            }
            spawnedThread.addOpData(tempData);
            ExecutionController.runProcess(spawnedThread);
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
            return true;
        }
    }
}

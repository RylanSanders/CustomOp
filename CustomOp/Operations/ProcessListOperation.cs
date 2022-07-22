using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;
namespace CustomOp.Operations
{
    internal class ProcessListOperation : Operation
    {
        Process operatedProcess;
        public ProcessListOperation(XElement config) : base(config)
        {
            operatedProcess = new Process(config.Element("OperatedProcess").Element("Process"));
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            List<int> l = data.getIntList("InputList");
            foreach(int i in l)
            {
                OpData tempData = new OpData();
                tempData.merge(data);
                tempData.put("ListIntVal", i);
                Process spawnedThread = operatedProcess.clone();
                spawnedThread.addOpData(data);
                ExecutionController.runProcess(spawnedThread);
            }
        }
    }
}

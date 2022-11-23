using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class RunDataProcessOperation : Operation
    {

        Process dataProcess;
        public RunDataProcessOperation(XElement config) : base(config)
        {
            XElement ProcessID = config.Element("ProcessID");
            if (ProcessID == null)
            {
                throw new Exception("Error in Run DataProcess Operation - Missing the ProcessID Config");
            }
            if (!ExecutionController.idProcesses.Keys.Contains(ProcessID.Value))
            {
                throw new Exception($"Error in Run DataProcess Operation - Invalid Process Name:{ProcessID.Value}");
            }
            dataProcess = ExecutionController.getNewProcessInstance(ProcessID.Value);
        }


        public override void execute(OpData data)
        {
            base.execute(data);

            Process spawnedProcess = dataProcess.clone();
            spawnedProcess.addOpData(data);
            ExecutionController.syncRunProcess(spawnedProcess);
        }
    }
}

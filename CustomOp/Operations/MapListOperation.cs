using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class MapListOperation : Operation
    {
        Process mappingProcess;
        public MapListOperation(XElement config) : base(config)
        {
            mappingProcess = new Process(config.Element("MappingProcess").Element("Process"));
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            //The Var will be ListValue and the output should be MappingOutput

            List<string> lst = data.getStringList("ToMapList");
            List<string> newLst = new List<string>();
            foreach (string i in lst)
            {
                OpData tempData = new OpData();
                tempData.merge(data);
                tempData.put("ListValue", i);
                Process spawnedThread = mappingProcess.clone();
                spawnedThread.addOpData(tempData);
                ExecutionController.syncRunProcess(spawnedThread);
                newLst.Add(spawnedThread.inputData.getString("MappingOutput"));
            }

            data.put("MappedList", newLst);
        }
    }
}

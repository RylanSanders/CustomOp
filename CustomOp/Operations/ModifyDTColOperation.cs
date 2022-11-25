using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class ModifyDTColOperation : Operation
    {
        //TODO Merge this with the General Modify Operation
        Process MappingProcess;
        public ModifyDTColOperation(XElement config) : base(config)
        {
            MappingProcess = new Process(config.Element("MappingProcess").Element("Process"));
        }

        public override void execute(OpData data)
        {
            base.execute(data);
            //The Var will be TableValue and the output should be MappingOutput
            DataTable dt = data.getDataTable("ToModifyDT");
            string colName = data.getString("ColName");

            List<string> newCol = new List<string>();
            foreach (string i in dt.getColumn(colName))
            {
                OpData tempData = new OpData();
                tempData.merge(data);
                tempData.put("TableValue", i);
                Process spawnedThread = MappingProcess.clone();
                spawnedThread.addOpData(tempData);
                ExecutionController.syncRunProcess(spawnedThread);
                newCol.Add(spawnedThread.inputData.getString("MappingOutput"));
            }

            dt.removeCol(colName);
            dt.addCol(colName, newCol);
            data.put("ModifiedDT", dt);
        }
    }
    }

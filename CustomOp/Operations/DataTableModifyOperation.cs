using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;
namespace CustomOp.Operations
{
    internal class DataTableModifyOperation : Operation
    {
        IEnumerable<String> KeepOnlyColumns;
        bool isKeepOnly = false;

        Dictionary<String, Dictionary<String, String>> newBoolCols;

        public DataTableModifyOperation(XElement config) : base(config)
        {
            XElement modificationOps = config.Element("DTOperations");
            XElement keepOnlyElement = modificationOps.Element("KeepOnly");
            if(keepOnlyElement != null)
            {
                isKeepOnly = true;
                KeepOnlyColumns = keepOnlyElement.Elements("Col").Select(e => e.Attribute("name").Value);
            }

            XElement createColElement = modificationOps.Element("CreateCols");
            newBoolCols = new Dictionary<String, Dictionary<String, String>>();
            if(createColElement != null)
            {
                foreach(XElement col in createColElement.Elements("Col"))
                {
                    if (col.Attribute("type").Value == "bool")
                    {
                        Dictionary<String, String> inputs = new Dictionary<String, String>() ;
                        foreach(XElement condition in col.Element("Conditions").Elements("Condition"))
                        {
                            condition.Attributes().ToList().ForEach((x) => inputs.Add(x.Name.ToString(), x.Value.ToString()));
                        }
                        newBoolCols.Add(col.Attribute("name").Value, inputs);
                    }
                   
                }
            }
            
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            DataTable dt = data.getDataTable("InputDataTable");

            if (isKeepOnly)
            {
                keepOnlyFilter(dt);
            }
            foreach(string newCol in newBoolCols.Keys)
            {
                testCondition(dt, newCol, newBoolCols[newCol]);
            }

            data.put("ModifiedDataTable", dt);
        }

        private void keepOnlyFilter(DataTable t)
        {
            List<String> toRemoveCols = t.getCols();
            toRemoveCols.RemoveAll((c) => KeepOnlyColumns.Contains(c));
            toRemoveCols.ForEach(col => t.removeCol(col));
        }


        private void testCondition(DataTable table, string colName,  Dictionary<String, String> conditionVars) {
            if (conditionVars["type"] == "contains")
            {
                List<String> col = table.getColumn(conditionVars["col"]);
                string val = conditionVars["value"];
                table.addCol(colName, col.Select((x) => x.Contains(val).ToString()).ToList());
            }
        }


    }
}

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
        bool generateNewDataTable = false;
        int emptyRows = 0;
        bool ignoreBadCols = false;

        Dictionary<String, Dictionary<String, String>> newBoolCols;
        Dictionary<String, String> newStaticStringCols;
        Dictionary<String, String> addNewCols;

        List<Dictionary<string, string>> newRows = new List< Dictionary<string, string>>();

        public DataTableModifyOperation(XElement config) : base(config)
        {
            XElement generateNewDataTableElement = config.Element("GenerateNewDataTable");
            if(generateNewDataTableElement!=null && (generateNewDataTableElement.Value=="True" || generateNewDataTableElement.Value == "true"))
            {
                generateNewDataTable = true;
            }

            XElement emptyRowsElement = config.Element("EmptyRows");
            if (generateNewDataTable && emptyRowsElement != null )
            {
                emptyRows = int.Parse(emptyRowsElement.Value);
            }

            XElement ignoreBadColElement = config.Element("IgnoreBadColumns");
            if ( ignoreBadColElement != null)
            {
                ignoreBadCols = Boolean.Parse(ignoreBadColElement.Value);
            }

            XElement modificationOps = config.Element("DTOperations");
            XElement keepOnlyElement = modificationOps.Element("KeepOnly");
            if(keepOnlyElement != null)
            {
                isKeepOnly = true;
                KeepOnlyColumns = keepOnlyElement.Elements("Col").Select(e => e.Attribute("name").Value);
            }

            XElement createColElement = modificationOps.Element("CreateCols");
            newBoolCols = new Dictionary<String, Dictionary<String, String>>();
            newStaticStringCols = new Dictionary<string, string>();
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
                    else if(col.Attribute("type").Value == "staticVarString")
                    {
                        newStaticStringCols.Add(col.Attribute("ColName").Value, col.Attribute("VarName").Value);
                    }
                   
                }
            }

            //Add new rows
            XElement CreateRowsElement = config.Element("CreateRows");
            if (CreateRowsElement != null)
            {
                var newCols = from input in CreateRowsElement.Descendants()
                              select new
                              {
                                  values = input.Attributes().ToList()
                              };

                foreach (var input in newCols)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    foreach (XAttribute att in input.values)
                    {
                        dict.Add(att.Name.ToString(), att.Value);
                    }
                    newRows.Add(dict);
                }
            }
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            DataTable dt = new DataTable();

            if (!generateNewDataTable)
            {
                dt = data.getDataTable("InputDataTable");
            }
            else { 
                for(int i = 0; i < emptyRows; i++)
                {
                    Dictionary<string, string> map = new Dictionary<string, string>();
                    map.Add("FakeEmptyRow", i.ToString());
                    dt.addRow( map);
                }
            }

            if (isKeepOnly)
            {
                keepOnlyFilter(dt);
            }
            foreach(string newCol in newBoolCols.Keys)
            {
                testCondition(dt, newCol, newBoolCols[newCol]);
            }
            if (newRows.Count > 0)
            {
                dt.addRows(newRows);
            }
            foreach(string newStaticVarCol in newStaticStringCols.Keys)
            {
                tryGenerateVarCol(newStaticVarCol, dt.getMaxNumRows(), data, dt);
            }

            
            data.put("ModifiedDataTable", dt);

        }

        private void tryGenerateVarCol(string s, int num, OpData data, DataTable dt)
        {
            if (ignoreBadCols)
            {
                try
                {
                    dt.addCol(s, generateVarCol(newStaticStringCols[s], dt.getMaxNumRows(), data));
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                dt.addCol(s, generateVarCol(newStaticStringCols[s], dt.getMaxNumRows(), data));
            }
        }


        private List<string> generateVarCol(string s, int num, OpData data)
        {
            List<string> toRet = new List<string>();
            string var = Operation.parseVars(s, data, null).ToString();
            for(int i = 0; i < num; i++)
            {
                toRet.Add(var);
            }
            return toRet;
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

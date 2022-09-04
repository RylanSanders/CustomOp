using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomOp.Objects
{
    internal class DataTable
    {
        public Dictionary<string, List<string>> colValues;

        public DataTable()
        {
            colValues = new Dictionary<string, List<string>>();
        }

        public DataTable(Dictionary<string, List<string>> table)
        {
            colValues = table;
        }

        public List<string> getColumn(string colName)
        {
            return colValues[colName];
        }

        public void addRow(Dictionary<string, string> values)
        {
            foreach(string key in values.Keys)
            {
                if (colValues.ContainsKey(key))
                {
                    colValues[key].Add(values[key]);
                }
                else
                {
                    colValues.Add(key, getEmptyStringList(getMaxNumRows()));
                    colValues[key].Add(values[key]);
                }
                
            }

            foreach(string key in colValues.Keys)
            {
                if (!values.ContainsKey(key))
                {
                    colValues[key].Add(String.Empty);
                }
            }
        }

        public int getMaxNumRows()
        {
            if (colValues.Count == 0)
                return 0;
            return colValues[colValues.Keys.FirstOrDefault()].Count;
        }

        private List<string> getEmptyStringList(int length)
        {
            List<string> toRet = new List<string>();
            for(int i = 0; i < length-1; i++)
            {
                toRet.Add(String.Empty);
            }
            return toRet;
        }

        public void addRows(List<Dictionary<string, string>> rows)
        {
            foreach(Dictionary<string, string> row in rows)
            {
                addRow(row);
            }
        }

        public string generateInsertStatement(string database)
        {
            StringBuilder sb = new StringBuilder($"INSERT INTO {database} (");
            foreach (string k in colValues.Keys)
            {
                sb.Append($"{k},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") VALUES ");

            int rowNum = 0;
            while (rowNum < colValues[colValues.Keys.FirstOrDefault()].Count)
            {

                sb.Append("\n (");
                foreach (string key in colValues.Keys)
                {
                    sb.Append($"'{colValues[key][rowNum]}',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("),");
                rowNum++;
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public string generateInsertStatement(string database, Dictionary<string, string> colMap)
        {
            StringBuilder sb = new StringBuilder($"INSERT INTO {database} (");
            foreach (string k in colValues.Keys)
            {
                if (colMap.ContainsValue(k))
                {
                    string key = colMap.Keys.Where((x) => colMap[x] == k).FirstOrDefault();
                    sb.Append($"{key},");
                }
                
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") VALUES ");

            IEnumerable<string> usedColumns = colValues.Keys.Intersect(colMap.Keys);
            int rowNum = 0;
            while (rowNum < colValues[colValues.Keys.FirstOrDefault()].Count)
            {

                sb.Append("\n (");
                foreach (string key in usedColumns)
                {
                    sb.Append($"'{colValues[key][rowNum].Replace("\'", "").Replace("\"", "")}',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("),");
                rowNum++;
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(string key in colValues.Keys)
            {
                sb.Append($"{key}, ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.AppendLine();
            for(int i=0;i<getMaxNumRows();i++)
            {
                foreach(string key in colValues.Keys)
                {
                    sb.Append($"{colValues[key][i]}, ");
                }
                sb.Remove(sb.Length - 2, 2);
                sb.AppendLine();
            }
            return sb.ToString();
        }


    }
}

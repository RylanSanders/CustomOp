using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace CommandPanel
{
    internal class SQLSelectDataRetreiver
    {
        string SqlQuerry = "";
        string connnectionString = string.Empty;
        public SQLSelectDataRetreiver(XElement config)
        {
            SqlQuerry = config.Element("SQLQuery").Value;
            connnectionString = config.Element("ConnectionString").Value.ToString();
        }

        public void applyDataRetrevier(Panel control, Dictionary<string, string> args)
        {
            string query = MainWindow.getVar(SqlQuerry, args);
            string connString = MainWindow.getVar(connnectionString, args);
            DataGrid dg = DataGridGenerator.GenerateDataGridFromTable(connString,query );
            control.Children.Add(dg);
            
        }
    }
}

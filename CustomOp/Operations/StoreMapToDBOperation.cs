using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class StoreMapToDBOperation : Operation
    {
        Dictionary<string, string> ColToVar;

        private string dataSource;
        private string InitialCatalog;
        private string Password;
        private string User;
        private string DataBase;

        public StoreMapToDBOperation(XElement config) : base(config)
        {
            //TODO put this in a connection section
             dataSource = config.Element("DataSource").Value;
            InitialCatalog = config.Element("InitialCatalog").Value;
            Password = config.Element("Password").Value;
            User = config.Element("User").Value;
            DataBase = config.Element("DataBase").Value;

            //DBCol is the database column name and the VarName is the name of the variable in the map
            ColToVar = new Dictionary<string, string>();
            var inputs = from input in config.Descendants("DBCols")
                         select new
                         {
                             Name = input.Attribute("DBCol").Value.ToString(),
                             Value = input.Attribute("VarName").Value.ToString(),
                         };

            foreach (var input in inputs)
            {
                ColToVar.Add(input.Name, input.Value);
            }
        }

        public override void execute(OpData data)
        {
            base.execute(data);
            //THIS ONLY DOES A SINGLE PUT OPERATION, NOT MULTIPLE
            Dictionary<string, string> map = data.getMap("MapToDB");
            //Start Insert Statement
            StringBuilder sb = new StringBuilder($"INSERT {DataBase} (");
            foreach (string k in ColToVar.Keys)
            {
                sb.Append($"{k},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") VALUES \n (");

            
            foreach (string val in ColToVar.Values)
            {
                //TODO make it so that if the map doesn't contain the value then try getting the string from the map
                sb.Append($"'{map[val]}',");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")");
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = dataSource;
            csb.InitialCatalog = InitialCatalog;
            csb.IntegratedSecurity = true;
            csb.Password = Password;
            csb.UserID = User;
            SqlConnection sqlConnection1 = new SqlConnection(csb.ToString());
            using (SqlConnection connection = sqlConnection1)
            {
                // Connect to the database then retrieve the schema information.  
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = sqlConnection1;
                cmd.CommandText = sb.ToString();
                cmd.ExecuteNonQuery();
                connection.Close();
            }

        }
    }
}

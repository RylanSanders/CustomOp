using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CustomOp.Objects;

namespace CustomOp.Operations
{
    internal class StoreTableToDBOperation : Operation
    {
        Dictionary<string, string> ColToVar;

        private string dataSource;
        private string InitialCatalog;
        private string Password;
        private string User;
        private string DataBase;
        public StoreTableToDBOperation(XElement config) : base(config)
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

            DataTable dt = data.getDataTable("TableToDB");

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = dataSource;
            csb.InitialCatalog = InitialCatalog;
            csb.IntegratedSecurity = true;
            csb.Password = Password;
            csb.UserID = User;
            SqlConnection sqlConnection1 = new SqlConnection(csb.ToString());
            try
            {
                using (SqlConnection connection = sqlConnection1)
                {
                    // Connect to the database then retrieve the schema information.  
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = sqlConnection1;
                    cmd.CommandText = dt.generateInsertStatement(DataBase, ColToVar);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            } catch(Exception e)
            {
                throw new Exception($"SQL Error in StoreTableToDB: {e.Message} at {e.StackTrace} with querry: {dt.generateInsertStatement(DataBase, ColToVar)}");
            }
        }
    }
}

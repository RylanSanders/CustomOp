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
    internal class SQLSelectOperation : Operation
    {
        private string dataSource;
        private string InitialCatalog;
        private string Password;
        private string User;
        private string DataBase;
        public SQLSelectOperation(XElement config) : base(config)
        {
            //TODO put this in a connection section
            dataSource = config.Element("DataSource").Value;
            InitialCatalog = config.Element("InitialCatalog").Value;
            Password = config.Element("Password").Value;
            User = config.Element("User").Value;
            DataBase = config.Element("DataBase").Value;
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = dataSource;
            csb.InitialCatalog = InitialCatalog;
            csb.IntegratedSecurity = true;
            csb.Password = Password;
            csb.UserID = User;
            string[] colNames = getColumnsName(csb.ToString());
            Dictionary<string, List<string>> table = new Dictionary<string, List<string>>();
            foreach(string colName in colNames)
            {
                table.Add(colName, new List<string>());
            }
            SqlConnection sqlConnection1 = new SqlConnection(csb.ToString());
            using (SqlConnection connection = sqlConnection1)
            {
                // Connect to the database then retrieve the schema information.  
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = sqlConnection1;
                cmd.CommandText = data.getString("SQLSelectStatement");
                SqlDataReader reader = cmd.ExecuteReader();

                // Call Read before accessing data.
                while (reader.Read())
                {
                    IDataRecord row = ((IDataRecord)reader);
                    for(int i = 0; i < row.FieldCount; i++)
                    {
                        if(row.GetDataTypeName(i) == "int")
                        {
                            table[row.GetName(i)].Add( row.GetInt32(i).ToString());
                        }
                        else if(row.GetDataTypeName(i) == "nchar")
                        {
                            table[row.GetName(i)].Add(row.GetString(i));
                        }
                       
                    }
                }

                // Call Close when done reading.
                reader.Close();
            }

            data.put("SelectedTable", new Objects.DataTable(table));
        }

        //https://stackoverflow.com/questions/19392331/get-column-name-from-sql-server
        private string[] getColumnsName(string Connection)
        {
            List<string> listacolumnas = new List<string>();
            using (SqlConnection connection = new SqlConnection(Connection))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = $"select c.name from sys.columns c inner join sys.tables t on t.object_id = c.object_id and t.name = '{DataBase}' and t.type = 'U'";
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listacolumnas.Add(reader.GetString(0));
                    }
                }
            }
            return listacolumnas.ToArray();
        }

    }
}

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
        private SQLLogin login;
        private string DataBase;
        public SQLSelectOperation(XElement config) : base(config)
        {
            if (SQLLogin.isSQLLogin(config))
            {
                login = new SQLLogin(config);
            }
            DataBase = config.Element("DataBase").Value;
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            if (login == null)
            {
                if (!data.contains("SQLLogin"))
                {
                    throw new Exception("No Valid SQLLogin!");
                }
                login = data.getSQLLogin("SQLLogin");
            }
            csb.DataSource = login.DataSource;
            csb.InitialCatalog = login.InitialCatalog;
            csb.IntegratedSecurity = true;
            csb.Password = login.Password;
            csb.UserID = login.UserID;
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
                        string datatype = row.GetDataTypeName(i);
                        if (row.IsDBNull(i))
                        {
                            table[row.GetName(i)].Add("NA");
                        }
                        else if(datatype == "tinyint")
                        {
                            table[row.GetName(i)].Add(((int)row.GetByte(i)).ToString());
                        }
                        else if (datatype == "int" )
                        {
                            table[row.GetName(i)].Add(row.GetInt32(i).ToString());
                        }
                        else if (datatype == "bigint")
                        {
                            table[row.GetName(i)].Add(row.GetInt64(i).ToString());
                        }
                        else if (datatype == "nchar" || datatype=="varchar" || datatype=="nvarchar" || datatype=="xml")
                        {
                           table[row.GetName(i)].Add(row.GetString(i));
                        }
                        else if (datatype == "bit")
                        {
                            table[row.GetName(i)].Add(row.GetBoolean(i).ToString());
                        }
                        else if(datatype == "datetime")
                        {
                            table[row.GetName(i)].Add(row.GetDateTime(i).ToString());
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

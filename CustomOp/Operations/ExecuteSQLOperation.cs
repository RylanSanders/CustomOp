using CustomOp.Objects;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{

    internal class ExecuteSQLOperation : Operation
    {
        private string dataSource;
        private string InitialCatalog;
        private string Password;
        private string User;
        public ExecuteSQLOperation(XElement config) : base(config)
        {
            //TODO put this in a connection section
            dataSource = config.Element("DataSource").Value;
            InitialCatalog = config.Element("InitialCatalog").Value;
            Password = config.Element("Password").Value;
            User = config.Element("User").Value;
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
            SqlConnection sqlConnection = new SqlConnection(csb.ToString());

            using (SqlConnection connection = sqlConnection)
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(data.getString("SQLStatement"), connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

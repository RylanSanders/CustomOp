using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class AttachDatabaseOperation : Operation
    {
        private string dataSource;
        private string InitialCatalog;
        private string Password;
        private string User;
        public AttachDatabaseOperation(XElement config) : base(config)
        {
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
            string mdfFilePath = data.getString("MDFFilePath");
            string ldfFilePath = mdfFilePath.Replace("mdf","ldf");
            string databaseName = Path.GetFileNameWithoutExtension(mdfFilePath);
            string attachQuery = $"CREATE DATABASE [{databaseName}] ON (FILENAME = '{mdfFilePath}'), " +
                         $"(FILENAME = '{ldfFilePath}') FOR ATTACH";

            using (SqlConnection connection = sqlConnection)
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(attachQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

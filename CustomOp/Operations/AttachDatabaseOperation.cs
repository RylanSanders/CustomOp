using CustomOp.Objects;
using Microsoft.VisualBasic.Logging;
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
        private SQLLogin login;
        public AttachDatabaseOperation(XElement config) : base(config)
        {
            if (SQLLogin.isSQLLogin(config))
            {
                login = new SQLLogin(config);
            }
        }

        public override void execute(OpData data)
        {
            base.execute(data);
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            if(login == null)
            {
                if (!data.contains("SQLLogin"))
                {
                    throw new Exception("No Valid SQLLogin!");
                }
                data.getString("SQLLogin");
            }
            csb.DataSource = login.DataSource;
            csb.InitialCatalog = login.InitialCatalog;
            csb.IntegratedSecurity = true;
            csb.Password = login.Password;
            csb.UserID = login.UserID;
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

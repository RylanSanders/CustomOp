using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class DetachDatabaseOperation : Operation
    {
        private SQLLogin login;

        public DetachDatabaseOperation(XElement config) : base(config)
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
            SqlConnection sqlConnection1 = new SqlConnection(csb.ToString());
            string databaseName = data.getString("DBToDetach");
            using (SqlConnection connection = sqlConnection1)
            {
                connection.Open();
                string detachQuery = $"EXEC sp_detach_db '{databaseName}'";
                string alterQuery = $"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                using (SqlCommand command = new SqlCommand(alterQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand(detachQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

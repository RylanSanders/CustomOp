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
        private string dataSource;
        private string InitialCatalog;
        private string Password;
        private string User;
        
        public DetachDatabaseOperation(XElement config) : base(config)
        {
            dataSource = config.Element("DataSource").Value;
            InitialCatalog = config.Element("InitialCatalog").Value;
            Password = config.Element("Password").Value;
            User = config.Element("User").Value;
        }

        public override void execute(OpData data)
        {

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = dataSource;
            csb.InitialCatalog = InitialCatalog;
            csb.IntegratedSecurity = true;
            csb.Password = Password;
            csb.UserID = User;
            base.execute(data);
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

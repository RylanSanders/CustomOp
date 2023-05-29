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

        private SQLLogin login;
        public ExecuteSQLOperation(XElement config) : base(config)
        {
            if(SQLLogin.isSQLLogin(config))
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

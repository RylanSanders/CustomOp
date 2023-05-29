using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Objects
{
    public class SQLLogin
    {
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }

        public SQLLogin(XElement config)
        {
            DataSource = config.Element("DataSource").Value;
            InitialCatalog = config.Element("InitialCatalog").Value;
            Password = config.Element("Password").Value;
            UserID = config.Element("User").Value;
        }

        public static bool isSQLLogin(XElement config)
        {
            return config.Element("DataSource") != null && config.Element("InitialCatalog") != null && config.Element("Password") != null && config.Element("User") != null;
        }
    }
}

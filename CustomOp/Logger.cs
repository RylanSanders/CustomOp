using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomOp
{
    
    internal class Logger
    {
        public static log4net.ILog log = log4net.LogManager.GetLogger("file");
        public static void loadLogger()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            log = log4net.LogManager.GetLogger("file");
        }
    }
}

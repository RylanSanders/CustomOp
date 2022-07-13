using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Objects
{
    internal class ExecutionController
    {
        public List<Process> processes;
        public ExecutionController()
        {
            processes = new List<Process>();
            //Load xml
            XDocument xdoc = XDocument.Load("Operation.xml");
            foreach(XElement e in xdoc.Root.Elements())
            {
                if (e.Name.ToString().Equals("Process"))
                {
                    processes.Add(new Process(e));
                }
            }
        }

        public void runProcess(Process process)
        {
            try
            {
                ProcessThread opt = new ProcessThread(process);
                opt.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }


    }
}

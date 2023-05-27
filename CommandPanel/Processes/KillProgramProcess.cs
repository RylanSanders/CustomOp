using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Process = System.Diagnostics.Process;
namespace CommandPanel.Processes
{
    class KillProgramProcess : Process
    {
        string programName;
        public KillProgramProcess(XElement element)
        {
            programName = element.Element("ProgramName").Value;
        }
        public void execute(Dictionary<string, string> args)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(MainWindow.getVar(programName, args));
            foreach (System.Diagnostics.Process p in processes)
            {
                p.Kill();
            }
        }

        public bool isComplete(Dictionary<string, string> args)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(MainWindow.getVar(programName, args));
            return processes.Count() == 0;
        }
    }
}

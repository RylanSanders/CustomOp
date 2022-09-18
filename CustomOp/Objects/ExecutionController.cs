﻿using System;
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
        public static Dictionary<string, Process> idProcesses;
        public ExecutionController()
        {
            processes = new List<Process>();
            //Load xml
            XDocument xdoc = XDocument.Load("Operation.xml");
            idProcesses = new Dictionary<string, Process>();
            foreach(XElement e in xdoc.Root.Elements())
            {
                if (e.Name.ToString().Equals("Process"))
                {
                    processes.Add(new Process(e));
                    idProcesses.Add(e.Attribute("name").Value.ToString(), new Process(e));
                }
            }

        }

        public static void runProcess(Process process)
        {
            try
            {
                ProcessThread opt = new ProcessThread(process);
                opt.Start();
            }
            catch (Exception e)
            {
                Logger.log.Error(e.StackTrace);
            }

        }

        public static Process getNewProcessInstance(string idName)
        {
            return idProcesses[idName].clone();
        }

        public static void syncRunProcess(Process process)
        {
            try
            {
                process.onEnter();
                process.run();
                process.onExit();
            }
            catch (Exception e)
            {
                Logger.log.Error(e.StackTrace);
            }
        }


    }
}

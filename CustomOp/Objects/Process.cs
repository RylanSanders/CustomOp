using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Objects
{
    public class Process
    {
        public OpData inputData;
        public string name;
        List<Operation> ops;

        public Process(XElement config)
        {
            ops = new List<Operation>();
            foreach (XElement x in config.Elements())
            {
                if (x.Name.ToString().Equals("Operation"))
                {
                    ops.Add(XMLParser.parseOpXML(x));
                }
            }
            inputData = new OpData();
            name = config.Attribute("name").Value.ToString();
        }

        public Process(XElement config, OpData presetData)
        {
            ops = new List<Operation>();
            foreach (XElement x in config.Elements())
            {
                if (x.Name.ToString().Equals("Operation"))
                {
                    ops.Add(XMLParser.parseOpXML(x));
                }
            }
            inputData = new OpData();
            inputData.merge(presetData);
            name = config.Attribute("name").Value.ToString();
        }

        public Process(List<Operation> listOps, OpData presetData)
        {
            ops = listOps;
            inputData = presetData;
        }

        public void run()
        {
            foreach(Operation op in ops)
            {
                try
                {
                    op.onEnter(inputData);
                    op.execute(inputData);
                    op.onExit(inputData);
                }
                catch(Exception e)
                {
                    MessageBox.Show($"Error in {op.name} of type {op.GetType()}. \nMessage:{e.Message}. \nStackTrace:{e.StackTrace}");
                    Logger.log.Error("Error in run method of process message: " + e.Message + "\n StackTrace:" + e.StackTrace);
                    op.onError();
                    break;
                }
            }
        }

        public void addOpData(OpData data)
        {
            inputData = data.merge(inputData);
        }

        public Process clone()
        {
            Process c = new Process(ops, inputData);
            c.name = name;
            return c;
        }

        public Action generateAction()
        {
            return () =>
            {
                onEnter();
                run();
                onExit();
            };
        }

        public void onEnter()
        {
            Logger.log.Info($"Starting Process: {name}");
        }


        public void onExit()
        {
            Logger.log.Info($"Finished Process: {name}");
        }


    }
}

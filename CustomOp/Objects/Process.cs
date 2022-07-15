using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Objects
{
    internal class Process
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

        public void run()
        {
            foreach(Operation op in ops)
            {
                try
                {
                    op.execute(inputData);
                }
                catch(Exception e)
                {
                    op.onError();
                }
            }
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

using Antlr4.Runtime.Misc;
using CustomOp2V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Script
{
    internal class ScriptVisitor: ScriptBaseVisitor<XElement>
    {
        public override XElement VisitArgs([NotNull] ScriptParser.ArgsContext context)
        {
            XElement args = new XElement("Vars");
            foreach (ScriptParser.Arg_partContext arg in context.arg_part())
            {
                args.Add(Visit(arg));
            }
            return base.VisitArgs(context);
        }

        public override XElement VisitArg_part([NotNull] ScriptParser.Arg_partContext context)
        {
            string methodArg = context.WORD(0).ToString();
            string varArg = context.WORD(1).ToString();

            if (varArg.StartsWith("$"))
            {
                XElement mapping = new XElement("Mapping");
                mapping.SetAttributeValue("methodName", methodArg);
                mapping.SetAttributeValue("varName", varArg);
                return mapping;
            }

            XElement toRet = new XElement("Data");
            toRet.SetAttributeValue("name", methodArg);
            toRet.SetAttributeValue("type", "string");
            toRet.SetAttributeValue("value", varArg);
            return toRet;
        }

        public override XElement VisitLine([NotNull] ScriptParser.LineContext context)
        {
            XElement operation = Visit(context.operation());
            operation.Add(Visit(context.args()));
            try
            {
                if (context!= null)
                {
                    //THIS is DUMB but it works
                    string cont = context.GetText(); 
                    string maybeXML = cont.Substring(cont.LastIndexOf(")") + 1, (cont.Length - cont.LastIndexOf(")") + 1) -3);
                    operation.Add(XElement.Parse(maybeXML));
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return operation;
        }
        


        public override XElement VisitOperation([NotNull] ScriptParser.OperationContext context)
        {
            XElement operation = new XElement("Operation");
            operation.SetAttributeValue("name", context.WORD().ToString());
            operation.SetAttributeValue("type", context.WORD().ToString());
            return operation;
        }

        public override XElement VisitProcesses([NotNull] ScriptParser.ProcessesContext context)
        {
            XElement processes = new XElement("Processes");
            foreach (ScriptParser.ProcessContext process in context.process())
            {
                processes.Add(Visit(process));
            }
            return processes;
        }

        public override XElement VisitProcess([NotNull] ScriptParser.ProcessContext context)
        {
            XElement process = new XElement("Process");
            process.SetAttributeValue("name", context.WORD().ToString());

            foreach (ScriptParser.LineContext arg in context.line())
            {
                process.Add(Visit(arg));
            }
            return process;
        }
    }
}

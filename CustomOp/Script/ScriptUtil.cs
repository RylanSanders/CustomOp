using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomOp2V2;

namespace CustomOp.Script
{
    public class ScriptUtil
    {

        public static void parseScript(string toParse)
        {
            var str = new AntlrInputStream(toParse);
            var lexer = new ScriptLexer(str);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ScriptParser(tokens);
            var tree = parser.processes(); ;
            ScriptVisitor visitor = new ScriptVisitor();
            parser.BuildParseTree = true;
            
            MessageBox.Show(toParse);
            MessageBox.Show((tree.ToStringTree(parser)));
            MessageBox.Show(visitor.Visit(tree).ToString());
            //return visitor.Visit(tree);
        }
    }
}

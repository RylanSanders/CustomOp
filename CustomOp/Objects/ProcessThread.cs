using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomOp.Objects
{
    internal class ProcessThread : Task
    {
        public ProcessThread(Process p) : base(p.generateAction())
        {
        }

        
    }
}

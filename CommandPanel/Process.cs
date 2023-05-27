using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPanel
{
    public interface Process
    {
        public void execute(Dictionary<string, string> args);
        public bool isComplete(Dictionary<string, string> args);
    }
}

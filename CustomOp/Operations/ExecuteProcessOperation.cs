using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class ExecuteProcessOperation : Operation
    {
        public ExecuteProcessOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);

            System.Diagnostics.Process process = new System.Diagnostics.Process();

            // Set the start info for the process
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = data.getString("ProcessFileName"); // Specify the command-line executable
            startInfo.Arguments = data.getString("Args"); // Specify the command to execute
            startInfo.RedirectStandardOutput = true; // Redirect the output for reading
            startInfo.UseShellExecute = false; // Do not use the operating system shell to start the process

            process.StartInfo = startInfo;

            // Start the process
            process.Start();

            // Read the output
            string output = process.StandardOutput.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();
        }
    }
}

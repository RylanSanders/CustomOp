using CustomOp.Objects;

namespace CustomOp
{
    public partial class Form1 : Form
    {
        ExecutionController controller;
        Dictionary<String, Process> processes;
        public Form1()
        {
            InitializeComponent();

            controller = new ExecutionController();
            processes = new Dictionary<string, Process>();
            foreach(Process p in controller.processes)
            {
                processes.Add(p.name, p);
                Button b = new Button();
                b.Text = p.name;
                b.MouseClick += new MouseEventHandler((sender, e)=>controller.runProcess(p));
                this.flowLayoutPanel1.Controls.Add(b);
            }

        }


    }
}
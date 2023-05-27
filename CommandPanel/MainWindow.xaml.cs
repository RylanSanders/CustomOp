using CommandPanel.Processes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CommandPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Dictionary<string, string> GlobalVars= new Dictionary<string, string>();
        public MainWindow()
        {
            InitializeComponent();
            XElement config = XDocument.Load("TestConfig.xml").Root;
            config.Element("GlobalVars").Elements("Var")
                .ToList()
                .ForEach(v => GlobalVars.Add(v.Attribute("Name").Value, v.Value));

            config.Element("Panels").Elements("Panel")
                .Where(xml => xml.Attribute("Type").Value.Equals("Action"))
                .ToList()
                .ForEach(loadActionPanel);

            config.Element("Panels").Elements("Panel")
                .Where(xml => xml.Attribute("Type").Value.Equals("Display"))
                .ToList()
                .ForEach(loadDisplayPanel);

            Main.Children.OfType<DisplayPanel>().ToList().ForEach(dp => dp.refreshData(new Dictionary<string, string>()));
        }

        private void loadActionPanel(XElement config)
        {
            InputPanel pnl = new InputPanel(config);
            Main.Children.Add(pnl);
            Grid.SetColumn(pnl, Main.Children.Count-1);
            Grid.SetRow(pnl, 0);
        }

        private void loadDisplayPanel(XElement config)
        {
            DisplayPanel panel = new DisplayPanel(config);
            Main.Children.Add(panel);
            Grid.SetColumn(panel, Main.Children.Count - 1);
            Grid.SetRow(panel, 0);
        }

        public static void ExecuteAction(List<Process> processes, Dictionary<string, string> args)
        {
            int currentProcessIndex = 0;
            while (currentProcessIndex < processes.Count())
            {
                Task ta = new Task(() => processes[currentProcessIndex].execute(args));
                ta.Start();
                bool completedTask = false;
                while (!completedTask)
                {
                    Thread.Sleep(100);
                    if (processes[currentProcessIndex].isComplete(args))
                    {
                        completedTask = true;
                    }
                }
                currentProcessIndex++;
            }
        }

        public static Process getProcess( XElement config)
        {
            switch (config.Attribute("Type").Value)
            {
                case "KillProgram": return new KillProgramProcess(config);
                default: throw new Exception($"Invalid Operation Type ({config.Attribute("Type").Value})");
            }
        }

        public static string getVar(string varName, Dictionary<string, string> args)
        {
            Dictionary<string, string> totalVars = new Dictionary<string, string>();
            args.ToList().ForEach(kvp=>totalVars.Add(kvp.Key, kvp.Value));
            GlobalVars.ToList().ForEach(kvp => totalVars.Add(kvp.Key, kvp.Value));

            if (varName.StartsWith("$"))
            {
                return totalVars[varName.Substring(1)];
            }
            return varName;
        }

    }
}

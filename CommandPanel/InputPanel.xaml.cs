using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for InputPanel.xaml
    /// </summary>
    public partial class InputPanel : UserControl
    {
        private List<Process> processList;
        public InputPanel(XElement element)
        {
            InitializeComponent();

            Grid.SetColumnSpan(Main, 1);
            processList = element.Element("Processes").Elements("Process").Select(x=>MainWindow.getProcess(x)).ToList();
            List<XElement> inputElements = element.Element("Inputs").Elements("Input").ToList();
            Grid.SetRowSpan(Main, inputElements.Count);
            addInputElements(inputElements);
        }

        private void addInputElements(List<XElement> inputElements)
        {
            int i = 0;
            foreach(XElement e in inputElements)
            {
                Main.RowDefinitions.Add(new RowDefinition());
                FrameworkElement component = new Canvas();
                if (e.Attribute("Type").Value.Equals("Button"))
                {
                    Button b = new Button();
                    b.Content = e.Attribute("Name").Value.ToString();
                    
                    b.Click += (obj, eventargs) => {
                        Dictionary<string, string> args = new Dictionary<string, string>();
                        args.Add("Sender", e.Attribute("Name").Value);
                        addInputsToArgs(args);
                        MainWindow.ExecuteAction(processList, args);
                    };
                    component = b;
                }
                else if (e.Attribute("Type").Value.Equals("String"))
                {
                    TextBox ta = new TextBox();
                    component = ta;
                }
                component.Tag = e;
                component.VerticalAlignment = VerticalAlignment.Stretch;
                component.HorizontalAlignment = HorizontalAlignment.Stretch;
                Main.Children.Add(component);
                Grid.SetColumn(component, 0);
                Grid.SetRow(component, i);
                i++;
            }
        }

        private void addInputsToArgs(Dictionary<string, string> args)
        {
            foreach(FrameworkElement input in Main.Children)
            {
                XElement inputConfig = input.Tag as XElement;
                if (inputConfig != null)
                {
                    if (inputConfig.Attribute("Type").Value.Equals("String"))
                    {
                        args.Add(inputConfig.Attribute("Name").Value, ((TextBox)input).Text);
                    }
                }
            }
        }
    }
}

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
    /// Interaction logic for DisplayPanel.xaml
    /// </summary>s
    public partial class DisplayPanel : UserControl
    {
        string TEST_STRING = "Data Source=.;Initial Catalog=MTG;Integrated Security=True;User ID=TestUser;Password=TestUser";
        string displayType = string.Empty;

        SQLSelectDataRetreiver dataRetreiver;
        public DisplayPanel(XElement config)
        {
            InitializeComponent();
            displayType = config.Element("DisplayType").Value;
            dataRetreiver = new SQLSelectDataRetreiver(config.Element("DataRetreiver"));
        }

        public void refreshData(Dictionary<string, string> args)
        {
            Main.Children.Clear();
            dataRetreiver.applyDataRetrevier(Main, args);
        }
    }
}

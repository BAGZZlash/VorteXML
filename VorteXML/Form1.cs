using System;
using System.Windows.Forms;

namespace VorteXML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 8<---------------------------------------------------------------------------------------------

            //System.Xml.Linq.XNamespace elementNS = "Element";

            //var MyQuery = from c in MyXML.Root.Descendants(elementNS + "Input")
            //              select c.Element("InputTypes").Elements();

            //foreach (System.Xml.Linq.XElement MyData in MyQuery.First())
            //{
            //    MessageBox.Show(MyData.Name.ToString());
            //}

            // 8<---------------------------------------------------------------------------------------------
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VorteXML MyVorteXML = new VorteXML("C:\\Temp\\Cloud\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");
            //VorteXML MyVorteXML = new VorteXML("C:\\Users\\ccroo\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");

            System.Diagnostics.Debug.WriteLine ("Number of rows: " + MyVorteXML.ToolRows.Length.ToString());
        }
    }
}

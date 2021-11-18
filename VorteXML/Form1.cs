using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //System.Xml.Linq.XDocument MyXML = System.Xml.Linq.XDocument.Load("C:\\Temp\\Cloud\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");
            System.Xml.Linq.XDocument MyXML = System.Xml.Linq.XDocument.Load("C:\\Users\\ccroo\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");

            System.Xml.Linq.XNamespace elementNS = "Element";

            var MyQuery = from c in MyXML.Root.Descendants(elementNS + "Input")

            select c.Element("InputTypes").Elements();

            foreach (System.Xml.Linq.XElement MyData in MyQuery.First())
            {
                MessageBox.Show(MyData.Name.ToString());
            }
        }
    }
}

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
            System.Xml.Linq.XDocument MyXML = System.Xml.Linq.XDocument.Load("C:\\Temp\\Cloud\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");

            var MyQuery = from c in MyXML.Root.Descendants("Element")
                        //where (int)c.Attribute("rowNr") < 4
                        select c.Element("InputTypes").Value;

            foreach (string MyData in MyQuery)
            {
                MessageBox.Show("Data: {0}", MyData);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Xml.XmlDocument MyDoc = new System.Xml.XmlDocument();
            MyDoc.Load("C:\\Temp\\Cloud\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");

            System.Xml.XmlNode MyNode = MyDoc.DocumentElement.SelectSingleNode("NodeElements/Element");

            foreach (System.Xml.XmlNode AnotherNode in MyDoc.DocumentElement.ChildNodes)
            {
                string MyText = AnotherNode.InnerText;
                MessageBox.Show(MyText);
            }

        }
    }
}

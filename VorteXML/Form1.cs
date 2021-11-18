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

        private void HandleContent(System.Xml.Linq.XNode ThisNode)
        {
            if (ThisNode.Parent.Name.NamespaceName == "Element" & ThisNode.Parent.Name.LocalName == "Input")
            {
                if (ThisNode.Name == "InputTypes") // ThisNode hat angeblich kein Feld "Name", obwohl unten bei Locals eins angezeigt wird... :-/
                {

                }
            }

            if (ThisNode.Parent.Name.NamespaceName == "Element" & ThisNode.Parent.Name.LocalName == "Output")
            {

            }

            if (ThisNode.Parent.Name.NamespaceName == "Element" & ThisNode.Parent.Name.LocalName == "Control")
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string NodeStyle;
            string EditorVersion;
            string NodeTitle;
            System.Xml.Linq.XElement FirstNode = null;
            System.Xml.Linq.XNode CurrentNode = null;

            //System.Xml.Linq.XDocument MyXML = System.Xml.Linq.XDocument.Load("C:\\Temp\\Cloud\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");
            System.Xml.Linq.XDocument MyXML = System.Xml.Linq.XDocument.Load("C:\\Users\\ccroo\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");

            if (!MyXML.Root.FirstNode.ToString().ToLower().Contains("editorversion"))
            {
                throw new Exception("No VorteXML format detected.");
            }

            var RootDescendants = MyXML.Root.Descendants();

            foreach(var nodes in RootDescendants)
            {
                if (nodes.Name == "Node")
                {
                    foreach(var attribs in nodes.Attributes())
                    {
                        if (attribs.Name == "style") NodeStyle = attribs.Value;
                        if (attribs.Name == "editorVersion") EditorVersion = attribs.Value;
                    }
                }

                if (nodes.Name == "NodeTitle") NodeTitle = nodes.Value;

                if (nodes.Name.NamespaceName == "Element")
                {
                    foreach (var attribs in nodes.Attributes())
                    {
                        if (attribs.Name == "rowNr")
                        {
                            if (Convert.ToInt32(attribs.Value) == 1)
                            {
                                FirstNode = nodes;
                                break;
                            }
                        }
                    }
                }
                if (FirstNode != null) break;
            }

            foreach (var Children in FirstNode.Nodes())
            {
                // Iterate through the first row's contents.

                HandleContent(Children);
            }
            CurrentNode = FirstNode.NextNode;

            while (CurrentNode != null)
            {
                // Iterate through the following rows.
                foreach (var Children in FirstNode.Nodes())
                {
                    // Iterate through the following rows' contents.

                    HandleContent(Children);
                }
                CurrentNode = CurrentNode.NextNode;
            }

            // 8<---------------------------------------------------------------------------------------------

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

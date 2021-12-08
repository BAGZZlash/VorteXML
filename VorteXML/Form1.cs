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

        private void HandleContent(System.Xml.Linq.XNode ThisNode, int Counter)
        {
            System.Xml.Linq.XElement ThisElement = (System.Xml.Linq.XElement)ThisNode;
            System.Xml.Linq.XElement SubElement;
            System.Xml.Linq.XElement SubElement2;
            int SubCounter;
            int SubCounter2;

            if (ThisElement.Parent.Name.NamespaceName == "Element" & ThisElement.Parent.Name.LocalName == "Input")
            {
                if (ThisElement.Name == "InputTypes")
                {
                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;
                        System.Windows.Forms.MessageBox.Show("Row " + (Counter + 1).ToString() + ": Input" + " - " + ThisElement.Parent.LastAttribute + ". Type " + (SubCounter + 1).ToString() + ": " + SubElement.Name + ".");
                        SubCounter++;
                    }
                }

                if (ThisElement.Name == "AlternateControl")
                {
                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;
                        System.Windows.Forms.MessageBox.Show("Row " + (Counter + 1).ToString() + ": AlternateControl. Type " + (SubCounter + 1).ToString() + ": " + SubElement.Name + ".");

                        SubCounter2 = 0;
                        foreach(var nodes2 in SubElement.Nodes())
                        {
                            SubElement2 = (System.Xml.Linq.XElement)nodes2;
                            System.Windows.Forms.MessageBox.Show((SubCounter2 + 1).ToString() + ", type: " + SubElement2.Name + ", value name: '" + SubElement2.Value + "', attribute: " + SubElement2.FirstAttribute + ".");
                            SubCounter2++;
                        }

                        SubCounter++;
                    }
                }
            }

            if (ThisElement.Parent.Name.NamespaceName == "Element" & ThisElement.Parent.Name.LocalName == "Output")
            {
                if (ThisElement.Name == "OutputTypes")
                {
                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;
                        System.Windows.Forms.MessageBox.Show("Row " + (Counter + 1).ToString() + ": Output" + " - " + ThisElement.Parent.LastAttribute + ". Type " + (SubCounter + 1).ToString() + ": " + SubElement.Name + ".");
                        SubCounter++;
                    }
                }
            }

            if (ThisElement.Parent.Name.NamespaceName == "Element" & ThisElement.Parent.Name.LocalName == "Control")
            {
                System.Windows.Forms.MessageBox.Show("Row " + (Counter + 1).ToString() + ": Control. Type " + ThisElement.Name + ", " + ThisElement.Parent.LastAttribute + ".");

                if (ThisElement.Name == "{Control}Slider")
                {
                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;
                        System.Windows.Forms.MessageBox.Show("Slider property " + (SubCounter + 1).ToString() + ": " + SubElement.Value + ", attribute: " + SubElement.FirstAttribute + ".");
                        SubCounter++;
                    }
                }

                if (ThisElement.Name == "{Control}Checkbox")
                {
                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;
                        System.Windows.Forms.MessageBox.Show("Row " + (Counter + 1).ToString() + ": Checkbox. Type " + (SubCounter + 1).ToString() + ": " + SubElement.Name + ".");

                        SubCounter2 = 0;
                        foreach (var nodes2 in SubElement.Nodes())
                        {
                            SubElement2 = (System.Xml.Linq.XElement)nodes2;
                            System.Windows.Forms.MessageBox.Show((SubCounter2 + 1).ToString() + ", type: " + SubElement2.Name + ", reference to: " + SubElement2.FirstAttribute + ".");
                            SubCounter2++;
                        }

                        SubCounter++;
                    }
                }

                if (ThisElement.Name == "{Control}Dropdown")
                {
                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;
                        System.Windows.Forms.MessageBox.Show("Dropdown property " + (SubCounter + 1).ToString() + ": " + SubElement.FirstAttribute + ".");
                        SubCounter++;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string NodeStyle;
            string EditorVersion;
            string NodeTitle;
            int Counter = 0;
            System.Xml.Linq.XElement FirstNode = null;
            System.Xml.Linq.XNode CurrentNode = null;

            System.Xml.Linq.XDocument MyXML = System.Xml.Linq.XDocument.Load("C:\\Temp\\Cloud\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");
            //System.Xml.Linq.XDocument MyXML = System.Xml.Linq.XDocument.Load("C:\\Users\\ccroo\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");

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

                HandleContent(Children, Counter);
            }
            CurrentNode = FirstNode.NextNode;
            Counter++;

            while (CurrentNode != null)
            {
                // Iterate through the following rows.
                foreach (var Children in ((System.Xml.Linq.XElement)CurrentNode).Nodes())
                {
                    // Iterate through the following rows' contents.

                    HandleContent(Children, Counter);
                }
                CurrentNode = CurrentNode.NextNode;
                Counter++;
            }

            // 8<---------------------------------------------------------------------------------------------

            //System.Xml.Linq.XNamespace elementNS = "Element";

            //var MyQuery = from c in MyXML.Root.Descendants(elementNS + "Input")
            //              select c.Element("InputTypes").Elements();

            //foreach (System.Xml.Linq.XElement MyData in MyQuery.First())
            //{
            //    MessageBox.Show(MyData.Name.ToString());
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //System.Xml.Linq.XDocument MyXML = System.Xml.Linq.XDocument.Load("C:\\Temp\\Cloud\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");
            System.Xml.Linq.XDocument MyXML = System.Xml.Linq.XDocument.Load("C:\\Users\\ccroo\\ownCloud\\WFLO\\Vortex\\Node-Beschreibungs-Theorie\\VorteXML.xml");

            VorteXML MyVorteXML = new VorteXML(MyXML);
        }
    }
}

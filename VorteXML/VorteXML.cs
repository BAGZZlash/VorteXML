using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorteXML
{
    class VorteXML
    {
        private string NodeStyle;
        private string EditorVersion;
        private string NodeTitle;

        public enum RowType
        {
            Input,
            Output,
            Control

            //...?
        }

        public enum ControlType
        {
            Slider,
            Checkbox,
            Dropdown,
            Textbox

            //...
        }

        public enum ConnectorType
        {
            // Vector types
            VectorPoint,
            VectorLine,
            VectorPolygon

            // e.g. raster types
            // ...
        }

        public struct Slider
        {
            public string Style;
            public float Start;
            public float End;
        }

        public struct Checkbox
        {
            public string Style;
            public int Reference;
        }

        public struct Dropdown
        {
            public string Style;
            public string[] Values;
        }

        public struct Textbox
        {
            public string Default;
            public string Name;
        }

        public struct AltControls
        {
            public ConnectorType inputType;
            public Textbox[] textboxes;
        }

        public struct InputRow
        {
            public ConnectorType[] inputTypes;
            public AltControls[] altControls;
        }

        public struct OutputRow
        {
            public ConnectorType[] outputTypes;
        }

        public struct ControlRow
        {
            public ConnectorType[] outputTypes;
            public Slider slider;
            public Checkbox checkbox;
            public Dropdown dropdown;
            public Textbox textbox;
        }

        public struct ToolRow
        {
            public RowType rowType;
            public string Name;
            public InputRow inputRow; // Beim Instanziieren die Member dimensionieren.
            public OutputRow outputRow;
            public ControlRow controlRow;
        }

        public ToolRow[] ToolRows;

        //-----------------------------------------------------------------------------------------------------

        public VorteXML(System.Xml.Linq.XDocument MyXML)
        {
            int Counter = 0;
            int NumRows = -1;
            System.Xml.Linq.XElement FirstNode = null;
            System.Xml.Linq.XNode CurrentNode = null;

            if (!MyXML.Root.FirstNode.ToString().ToLower().Contains("editorversion"))
            {
                throw new Exception("No VorteXML format detected.");
            }

            var RootDescendants = MyXML.Root.Descendants();

            foreach (var nodes in RootDescendants)
            {
                if (nodes.Name == "Node")
                {
                    foreach (var attribs in nodes.Attributes())
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
                            NumRows = Convert.ToInt32(attribs.Value);

                            if (NumRows == 1)
                            {
                                FirstNode = nodes;
                                break;
                            }
                        }
                    }
                }
                //if (FirstNode != null) break;
            }

            if (NumRows == -1) throw new Exception("No rows present.");

            ToolRows = new ToolRow[NumRows];

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
        }

        //-----------------------------------------------------------------------------------------------------

        private void HandleContent(System.Xml.Linq.XNode ThisNode, int Counter)
        {
            System.Xml.Linq.XElement ThisElement = (System.Xml.Linq.XElement)ThisNode;
            System.Xml.Linq.XElement SubElement;
            System.Xml.Linq.XElement SubElement2;
            int SubCounter;
            int SubCounter2;

            if (ThisElement.Parent.Name.NamespaceName == "Element" & ThisElement.Parent.Name.LocalName == "Input")
            {
                ToolRows[Counter].rowType = RowType.Input;

                if (ThisElement.Name == "InputTypes")
                {
                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubCounter++;
                    }

                    ToolRows[Counter].inputRow.inputTypes = new ConnectorType[SubCounter];
                    foreach (var attribs in ThisElement.Parent.Attributes())
                    {
                        if (attribs.Name == "name")
                        {
                            ToolRows[Counter].Name = attribs.Value;
                            break;
                        }
                    }

                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;

                        if (SubElement.Name == "{Vector}Point") ToolRows[Counter].inputRow.inputTypes[SubCounter] = ConnectorType.VectorPoint;
                        if (SubElement.Name == "{Vector}Line") ToolRows[Counter].inputRow.inputTypes[SubCounter] = ConnectorType.VectorLine;
                        if (SubElement.Name == "{Vector}Polygon") ToolRows[Counter].inputRow.inputTypes[SubCounter] = ConnectorType.VectorPolygon;

                        SubCounter++;
                    }
                }

                if (ThisElement.Name == "AlternateControl")
                {
                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubCounter++;
                    }

                    ToolRows[Counter].inputRow.altControls = new AltControls[SubCounter];
                    foreach (var attribs in ThisElement.Parent.Attributes())
                    {
                        if (attribs.Name == "name")
                        {
                            ToolRows[Counter].Name = attribs.Value;
                            break;
                        }
                    }

                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;

                        if (SubElement.Name == "{Vector}Point") ToolRows[Counter].inputRow.altControls[SubCounter].inputType = ConnectorType.VectorPoint;
                        if (SubElement.Name == "{Vector}Line") ToolRows[Counter].inputRow.altControls[SubCounter].inputType = ConnectorType.VectorLine;
                        if (SubElement.Name == "{Vector}Polygon") ToolRows[Counter].inputRow.altControls[SubCounter].inputType = ConnectorType.VectorPolygon;

                        SubCounter2 = 0;
                        foreach (var nodes2 in SubElement.Nodes())
                        {
                            SubCounter2++;
                        }
                        ToolRows[Counter].inputRow.altControls[SubCounter].textboxes = new Textbox[SubCounter2];

                        SubCounter2 = 0;
                        foreach (var nodes2 in SubElement.Nodes())
                        {
                            SubElement2 = (System.Xml.Linq.XElement)nodes2;

                            ToolRows[Counter].inputRow.altControls[SubCounter].textboxes[SubCounter2].Name = SubElement2.Value;

                            foreach (var attribs in SubElement2.Attributes())
                            {
                                if (attribs.Name == "default")
                                {
                                    ToolRows[Counter].inputRow.altControls[SubCounter].textboxes[SubCounter2].Default = attribs.Value;
                                    break;
                                }
                            }
                            
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
    }
}

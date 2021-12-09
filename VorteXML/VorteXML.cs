namespace VorteXML
{
    class VorteXML
    {
        private string IntNodeStyle;
        private string IntEditorVersion;
        private string IntNodeTitle;
        
        public ToolRow[] ToolRows;

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

            // Add primitive types such as int, float, string, ...? E.g. for controls.
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
            public string Style;
            public string Name;
            public string Value;
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
            public ControlType controlType;
            public Slider slider;
            public Checkbox checkbox;
            public Dropdown dropdown;
            public Textbox textbox;
        }

        public struct ToolRow
        {
            public int Index;
            public string Name;
            public RowType rowType;
            public InputRow inputRow;
            public OutputRow outputRow;
            public ControlRow controlRow;
        }

        //-----------------------------------------------------------------------------------------------------
        // Getters and setters

        public string NodeStyle
        {
            get { return (IntNodeStyle); }
            set { NodeStyle = value; }
        }

        public string EditorVersion
        {
            get { return (IntEditorVersion); }
            set { IntEditorVersion = value; }
        }

        public string NodeTitle
        {
            get { return (IntNodeTitle); }
            set { IntNodeTitle = value; }
        }

        //-----------------------------------------------------------------------------------------------------
        // Constructors

        public VorteXML()
        {
            // Nothing useful to do here...
        }

        public VorteXML(string FileName)
        {
            if (!System.IO.File.Exists(FileName)) throw new System.Exception("File not found.");

            System.Xml.Linq.XDocument MyXML = System.Xml.Linq.XDocument.Load(FileName);
            ImportXML(MyXML);
        }

        public VorteXML(System.Xml.Linq.XDocument MyXML)
        {
            ImportXML(MyXML);
        }

        //-----------------------------------------------------------------------------------------------------
        // Private methods

        private void ImportXML(System.Xml.Linq.XDocument MyXML)
        {
            int Counter = 0;
            int NumRows = -1;
            System.Xml.Linq.XElement FirstNode = null;
            System.Xml.Linq.XNode CurrentNode = null;

            if (!MyXML.Root.FirstNode.ToString().ToLower().Contains("editorversion"))
            {
                throw new System.Exception("No VorteXML format detected.");
            }

            var RootDescendants = MyXML.Root.Descendants();

            foreach (var nodes in RootDescendants)
            {
                if (nodes.Name == "Node")
                {
                    foreach (var attribs in nodes.Attributes())
                    {
                        if (attribs.Name == "style") IntNodeStyle = attribs.Value;
                        if (attribs.Name == "editorVersion") IntEditorVersion = attribs.Value;
                    }
                }

                if (nodes.Name == "NodeTitle") IntNodeTitle = nodes.Value;

                if (nodes.Name.NamespaceName == "Element")
                {
                    foreach (var attribs in nodes.Attributes())
                    {
                        if (attribs.Name == "rowNr")
                        {
                            NumRows = System.Convert.ToInt32(attribs.Value);

                            if (NumRows == 1)
                            {
                                FirstNode = nodes;
                                break;
                            }
                        }
                    }
                }
            }

            if (NumRows == -1) throw new System.Exception("No rows present.");

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
                ToolRows[Counter].Index = Counter;

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
                        //...

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
                        //...

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
                                    ToolRows[Counter].inputRow.altControls[SubCounter].textboxes[SubCounter2].Value = attribs.Value;
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
                ToolRows[Counter].rowType = RowType.Output;
                ToolRows[Counter].Index = Counter;

                if (ThisElement.Name == "OutputTypes")
                {
                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubCounter++;
                    }

                    ToolRows[Counter].outputRow.outputTypes = new ConnectorType[SubCounter];
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

                        if (SubElement.Name == "{Vector}Point") ToolRows[Counter].outputRow.outputTypes[SubCounter] = ConnectorType.VectorPoint;
                        if (SubElement.Name == "{Vector}Line") ToolRows[Counter].outputRow.outputTypes[SubCounter] = ConnectorType.VectorLine;
                        if (SubElement.Name == "{Vector}Polygon") ToolRows[Counter].outputRow.outputTypes[SubCounter] = ConnectorType.VectorPolygon;
                        //...

                        SubCounter++;
                    }
                }
            }

            if (ThisElement.Parent.Name.NamespaceName == "Element" & ThisElement.Parent.Name.LocalName == "Control")
            {
                ToolRows[Counter].rowType = RowType.Control;
                ToolRows[Counter].Index = Counter;

                foreach (var attribs in ThisElement.Parent.Attributes())
                {
                    if (attribs.Name == "name")
                    {
                        ToolRows[Counter].Name = attribs.Value;
                        break;
                    }
                }

                if (ThisElement.Name == "{Control}Slider") ToolRows[Counter].controlRow.controlType = ControlType.Slider;
                if (ThisElement.Name == "{Control}Checkbox") ToolRows[Counter].controlRow.controlType = ControlType.Checkbox;
                if (ThisElement.Name == "{Control}Dropdown") ToolRows[Counter].controlRow.controlType = ControlType.Dropdown;
                if (ThisElement.Name == "{Control}Textbox") ToolRows[Counter].controlRow.controlType = ControlType.Textbox;
                //...

                if (ThisElement.Name == "{Control}Slider")
                {
                    foreach (var attribs in ThisElement.Attributes())
                    {
                        if (attribs.Name == "style")
                        {
                            ToolRows[Counter].controlRow.slider.Style = attribs.Value;
                            break;
                        }
                    }

                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;

                        if (SubElement.Value == "Start")
                        {
                            foreach (var attribs in SubElement.Attributes())
                            {
                                if (attribs.Name == "default")
                                {
                                    ToolRows[Counter].controlRow.slider.Start = System.Convert.ToSingle(attribs.Value, System.Globalization.CultureInfo.InvariantCulture);
                                    break;
                                }
                            }
                        }

                        if (SubElement.Value == "End")
                        {
                            foreach (var attribs in SubElement.Attributes())
                            {
                                if (attribs.Name == "default")
                                {
                                    ToolRows[Counter].controlRow.slider.End = System.Convert.ToSingle(attribs.Value, System.Globalization.CultureInfo.InvariantCulture);
                                    break;
                                }
                            }
                        }

                        SubCounter++;
                    }
                }

                if (ThisElement.Name == "{Control}Checkbox")
                {
                    foreach (var attribs in ThisElement.Attributes())
                    {
                        if (attribs.Name == "style")
                        {
                            ToolRows[Counter].controlRow.checkbox.Style = attribs.Value;
                            break;
                        }
                    }

                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;
                        if (SubElement.Name == "EnableElements")
                        {
                            SubCounter2 = 0;
                            foreach (var nodes2 in SubElement.Nodes())
                            {
                                SubElement2 = (System.Xml.Linq.XElement)nodes2;

                                foreach (var attribs in SubElement2.Attributes())
                                {
                                    if (attribs.Name == "row")
                                    {
                                        ToolRows[Counter].controlRow.checkbox.Reference = System.Convert.ToInt32(attribs.Value);
                                        break;
                                    }
                                }

                                SubCounter2++;
                            }
                        }

                        SubCounter++;
                    }
                }

                if (ThisElement.Name == "{Control}Dropdown")
                {
                    foreach (var attribs in ThisElement.Attributes())
                    {
                        if (attribs.Name == "style")
                        {
                            ToolRows[Counter].controlRow.dropdown.Style = attribs.Value;
                            break;
                        }
                    }

                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubCounter++;
                    }
                    ToolRows[Counter].controlRow.dropdown.Values = new string[SubCounter];

                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;

                        foreach (var attribs in SubElement.Attributes())
                        {
                            if (attribs.Name == "value")
                            {
                                ToolRows[Counter].controlRow.dropdown.Values[SubCounter] = attribs.Value;
                                break;
                            }
                        }

                        SubCounter++;
                    }
                }

                if (ThisElement.Name == "{Control}Textbox")
                {
                    foreach (var attribs in ThisElement.Attributes())
                    {
                        if (attribs.Name == "style")
                        {
                            ToolRows[Counter].controlRow.textbox.Style = attribs.Value;
                            break;
                        }
                    }

                    SubCounter = 0;
                    foreach (var nodes in ThisElement.Nodes())
                    {
                        SubElement = (System.Xml.Linq.XElement)nodes;

                        foreach (var attribs in SubElement.Attributes())
                        {
                            if (attribs.Name == "value")
                            {
                                ToolRows[Counter].controlRow.textbox.Value = attribs.Value;
                                break;
                            }
                        }

                        foreach (var attribs in SubElement.Attributes())
                        {
                            if (attribs.Name == "name")
                            {
                                ToolRows[Counter].controlRow.textbox.Name = attribs.Value;
                                break;
                            }
                        }

                        SubCounter++;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        // Public methods

        //...
    }
}

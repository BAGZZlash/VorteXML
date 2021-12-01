using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorteXML
{
    class VorteXML
    {
        public enum RowType
        {
            Input,
            Output,
            Control
        }

        public enum ControlType
        {
            Slider,
            Checkbox,
            Dropdown,
            Textbox
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
            string Style;
            float Start;
            float End;
        }

        public struct Checkbox
        {
            string Style;
            int Reference;
        }

        public struct Dropdown
        {
            string Style;
            string[] Values;
        }

        public struct Textbox
        {
            string Default;
        }

        public struct InputRow
        {
            ConnectorType[] inputTypes;
            Textbox[] altControls;
        }

        public struct OutputRow
        {
            ConnectorType[] outputTypes;
        }

        public struct ControlRow
        {
            ConnectorType[] outputTypes;
            Slider slider;
            Checkbox checkbox;
            Dropdown dropdown;
            Textbox textbox;
        }

        public struct ToolRow
        {
            int Index;
            RowType rowType;
            string Name;
            InputRow inputRow; // Beim Instanziieren die Member dimensionieren.
            OutputRow outputRow;
            ControlRow controlRow;
        }

        private ToolRow[] ToolRows;
    }
}

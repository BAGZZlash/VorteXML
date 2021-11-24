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

        public enum InputType
        {
            // Vector types
            VectorPoint,
            VectorLine,
            VectorPolygon

            // e.g. raster types
            // ...
        }

        public struct InputRow
        {
            InputType[] inputTypes;

        }

        public struct ToolRow
        {
            int Index;
            RowType rowType;

        }

        private ToolRow[] ToolRows;
    }
}

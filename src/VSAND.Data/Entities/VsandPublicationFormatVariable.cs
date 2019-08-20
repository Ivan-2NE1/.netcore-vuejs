using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPublicationFormatVariable
    {
        public int FormatVariableId { get; set; }
        public int PublicationId { get; set; }
        public string VariableName { get; set; }
        public string VariableValue { get; set; }
        public string ValueType { get; set; }

        public VsandPublication Publication { get; set; }
    }
}

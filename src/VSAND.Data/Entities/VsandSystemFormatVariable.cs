using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSystemFormatVariable
    {
        public int FormatVariableId { get; set; }
        public string VariableName { get; set; }
        public string VariableValue { get; set; }
        public string ValueType { get; set; }
    }
}

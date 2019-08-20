using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandSportStatFormula
    {
        public int FormulaId { get; set; }
        public int SportId { get; set; }
        public string Name { get; set; }
        public string Formula { get; set; }
        public string PreviewFormula { get; set; }
        public string Scope { get; set; }

        public VsandSport Sport { get; set; }
    }
}

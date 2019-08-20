using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.Sports
{
    public class StatBase
    {
        public string name { get; }
        public string abbreviation { get; }
        public string dataType { get; }
        public int sortOrder { get; }
        public bool enabled { get; }
        public bool calculated { get; }
    }
}

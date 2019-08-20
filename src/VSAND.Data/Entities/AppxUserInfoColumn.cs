using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxUserInfoColumn
    {
        public int UserInfoColumnId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int InputType { get; set; }
        public int SortOrder { get; set; }
        public bool Required { get; set; }
        public bool Enabled { get; set; }
        public string ValidationMessage { get; set; }
        public string ValueList { get; set; }
        public bool IsSystemField { get; set; }
    }
}

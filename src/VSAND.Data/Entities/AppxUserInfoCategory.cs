using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxUserInfoCategory
    {
        public int UserInfoCategoryId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }
    }
}

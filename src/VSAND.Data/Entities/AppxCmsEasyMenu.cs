using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxCmsEasyMenu
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string InnerHtml { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int? Level { get; set; }
        public int? SortOrder { get; set; }
        public string AllowedRoles { get; set; }
        public bool DisplayAnonymous { get; set; }
        public bool DisplayAuthenticated { get; set; }
    }
}

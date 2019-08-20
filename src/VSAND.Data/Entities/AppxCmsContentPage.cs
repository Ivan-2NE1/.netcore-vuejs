using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxCmsContentPage
    {
        public int ContentPageId { get; set; }
        public string PageRef { get; set; }
        public string PageTitle { get; set; }
        public string PageType { get; set; }
        public string MetaAbstract { get; set; }
        public string MetaAuthor { get; set; }
        public string MetaCopyright { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public string ScriptResource { get; set; }
    }
}

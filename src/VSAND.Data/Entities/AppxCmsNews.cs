using System;

namespace VSAND.Data.Entities
{
    public partial class AppxCmsNews
    {
        public int NewsId { get; set; }
        public int? NewsTypeId { get; set; }
        public string Headline { get; set; }
        public string Story { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public int CreatedBy { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Summary { get; set; }
        public bool Published { get; set; }
        public byte EnableComments { get; set; }
    }
}

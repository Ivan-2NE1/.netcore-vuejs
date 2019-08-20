namespace VSAND.Data.Entities
{
    public partial class AppxCmsNewsType
    {
        public int NewsTypeId { get; set; }
        public string NewsType { get; set; }
        public int ParentTypeId { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string Copyright { get; set; }
        public string ManagingEditor { get; set; }
        public string Webmaster { get; set; }
        public int? Ttl { get; set; }
        public string Image { get; set; }
    }
}

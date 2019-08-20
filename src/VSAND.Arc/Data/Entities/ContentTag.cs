namespace VSAND.Arc.Data.Entities
{
    public partial class ContentTag
    {
        public string ContentOperationId { get; set; }
        public string Tag { get; set; }
        public string Publication { get; set; }

        public virtual ContentOperation ContentOperation { get; set; }
    }
}

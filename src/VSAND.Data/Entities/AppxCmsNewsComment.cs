using System;

namespace VSAND.Data.Entities
{
    public partial class AppxCmsNewsComment
    {
        public int CommentId { get; set; }
        public int NewsId { get; set; }
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Url { get; set; }
        public string Ipaddress { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comment { get; set; }
        public bool SubscribeUpdates { get; set; }
    }
}

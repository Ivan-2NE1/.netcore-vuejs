using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandshimUserActivity
    {
        public string ActivityType { get; set; }
        public int ActivityId { get; set; }
        public string Description { get; set; }
        public DateTime? ActivityDate { get; set; }
        public int ActivityById { get; set; }
        public string ActivityByFirstName { get; set; }
        public string ActivityByLastName { get; set; }
    }
}

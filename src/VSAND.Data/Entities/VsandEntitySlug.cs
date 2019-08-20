using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VSAND.Data.Entities
{
    public partial class VsandEntitySlug
    {
        public VsandEntitySlug()
        {

        }

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; }
        [Required]
        [MaxLength(50)]
        public string EntityType { get; set; }
        [Required]
        public int EntityId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VSAND.Data.ViewModels
{
    public class GameReportMeta
    {
        [Required]
        public int SportGameMetaId { get; set; }
        public string MetaValue { get; set; }
    }
}

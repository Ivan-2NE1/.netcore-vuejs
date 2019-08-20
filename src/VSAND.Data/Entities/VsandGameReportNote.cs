using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReportNote
    {
        public int NoteId { get; set; }
        public int GameReportId { get; set; }
        public string Note { get; set; }
        public int NoteById { get; set; }
        public string NoteBy { get; set; }
        public DateTime? NoteDate { get; set; }

        public VsandGameReport GameReport { get; set; }
        public AppxUser NoteByNavigation { get; set; }
    }
}

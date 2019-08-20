using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPublicationStoryNote
    {
        public int NoteId { get; set; }
        public int PublicationStoryId { get; set; }
        public string Note { get; set; }
        public int NoteById { get; set; }
        public string NoteBy { get; set; }
        public DateTime? NoteDate { get; set; }

        public AppxUser NoteByNavigation { get; set; }
        public VsandPublicationStory PublicationStory { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.GameReport
{
    public class GameReportNote
    {
        public string Note { get; set; }
        public string NoteBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public GameReportNote()
        {

        }

        public GameReportNote(VsandGameReportNote note)
        {
            Note = note.Note;
            CreatedDate = note.NoteDate.Value;           
            NoteBy = note.NoteBy;
        }
    }
}

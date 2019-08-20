using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxSurveyHeader
    {
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string ResponseAction { get; set; }
        public string ResponseActionResource { get; set; }
        public string Redirect { get; set; }
        public bool? NumberQuestions { get; set; }
        public string SubmitButtonText { get; set; }
        public string PreText { get; set; }
        public string PostText { get; set; }
        public string ConfirmationText { get; set; }
    }
}

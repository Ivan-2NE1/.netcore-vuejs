using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxSurveyResponse
    {
        public int SurveyResponseId { get; set; }
        public int SurveyResponseHeaderId { get; set; }
        public int SurveyQuestionId { get; set; }
        public string Response { get; set; }
    }
}

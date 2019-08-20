using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxSurveyQuestion
    {
        public int SurveyQuestionId { get; set; }
        public string Question { get; set; }
        public int SurveyId { get; set; }
        public int? SortOrder { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public bool? RequiredField { get; set; }
        public string ValidationMessage { get; set; }
        public string ResponseOptions { get; set; }
    }
}

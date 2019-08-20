using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxSurveyResponseHeader
    {
        public int SurveyResponseHeaderId { get; set; }
        public int SurveyId { get; set; }
        public int? RespondantId { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string Ipaddress { get; set; }
        public string Ipgeocode { get; set; }
        public string Browser { get; set; }
        public string ResponseUrl { get; set; }
        public bool? Acknowledged { get; set; }
        public string AcknowledgedBy { get; set; }
        public DateTime? AcknowledgedDate { get; set; }
    }
}

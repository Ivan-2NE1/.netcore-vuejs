using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPublicationFtp
    {
        public int PublicationFtpid { get; set; }
        public int PublicationId { get; set; }
        public string Ftpusername { get; set; }
        public string Ftppassword { get; set; }
        public string Ftpurl { get; set; }
        public string Ftpformat { get; set; }

        public VsandPublication Publication { get; set; }
    }
}

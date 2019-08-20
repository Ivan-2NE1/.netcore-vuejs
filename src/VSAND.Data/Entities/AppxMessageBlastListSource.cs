using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxMessageBlastListSource
    {
        public int ListSourceId { get; set; }
        public string ListName { get; set; }
        public string DataSourceType { get; set; }
        public string ConnectionString { get; set; }
        public string ListQuery { get; set; }
    }
}

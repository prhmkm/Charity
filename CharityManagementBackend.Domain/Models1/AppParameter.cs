using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class AppParameter
    {
        public long id { get; set; }
        public string? ParamName { get; set; }
        public string? ParamValue { get; set; }
        public string? ParamDiscription { get; set; }
    }
}

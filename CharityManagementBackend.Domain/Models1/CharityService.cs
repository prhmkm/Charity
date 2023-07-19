using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class CharityService
    {
        public int TSRVCID { get; set; }
        public int SwCode { get; set; }
        public string ServiceName { get; set; } = null!;
        public string Account { get; set; } = null!;
        public string Iban { get; set; } = null!;
        public string? ContactName { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}

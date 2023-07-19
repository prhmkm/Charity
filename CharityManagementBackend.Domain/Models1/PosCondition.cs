using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class PosCondition
    {
        public string PcCode { get; set; } = null!;
        public string PccName { get; set; } = null!;
        public string PccShortName { get; set; } = null!;
        public string FaName { get; set; } = null!;
    }
}

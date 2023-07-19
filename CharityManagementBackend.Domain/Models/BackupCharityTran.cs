using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class BackupCharityTran
    {
        public string ABRNCHCOD { get; set; } = null!;
        public string TRPOSCCOD { get; set; } = null!;
        public string TRANDATE { get; set; } = null!;
        public string TRANTIME { get; set; } = null!;
        public string TRTRACENO { get; set; } = null!;
        public string TRRRN { get; set; } = null!;
        public string TRPAN { get; set; } = null!;
        public decimal TRAmount { get; set; }
        public string TSRVCID { get; set; } = null!;
        public int SWCODE { get; set; }
        public int Process { get; set; }
    }
}

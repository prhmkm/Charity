using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class Payment
    {
        public int PaymentesId { get; set; }
        public string TranDate { get; set; } = null!;
        public string TSRVCID { get; set; } = null!;
        public int SwCode { get; set; }
        public string ServiceName { get; set; } = null!;
        public string LocalPan { get; set; } = null!;
        public int TranCount { get; set; }
        public decimal SumAmounts { get; set; }
    }
}

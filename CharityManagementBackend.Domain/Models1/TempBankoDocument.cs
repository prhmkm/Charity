using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class TempBankoDocument
    {
        public string ACC { get; set; } = null!;
        public decimal Debtor { get; set; }
        public decimal Creditor { get; set; }
        public string Description { get; set; } = null!;
    }
}

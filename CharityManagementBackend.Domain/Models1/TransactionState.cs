using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class TransactionState
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
    }
}

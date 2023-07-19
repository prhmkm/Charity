using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int WishId { get; set; }
        public DateTime CreationDateTime { get; set; }
        public decimal Amount { get; set; }
        public int StateId { get; set; }
        public string? InvoiceId { get; set; }
        public string? TransactionId { get; set; }
        public string? UserName { get; set; }
    }
}

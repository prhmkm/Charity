using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class DWH_Transaction
    {
        public int Id { get; set; }
        public DateTime TransDate { get; set; }
        public int TransCount { get; set; }
        public decimal TransAmount { get; set; }
    }
}

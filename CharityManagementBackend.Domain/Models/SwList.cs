using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class SwList
    {
        public int Id { get; set; }
        public string SwTitle { get; set; } = null!;
        public bool? IsActive { get; set; }
        public DateTime CreationDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class View
    {
        public int Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
    }
}

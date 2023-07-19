using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class _User
    {
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Version { get; set; } = null!;
        public string MacAddress { get; set; } = null!;
        public int Enabled { get; set; }
        public string AccessLevel { get; set; } = null!;
    }
}

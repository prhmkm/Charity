using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string? UserName { get; set; }
        public string? PassWord { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }
        public string? NationalCode { get; set; }
        public string? BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? ImageThumb { get; set; }
        public string? Address { get; set; }
        public string? RefreshToken { get; set; }
        public bool RememberMe { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? RegisterDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

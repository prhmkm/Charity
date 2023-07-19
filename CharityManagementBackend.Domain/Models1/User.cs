using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class User
    {
        public User()
        {
            Wishes = new HashSet<Wish>();
        }

        public int Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PassWord { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public bool RememberMe { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Wish> Wishes { get; set; }
    }
}

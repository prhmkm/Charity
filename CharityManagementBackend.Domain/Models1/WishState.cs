using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class WishState
    {
        public WishState()
        {
            Wishes = new HashSet<Wish>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Wish> Wishes { get; set; }
    }
}

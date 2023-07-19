using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class Wish
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreationDateTime { get; set; }
        public int UserId { get; set; }
        public int WisherId { get; set; }
        public int StateId { get; set; }
        public int PriorityId { get; set; }
        public decimal GoalAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public string Picture { get; set; } = null!;
        public int DeadLine { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual WishPriority Priority { get; set; } = null!;
        public virtual WishState State { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual Wisher Wisher { get; set; } = null!;
    }
}

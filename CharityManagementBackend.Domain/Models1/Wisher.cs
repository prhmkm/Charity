using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class Wisher
    {
        public Wisher()
        {
            Wishes = new HashSet<Wish>();
        }

        public int Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public bool Type { get; set; }
        public string Name { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
        public int? PersonalityTypeId { get; set; }
        public int CreationUserId { get; set; }
        public string? IbanNumber { get; set; }
        public string? IbanOwner { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Wish> Wishes { get; set; }
    }
}

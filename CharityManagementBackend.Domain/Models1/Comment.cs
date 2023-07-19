using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class Comment
    {
        public Comment()
        {
            CommentPictures = new HashSet<CommentPicture>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public string? HLink { get; set; }
        public DateTime CreationDateTime { get; set; }
        public bool? Type { get; set; }
        public int StateId { get; set; }
        public int CreatorId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<CommentPicture> CommentPictures { get; set; }
    }
}

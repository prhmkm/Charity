using System;
using System.Collections.Generic;

namespace CharityManagementBackend.Domain.Models
{
    public partial class CommentPicture
    {
        public int Id { get; set; }
        public string Picture { get; set; } = null!;
        public int CommentId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Comment Comment { get; set; } = null!;
    }
}

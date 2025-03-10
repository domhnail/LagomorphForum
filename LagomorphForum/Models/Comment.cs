using LagomorphForum.Data;

namespace LagomorphForum.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public int DiscussionId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public Discussion? Discussion { get; set; }
    }
}

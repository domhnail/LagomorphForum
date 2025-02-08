
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LagomorphForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageFilename { get; set; } = string.Empty;
        [NotMapped]
        [Display(Name = "image, rabbit, hare or bunny preferred")]
        public IFormFile? ImageFile { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}

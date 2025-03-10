
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using LagomorphForum.Data;

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
        [DisplayName("Date Created")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}

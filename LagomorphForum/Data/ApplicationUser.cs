using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LagomorphForum.Models;
using Microsoft.AspNetCore.Identity;

namespace LagomorphForum.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [StringLength(40, ErrorMessage = "Name cannot be longer than 40 characters.")]
        public string Name { get; set; } = string.Empty;
        [PersonalData]
        [StringLength(250, ErrorMessage = "Location cannot be longer than 250 characters.")]
        public string Location { get; set; } = string.Empty;
        [PersonalData]
        [StringLength(255)] public string? ImageFilename { get; set; } = string.Empty;

        public List<Discussion> Discussions { get; set; } = new List<Discussion>();
    }
}

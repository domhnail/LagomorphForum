using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LagomorphForum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LagomorphForum.Data
{
    public class LagomorphForumContext : IdentityDbContext
    {
        public LagomorphForumContext (DbContextOptions<LagomorphForumContext> options)
            : base(options)
        {
        }

        public DbSet<LagomorphForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<LagomorphForum.Models.Comment> Comment { get; set; } = default!;
    }
}

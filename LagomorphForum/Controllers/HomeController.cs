using System.Diagnostics;
using LagomorphForum.Data;
using LagomorphForum.Migrations;
using LagomorphForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagomorphForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly LagomorphForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(LagomorphForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussion
                .Include(d => d.User)
                .Include(d => d.Comments)
                .OrderByDescending(d => d.CreateDate)
                .ToListAsync();
            return View(discussions);
        }

        public async Task<IActionResult> GetDiscussion(int ?id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var discussion = await _context.Discussion
                .Include(d => d.Comments)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Discussions)
                .ThenInclude(d => d.Comments)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user == null ? NotFound() : View(user);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

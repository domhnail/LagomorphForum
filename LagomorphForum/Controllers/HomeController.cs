using System.Diagnostics;
using LagomorphForum.Data;
using LagomorphForum.Migrations;
using LagomorphForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LagomorphForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly LagomorphForumContext _context;

        public HomeController(LagomorphForumContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var discussions = _context.Discussion
                .Include(d => d.Comments)
                .OrderByDescending(d => d.CreateDate);
            return View(await discussions.ToListAsync());
        }

        public async Task<IActionResult> GetDiscussion(int ?id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var discussion = await _context.Discussion
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
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

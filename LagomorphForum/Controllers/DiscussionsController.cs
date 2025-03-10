using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LagomorphForum.Data;
using LagomorphForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LagomorphForum.Controllers
{
    [Authorize]
    public class DiscussionsController : Controller
    {
        private readonly LagomorphForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DiscussionsController(LagomorphForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Discussions
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var discussions = await _context.Discussion
                .Where(m => m.UserId == userId)
                .ToListAsync();

            return View(discussions);
        }

        // GET: Discussions/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content,ImageFilename,CreateDate,ImageFile")] Discussion discussion)
        {
            
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (userId != null)
                {
                    discussion.UserId = userId;
                }

                discussion.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile?.FileName);

                discussion.CreateDate = DateTime.Now;
                if (discussion.ImageFile != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", discussion.ImageFilename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await discussion.ImageFile.CopyToAsync(fileStream);
                    }
                }
                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetDiscussion", "Home", new { id = discussion.DiscussionId });
            }
            return View(discussion);
        }

        // GET: Discussions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            var discussion = await _context.Discussion
                .Where(m => m.UserId == userId)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }
            return View(discussion);
        }

        // POST: Discussions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFilename,CreateDate")] Discussion discussion)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

            var existingDiscussion = await _context.Discussion.FindAsync(id);
            if (existingDiscussion == null)
            {
                return NotFound();
            }
            var currentUserId = _userManager.GetUserId(User);
            if (existingDiscussion.UserId != currentUserId)
            {
                return Forbid();
            }
            

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (userId != null)
                { 
                    discussion.UserId = existingDiscussion.UserId; //preserve original creator
                }

                //update properties
                existingDiscussion.Title = discussion.Title;
                existingDiscussion.Content = discussion.Content;
                existingDiscussion.ImageFilename = discussion.ImageFilename;
                existingDiscussion.CreateDate = discussion.CreateDate;

                try
                {
                    _context.Update(existingDiscussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.DiscussionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid!");
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Trace.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
                return View(discussion);
            }
            return View(discussion);
        }

        // GET: Discussions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            var discussion = await _context.Discussion
                .Where(m => m.UserId == userId)
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion != null)
            {
                _context.Discussion.Remove(discussion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussion.Any(e => e.DiscussionId == id);
        }
    }
}

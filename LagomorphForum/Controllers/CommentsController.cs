﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LagomorphForum.Data;
using LagomorphForum.Models;

namespace LagomorphForum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly LagomorphForumContext _context;

        public CommentsController(LagomorphForumContext context)
        {
            _context = context;
        }

        //// GET: Comments
        //public async Task<IActionResult> Index()
        //{
        //    var lagomorphForumContext = _context.Comment.Include(c => c.Discussion);
        //    return View(await lagomorphForumContext.ToListAsync());
        //}

        // GET: Comments/Create
        public IActionResult Create(int id)
        {
            return View(new Comment { DiscussionId = id });
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreateDate = DateTime.Now;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
            }
            return View(comment);
        }


        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.CommentId == id);
        }
    }
}

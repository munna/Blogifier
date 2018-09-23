﻿using Core.Data;
using Core.Helpers;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace App.Pages.Admin.Posts
{
    public class IndexModel : AdminPageModel
    {
        [BindProperty]
        public IEnumerable<PostItem> Posts { get; set; }
        public Pager Pager { get; set; }

        IDataService _db;
        INotificationService _ns;

        public IndexModel(IDataService db, INotificationService ns)
        {
            _db = db;
            _ns = ns;
            Pager = new Pager(1);
        }

        public async Task<IActionResult> OnGetAsync(int pg = 1, string status = "A")
        {
            var author = await _db.Authors.GetItem(a => a.AppUserName == User.Identity.Name);
            IsAdmin = author.IsAdmin;

            Expression<Func<BlogPost, bool>> predicate = p => p.Id > 0;
            Pager = new Pager(pg);

            if (IsAdmin)
            {
                if(status == "P")
                    predicate = p => p.Published > DateTime.MinValue;
                else if(status == "D")
                    predicate = p => p.Published == DateTime.MinValue;
            }
            else
            {
                if (status == "P")
                    predicate = p => p.Published > DateTime.MinValue && p.AuthorId == author.Id;
                else if (status == "D")
                    predicate = p => p.Published == DateTime.MinValue && p.AuthorId == author.Id;
                else
                    predicate = p => p.AuthorId == author.Id;
            }

            Posts = await _db.BlogPosts.GetList(predicate, Pager);

            Notifications = await _ns.GetNotifications(author.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var author = await _db.Authors.GetItem(a => a.AppUserName == User.Identity.Name);
            IsAdmin = author.IsAdmin;

            var page = int.Parse(Request.Form["page"]);
            var term = Request.Form["search"];

            Pager = new Pager(page);

            if(IsAdmin)
                Posts = await _db.BlogPosts.Search(Pager, term);
            else
                Posts = await _db.BlogPosts.Search(Pager, term, author.Id);

            return Page();
        }
    }
}
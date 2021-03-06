﻿using System.Linq;
using System.Web.Mvc;
using SitecoreLibrary.BAL.Contracts;
using SitecoreLibrary.BAL.Services;

namespace SitecoreLibrary.Controllers
{
    public class BookHistoryController : Controller
    {
        private readonly IBookHistoryService bookHistoryService;

        public BookHistoryController(IBookHistoryService bookHistoryService)
        {
            this.bookHistoryService = bookHistoryService;
        }
        
        // GET: BookHistory
        public ActionResult Index(int bookId)
        {
            ModelState.Clear();
            var booksHistory = bookHistoryService.GetBooksHistory().Where(b=>b.BookId == bookId).ToList();
            return View(booksHistory);
        }
    }
}
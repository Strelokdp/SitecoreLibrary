using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SitecoreLibrary.Repository;

namespace SitecoreLibrary.Controllers
{
    public class BookHistoryController : Controller
    {
        private BookHistoryRepository _bookHistoryRepo = new BookHistoryRepository();
        
        // GET: BookHistory
        public ActionResult Index()
        {
            ModelState.Clear();
            var booksHistory = _bookHistoryRepo.GetAllBooksHistory();
            return View(booksHistory);
        }
    }
}
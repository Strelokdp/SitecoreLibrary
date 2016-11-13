using System.Linq;
using System.Web.Mvc;
using SitecoreLibrary.BAL.Services;
using SitecoreLibrary.DAL.Contracts;
using SitecoreLibrary.DAL.Repository;

namespace SitecoreLibrary.Controllers
{
    public class BookHistoryController : Controller
    {
        private readonly BookHistoryService _bookHistoryService = new BookHistoryService();
        
        // GET: BookHistory
        public ActionResult Index(int bookId)
        {
            ModelState.Clear();
            var booksHistory = _bookHistoryService.GetBooksHistory().Where(b=>b.BookId == bookId).ToList();
            return View(booksHistory);
        }
    }
}
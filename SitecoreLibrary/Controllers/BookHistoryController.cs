using System.Linq;
using System.Web.Mvc;
using SitecoreLibrary.Contracts;
using SitecoreLibrary.Repository;

namespace SitecoreLibrary.Controllers
{
    public class BookHistoryController : Controller
    {
        private readonly IBookHistoryRepository _bookHistoryRepo = new BookHistoryRepository();
        
        // GET: BookHistory
        public ActionResult Index(int bookId)
        {
            ModelState.Clear();
            var booksHistory = _bookHistoryRepo.GetBooksHistory().Where(b=>b.BookId == bookId).ToList();
            return View(booksHistory);
        }
    }
}
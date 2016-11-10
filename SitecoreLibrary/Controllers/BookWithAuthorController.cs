using SitecoreLibrary.Models;
using SitecoreLibrary.Repository;
using System.Web.Mvc;

namespace SitecoreLibrary.Controllers
{
    public class BookWithAuthorController : Controller
    {
        // GET: Book/GetAllBookAuthorDetails
        public ActionResult GetAllBookAuthorDetails()
        {
            BookWithAuthorRepository bookAuthRepo = new BookWithAuthorRepository();
            ModelState.Clear();
            return View(bookAuthRepo.GetAllBooksWithAuthors());
        }
    }
}

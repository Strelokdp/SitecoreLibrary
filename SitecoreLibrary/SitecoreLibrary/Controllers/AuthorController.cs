using SitecoreLibrary.Models;
using SitecoreLibrary.Repository;
using System.Web.Mvc;

namespace SitecoreLibrary.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Author/GetAllAuthorDetails
        public ActionResult GetAllAuthorDetails()
        {
            AuthorRepository authorRepo = new AuthorRepository();
            ModelState.Clear();
            return View(authorRepo.GetAllAuthors());
        }

        // GET: Author/AddAuthor
        public ActionResult AddAuthor()
        {
            return View();
        }

        // POST: Author/AddAuthor
        [HttpPost]
        public ActionResult AddAuthor(Author author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AuthorRepository AuthorRepo = new AuthorRepository();

                    if (AuthorRepo.AddAuthor(author))
                    {
                        ViewBag.Message = "Author details added successfully";
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Author/EditAuthorDetails/5
        public ActionResult EditAuthorDetails(int id)
        {
            AuthorRepository AuthorRepo = new AuthorRepository();
            return View(AuthorRepo.GetAllAuthors().Find(author => author.Authorid == id));
        }

        // POST: Author/EditAuthorDetails/5
        [HttpPost]
        public ActionResult EditAuthorDetails(int id, Author obj)
        {
            try
            {
                AuthorRepository authorRepo = new AuthorRepository();
                authorRepo.UpdateAuthor(obj);
                return RedirectToAction("GetAllAuthorDetails");
            }
            catch
            {
                return View();
            }
        }

        // GET: Author/DeleteAuthor/5
        public ActionResult DeleteAuthor(int id)
        {
            try
            {
                AuthorRepository authorRepo = new AuthorRepository();
                if (authorRepo.DeleteAuthor(id))
                {
                    ViewBag.AlertMsg = "Author details deleted successfully";

                }
                return RedirectToAction("GetAllAuthorDetails");

            }
            catch
            {
                return View();
            }
        }
    }
}

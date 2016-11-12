using System;
using SitecoreLibrary.Models;
using SitecoreLibrary.Repository;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace SitecoreLibrary.Controllers
{
    public class BookWithAuthorController : Controller
    {
        // GET: Book/GetAllBookAuthorDetails
        public ActionResult GetAllBookAuthorDetails(string SelectList)
        {
            BookWithAuthorRepository bookAuthRepo = new BookWithAuthorRepository();
            ModelState.Clear();

            var bookFilterList = new List<string> {"All books", "Available books", "Taken books"} as IEnumerable<string>;

            ViewBag.SelectList = new SelectList(bookFilterList);

            var bookList = bookAuthRepo.GetAllBooksWithAuthors();

            
                switch (SelectList)
                {
                    case ("All books"):
                        bookList = bookAuthRepo.GetAllBooksWithAuthors();
                        break;

                    case ("Available books"):
                        bookList = bookAuthRepo.GetAllBooksWithAuthors().Where(x => !x.IsTaken).ToList();
                        break;

                    case ("Taken books"):
                        bookList = bookAuthRepo.GetAllBooksWithAuthors().Where(x => x.IsTaken).ToList();
                        break;
                }
            

            return View(bookList);
        }

        // GET: Book/AddBookAuthor
        public ActionResult AddBookWithAuthor()
        {
            return View();
        }

        // POST: Book/AddBookAuthor
        [HttpPost]
        public ActionResult AddBookWithAuthor(BooksWithAuthor bookWithAuthor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookWithAuthorRepository bookAuthRepo = new BookWithAuthorRepository();

                    if (bookAuthRepo.AddBookWithAuthor(bookWithAuthor))
                    {
                        ViewBag.Message = "Book details added successfully";
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/EditBookDetails/5
        public ActionResult EditBookWithAuthorDetails(int id)
        {
            BookWithAuthorRepository bookAuthRepo = new BookWithAuthorRepository();
            return View(bookAuthRepo.GetAllBooksWithAuthors().Find(book => book.Id == id));

        }

        // POST: Book/EditBookDetails/5
        [HttpPost]

        public ActionResult EditBookWithAuthorDetails(int id, BooksWithAuthor obj)
        {
            try
            {
                BookWithAuthorRepository bookAuthRepo = new BookWithAuthorRepository();
                bookAuthRepo.UpdateBook(obj);
                return RedirectToAction("GetAllBookAuthorDetails");
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/DeleteBook/5
        public ActionResult DeleteBookWithAuthor(int id)
        {
            try
            {
                BookWithAuthorRepository bookAuthRepo = new BookWithAuthorRepository();
                if (bookAuthRepo.DeleteBook(id))
                {
                    ViewBag.AlertMsg = "Book details deleted successfully";

                }
                return RedirectToAction("GetAllBookAuthorDetails");

            }
            catch
            {
                return View();
            }
        }

        public ActionResult TakeBookWithUser(int bookId, Guid userId)
        {
            try
            {
                BookWithAuthorRepository bookAuthRepo = new BookWithAuthorRepository();
                if (bookAuthRepo.TakeBook(bookId, userId))
                {
                    ViewBag.AlertMsg = "Book was taken";

                }
                return RedirectToAction("GetAllBookAuthorDetails");

            }
            catch
            {
                return View();

            }
        }

        public ActionResult ReturnBookWithUser(int bookId)
        {
            try
            {
                BookWithAuthorRepository bookAuthRepo = new BookWithAuthorRepository();
                if (bookAuthRepo.ReturnBook(bookId))
                {
                    ViewBag.AlertMsg = "Book was returned";

                }
                return RedirectToAction("GetAllBookAuthorDetails");

            }
            catch
            {
                return View();

            }
        }
    }
}

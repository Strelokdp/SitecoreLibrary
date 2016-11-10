using SitecoreLibrary.Models;
using SitecoreLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitecoreLibrary.Controllers
{
    public class BookController : Controller
    {

        // GET: Book/GetAllBookDetails
        public ActionResult GetAllBookDetails()
        {
            BookRepository bookRepo = new BookRepository();
            ModelState.Clear();
            return View(bookRepo.GetAllBooks());
        }


        // GET: Book/AddBook
        public ActionResult AddBook()
        {
            return View();
        }

        // POST: Book/AddBook
        [HttpPost]
        public ActionResult AddBook(Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookRepository BookRepo = new BookRepository();

                    if (BookRepo.AddBook(book))
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
        public ActionResult EditBookDetails(int id)
        {
            BookRepository bookRepo = new BookRepository();
            return View(bookRepo.GetAllBooks().Find(book => book.Bookid == id));

        }

        // POST: Book/EditBookDetails/5
        [HttpPost]

        public ActionResult EditBookDetails(int id, Book obj)
        {
            try
            {
                BookRepository bookRepo = new BookRepository();
                bookRepo.UpdateBook(obj);
                return RedirectToAction("GetAllBookDetails");
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/DeleteBook/5
        public ActionResult DeleteBook(int id)
        {
            try
            {
                BookRepository bookRepo = new BookRepository();
                if (bookRepo.DeleteBook(id))
                {
                    ViewBag.AlertMsg = "Book details deleted successfully";

                }
                return RedirectToAction("GetAllBookDetails");

            }
            catch
            {
                return View();
            }
        }
    }
}

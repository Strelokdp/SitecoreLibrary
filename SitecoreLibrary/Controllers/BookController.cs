using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using PagedList;
using SitecoreLibrary.BAL.Services;
using SitecoreLibrary.DAL.Contracts;
using SitecoreLibrary.DAL.Repository;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookService = new BookService();
        private readonly PostService _postService = new PostService();

        // GET: BookWithAuthor/GetAllBooks
        public ActionResult GetAllBooks(string selectList, string currentFilter, string sortOrder, int? page)
        {
            ModelState.Clear();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.BookSortParm = string.IsNullOrEmpty(sortOrder) ? "book_desc" : "";
            ViewBag.AuthorSortParm = sortOrder == "author" ? "author_desc" : "author";

            var bookFilterList = new List<string> {"All books", "Available books", "Taken books"} as IEnumerable<string>;

            ViewBag.selectList = new SelectList(bookFilterList);

            var bookList = _bookService.GetAllBooks();

            bookList = _bookService.FilterBooks(selectList, bookList);
            
            bookList = _bookService.SortBooks(sortOrder, bookList);

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(bookList.ToPagedList(pageNumber, pageSize));

        }

        // GET: BookWithAuthor/AddBook
        public ActionResult AddBook()
        {
            return View();
        }

        // POST: Book/AddBook
        [HttpPost]
        public ActionResult AddBook(Books book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (_bookService.AddBook(book))
                {
                    ViewBag.Message = "Book details added successfully";
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: BookWithAuthor/EditBook/5
        public ActionResult EditBook(int id)
        {
            return View(_bookService.GetAllBooks().Find(book => book.Id == id));

        }

        // POST:BookWithAuthor/EditBook/5
        [HttpPost]
        public ActionResult EditBook(int id, Books obj)
        {
            try
            {
                _bookService.UpdateBook(obj);
                return RedirectToAction("GetAllBooks");
            }
            catch
            {
                return View();
            }
        }

        // GET: BookWithAuthor/DeleteBook/5
        public ActionResult DeleteBook(int id)
        {
            try
            {
                if (_bookService.DeleteBook(id))
                {
                    ViewBag.AlertMsg = "Book details deleted successfully";

                }
                return RedirectToAction("GetAllBooks");

            }
            catch
            {
                return View();
            }
        }

        // POST:BookWithAuthor/TakeBookWithUser
        public ActionResult TakeBookWithUser(int bookId, Guid userId, string eMail, string bookName)
        {
            try
            {
                if (!_bookService.TakeBook(bookId, userId))
                {
                    return RedirectToAction("GetAllBooks");
                }

                ViewBag.AlertMsg = "Book was taken";
                
                _postService.SendBookTakenEmail(eMail, bookName);

                return RedirectToAction("GetAllBooks");

            }
            catch
            {
                return View();

            }
        }

        // POST:BookWithAuthor/ReturnBookWithUser
        public ActionResult ReturnBookWithUser(int bookId, string eMail, string bookName)
        {
            try
            {
                if (!_bookService.ReturnBook(bookId))
                {
                    return RedirectToAction("GetAllBooks");
                }

                ViewBag.AlertMsg = "Book was returned";

                _postService.SendBookReturnedEmail(eMail);

                return RedirectToAction("GetAllBooks");

            }
            catch
            {
                return View();

            }
        }

        // GET:BookWithAuthor/BookHistory
        public ActionResult BookHistory(int bookID)
        {
            return RedirectToAction("Index", "BookHistory", new { bookId = bookID });
        }
    }
}

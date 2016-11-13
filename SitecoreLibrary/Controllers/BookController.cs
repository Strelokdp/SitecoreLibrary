using System;
using System.Web.Mvc;
using System.Collections.Generic;
using PagedList;
using SitecoreLibrary.BAL.Services;
using SitecoreLibrary.ViewModels;
using SitecoreLibrary.BAL.Contracts;

namespace SitecoreLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService bookService;
        private readonly IPostService postService;

        public BookController(IBookService bookService, IPostService postService)
        {
            this.bookService = bookService;
            this.postService = postService;
        }

        // GET: BookWithAuthor/GetAllBooks
        public ActionResult GetAllBooks(string selectList, string currentFilter, string sortOrder, int? page)
        {
            ModelState.Clear();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.BookSortParm = string.IsNullOrEmpty(sortOrder) ? "book_desc" : "";
            ViewBag.AuthorSortParm = sortOrder == "author" ? "author_desc" : "author";

            var bookFilterList = new List<string> {"All books", "Available books", "Taken books"} as IEnumerable<string>;

            ViewBag.selectList = new SelectList(bookFilterList);

            var bookList = bookService.GetAllBooks();

            bookList = bookService.FilterBooks(selectList, bookList);
            
            bookList = bookService.SortBooks(sortOrder, bookList);

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

                if (bookService.AddBook(book))
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
            return View(bookService.GetAllBooks().Find(book => book.Id == id));

        }

        // POST:BookWithAuthor/EditBook/5
        [HttpPost]
        public ActionResult EditBook(int id, Books obj)
        {
            try
            {
                bookService.UpdateBook(obj);
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
                if (bookService.DeleteBook(id))
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
                if (!bookService.TakeBook(bookId, userId))
                {
                    return RedirectToAction("GetAllBooks");
                }

                ViewBag.AlertMsg = "Book was taken";
                
                postService.SendBookTakenEmail(eMail, bookName);

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
                if (!bookService.ReturnBook(bookId))
                {
                    return RedirectToAction("GetAllBooks");
                }

                ViewBag.AlertMsg = "Book was returned";

                postService.SendBookReturnedEmail(eMail);

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

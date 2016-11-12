using System;
using SitecoreLibrary.Models;
using SitecoreLibrary.Repository;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using PagedList;

namespace SitecoreLibrary.Controllers
{
    public class BookWithAuthorController : Controller
    {
        private BookWithAuthorRepository _bookAuthRep = new BookWithAuthorRepository();

        // GET: Book/GetAllBookAuthorDetails
        public ActionResult GetAllBookAuthorDetails(string SelectList, string currentFilter, string sortOrder, int? page)
        {
            ModelState.Clear();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.BookSortParm = String.IsNullOrEmpty(sortOrder) ? "book_desc" : "";
            ViewBag.AuthorSortParm = sortOrder == "Author" ? "author_desc" : "Author";

            var bookFilterList = new List<string> {"All books", "Available books", "Taken books"} as IEnumerable<string>;

            ViewBag.SelectList = new SelectList(bookFilterList);

            var bookList = _bookAuthRep.GetAllBooksWithAuthors();

            if (!String.IsNullOrEmpty(SelectList))
            {
                switch (SelectList)
                {
                    case ("Available books"):
                        bookList = _bookAuthRep.GetAllBooksWithAuthors().Where(x => !x.IsTaken).ToList();
                        break;

                    case ("Taken books"):
                        bookList = _bookAuthRep.GetAllBooksWithAuthors().Where(x => x.IsTaken).ToList();
                        break;

                    default:
                        bookList = _bookAuthRep.GetAllBooksWithAuthors();
                        break;
                }
            }

            switch (sortOrder)
            {
                case "author_desc":
                    bookList = bookList.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "Author":
                    bookList = bookList.OrderBy(s => s.FullName).ToList();
                    break;
                case "book_desc":
                    bookList = bookList.OrderByDescending(s => s.BookName).ToList();
                    break;
                default:
                    bookList = bookList.OrderBy(s => s.BookName).ToList();
                    break;
            }

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(bookList.ToPagedList(pageNumber, pageSize));

        }

        // GET: Book/AddBookWithAuthor
        public ActionResult AddBookWithAuthor()
        {
            return View();
        }

        // POST: Book/AddBookWithAuthor
        [HttpPost]
        public ActionResult AddBookWithAuthor(BooksWithAuthor bookWithAuthor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_bookAuthRep.AddBookWithAuthor(bookWithAuthor))
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
            return View(_bookAuthRep.GetAllBooksWithAuthors().Find(book => book.Id == id));

        }

        // POST: Book/EditBookDetails/5
        [HttpPost]
        public ActionResult EditBookWithAuthorDetails(int id, BooksWithAuthor obj)
        {
            try
            {
                _bookAuthRep.UpdateBook(obj);
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
                if (_bookAuthRep.DeleteBook(id))
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

        public ActionResult TakeBookWithUser(int bookId, Guid userId, string eMail, string bookName)
        {
            try
            {
                if (_bookAuthRep.TakeBook(bookId, userId))
                {
                    ViewBag.AlertMsg = "Book was taken";
                    var fromAddress = new MailAddress("sitecorelibrary2016@gmail.com", "Sitecore Library");
                    var toAddress = new MailAddress(eMail, eMail);
                    const string fromPassword = "sitecore2016";
                    const string subject = " has been taken";
                    const string body = "You have just taken book, don't forget to return it";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = bookName + subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }
                }
                return RedirectToAction("GetAllBookAuthorDetails");

            }
            catch
            {
                return View();

            }
        }

        public ActionResult ReturnBookWithUser(int bookId, string eMail, string bookName)
        {
            try
            {
                if (_bookAuthRep.ReturnBook(bookId))
                {
                    ViewBag.AlertMsg = "Book was returned";
                    var fromAddress = new MailAddress("sitecorelibrary2016@gmail.com", "Sitecore Library");
                    var toAddress = new MailAddress(eMail, eMail);
                    const string fromPassword = "sitecore2016";
                    const string subject = "Book has been returned";
                    const string body = "You have just returned book. Hope you liked it";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }

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

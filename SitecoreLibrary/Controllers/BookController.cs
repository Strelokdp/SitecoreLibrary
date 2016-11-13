using System;
using SitecoreLibrary.Repository;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using PagedList;
using SitecoreLibrary.Contracts;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookAuthRep = new BookRepository();

        // GET: BookWithAuthor/GetAllBooks
        public ActionResult GetAllBooks(string selectList, string currentFilter, string sortOrder, int? page)
        {
            ModelState.Clear();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.BookSortParm = string.IsNullOrEmpty(sortOrder) ? "book_desc" : "";
            ViewBag.AuthorSortParm = sortOrder == "author" ? "author_desc" : "author";

            var bookFilterList = new List<string> {"All books", "Available books", "Taken books"} as IEnumerable<string>;

            ViewBag.selectList = new SelectList(bookFilterList);

            var bookList = _bookAuthRep.GetAllBooks();

            if (!string.IsNullOrEmpty(selectList))
            {
                switch (selectList)
                {
                    case ("Available books"):
                        bookList = _bookAuthRep.GetAllBooks().Where( x => !x.IsTaken ). 
                                                              Where( x=>x.BookQuantity>0 )
                                                              .ToList();
                        break;

                    case ("Taken books"):
                        bookList = _bookAuthRep.GetAllBooks().Where(x => x.IsTaken).ToList();
                        break;

                    default:
                        bookList = _bookAuthRep.GetAllBooks();
                        break;
                }
            }

            switch (sortOrder)
            {
                case "author_desc":
                    bookList = bookList.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "author":
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

                if (_bookAuthRep.AddBook(book))
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
            return View(_bookAuthRep.GetAllBooks().Find(book => book.Id == id));

        }

        // POST:BookWithAuthor/EditBook/5
        [HttpPost]
        public ActionResult EditBook(int id, Books obj)
        {
            try
            {
                _bookAuthRep.UpdateBook(obj);
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
                if (_bookAuthRep.DeleteBook(id))
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
                if (!_bookAuthRep.TakeBook(bookId, userId))
                {
                    return RedirectToAction("GetAllBooks");
                }

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
                if (!_bookAuthRep.ReturnBook(bookId))
                {
                    return RedirectToAction("GetAllBooks");
                }

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

using System;
using System.Collections.Generic;
using System.Linq;
using SitecoreLibrary.DAL.Contracts;
using SitecoreLibrary.DAL.Repository;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.BAL.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookAuthRep = new BookRepository();

        public List<Books> SortBooks(string sortOrder, List<Books> bookList)
        {
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

            return bookList;
        }

        public List<Books> FilterBooks (string selectList, List<Books> bookList )
        {
            if (!string.IsNullOrEmpty(selectList))
            {
                switch (selectList)
                {
                    case ("Available books"):
                        bookList = _bookAuthRep.GetAllBooks().Where(x => !x.IsTaken).
                                                              Where(x => x.BookQuantity > 0)
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
            return bookList;
        }

        public List<Books> GetAllBooks()
        {
            return _bookAuthRep.GetAllBooks();
        }

        public bool AddBook(Books book)
        {
            return _bookAuthRep.AddBook(book);
        }

        public bool UpdateBook(Books book)
        {
            return _bookAuthRep.UpdateBook(book);
        }

        public bool DeleteBook(int Id)
        {
            return _bookAuthRep.DeleteBook(Id);
        }

        public bool TakeBook(int bookId, Guid userId)
        {
            return _bookAuthRep.TakeBook(bookId, userId);
        }

        public bool ReturnBook(int bookId)
        {
            return _bookAuthRep.ReturnBook(bookId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using SitecoreLibrary.BAL.Contracts;
using SitecoreLibrary.DAL.Contracts;
using SitecoreLibrary.DAL.Repository;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.BAL.Services
{
    public class BookService:IBookService
    {
        private readonly BookRepository bookAuthRep;

        public BookService(BookRepository bookAuthRep)
        {
            this.bookAuthRep = bookAuthRep;
        }

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
                        bookList = bookAuthRep.GetAllBooks().Where(x => !x.IsTaken).
                                                              Where(x => x.BookQuantity > 0)
                                                              .ToList();
                        break;

                    case ("Taken books"):
                        bookList = bookAuthRep.GetAllBooks().Where(x => x.IsTaken).ToList();
                        break;

                    default:
                        bookList = bookAuthRep.GetAllBooks();
                        break;
                }
            }
            return bookList;
        }

        public List<Books> GetAllBooks()
        {
            return bookAuthRep.GetAllBooks();
        }

        public bool AddBook(Books book)
        {
            return bookAuthRep.AddBook(book);
        }

        public bool UpdateBook(Books book)
        {
            return bookAuthRep.UpdateBook(book);
        }

        public bool DeleteBook(int Id)
        {
            return bookAuthRep.DeleteBook(Id);
        }

        public bool TakeBook(int bookId, Guid userId)
        {
            return bookAuthRep.TakeBook(bookId, userId);
        }

        public bool ReturnBook(int bookId)
        {
            return bookAuthRep.ReturnBook(bookId);
        }
    }
}
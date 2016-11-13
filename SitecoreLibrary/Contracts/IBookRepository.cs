using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.Contracts
{
    public interface IBookRepository
    {
        List<Books> GetAllBooks();
        bool AddBook(Books obj);
        bool UpdateBook(Books obj);
        bool DeleteBook(int Id);
        bool TakeBook(int bookId, Guid userId);
        bool ReturnBook(int bookId);
    }
}
using System.Collections.Generic;
using SitecoreLibrary.ViewModels;
using System;

namespace SitecoreLibrary.BAL.Contracts
{
    public interface IBookService
    {
        List<Books> SortBooks(string sortOrder, List<Books> bookList);
        List<Books> FilterBooks(string selectList, List<Books> bookList);
        List<Books> GetAllBooks();
        bool AddBook(Books book);
        bool UpdateBook(Books book);
        bool DeleteBook(int Id);
        bool TakeBook(int bookId, Guid userId);
        bool ReturnBook(int bookId);

    }
}
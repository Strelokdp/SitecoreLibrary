using System.Collections.Generic;
using SitecoreLibrary.DAL.Contracts;
using SitecoreLibrary.DAL.Repository;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.BAL.Services
{
    public class BookHistoryService
    {
        private readonly IBookHistoryRepository _bookAuthRep = new BookHistoryRepository();

        public List<BookHistory> GetBooksHistory()
        {
            return _bookAuthRep.GetBooksHistory();
        }
    }
}
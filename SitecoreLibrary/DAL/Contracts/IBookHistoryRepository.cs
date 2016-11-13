using System.Collections.Generic;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.DAL.Contracts
{
    public interface IBookHistoryRepository
    {
        List<BookHistory> GetBooksHistory();

    }
}

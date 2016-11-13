using System.Collections.Generic;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.BAL.Contracts
{
    public interface IBookHistoryService
    {
        List<BookHistory> GetBooksHistory();
    }
}
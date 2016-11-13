using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.Contracts
{
    public interface IBookHistoryRepository
    {
        List<BookHistory> GetBooksHistory();

    }
}

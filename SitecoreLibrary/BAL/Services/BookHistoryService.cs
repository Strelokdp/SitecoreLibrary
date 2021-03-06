﻿using System.Collections.Generic;
using SitecoreLibrary.BAL.Contracts;
using SitecoreLibrary.DAL.Contracts;
using SitecoreLibrary.DAL.Repository;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.BAL.Services
{
    public class BookHistoryService:IBookHistoryService
    {
        private readonly IBookHistoryRepository bookAuthRep;

        public BookHistoryService(IBookHistoryRepository bookAuthRep)
        {
            this.bookAuthRep = bookAuthRep;
        }

        public List<BookHistory> GetBooksHistory()
        {
            return bookAuthRep.GetBooksHistory();
        }
    }
}
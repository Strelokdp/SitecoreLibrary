using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SitecoreLibrary.DAL.Contracts;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.DAL.Repository
{
    public class BookHistoryRepository: GenericRepository, IBookHistoryRepository
    {
        private SqlConnection _con;
  
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SitecoreConn"].ToString();
            _con = new SqlConnection(constr);

        }

        public List<BookHistory> GetBooksHistory()
        {
            return GetFromDbSp("GetBooksHistory", (dr) => new BookHistory
            {
                Id = Convert.ToInt32(dr["HistoryRecID"]),
                BookName = Convert.ToString(dr["BookName"]),
                BookId = Convert.ToInt32(dr["BookId"]),
                Date = Convert.ToDateTime(dr["TimeTaken"]),
                UserName = Convert.ToString(dr["UserName"])
            });
        }
    }
}
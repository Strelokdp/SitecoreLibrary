using SitecoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SitecoreLibrary.Repository
{
    public class BookHistoryRepository
    {
        private SqlConnection _con;
  
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SitecoreConn"].ToString();
            _con = new SqlConnection(constr);

        }

        public List<BookHistory> GetAllBooksHistory()
        {
            Connection();
            List<BookHistory> booksHistory = new List<BookHistory>();

            SqlCommand com = new SqlCommand("GetBooksHistory", _con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            _con.Open();
            da.Fill(dt);
            _con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                booksHistory.Add(
                    new BookHistory
                    {
                        Id = Convert.ToInt32(dr["HistoryRecID"]),
                        BookName = Convert.ToString(dr["BookName"]),
                        BookId = Convert.ToInt32(dr["BookId"]),
                        Date = Convert.ToDateTime(dr["TimeTaken"]),
                        UserName = Convert.ToString(dr["UserName"])
                    }
                );
            }
            return booksHistory;
        }
    }
}
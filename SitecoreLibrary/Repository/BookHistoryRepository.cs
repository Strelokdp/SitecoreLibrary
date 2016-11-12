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
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SitecoreConn"].ToString();
            con = new SqlConnection(constr);

        }

        //To view Author details with generic list     
        public List<BookHistory> GetAllBooksHistory()
        {
            connection();
            List<BookHistory> booksHistory = new List<BookHistory>();

            SqlCommand com = new SqlCommand("GetBooksHistory", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind Author generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {
                booksHistory.Add(
                    new BookHistory
                    {
                        Id = Convert.ToInt32(dr["HistoryRecID"]),
                        BookName = Convert.ToString(dr["BookName"]),
                        //BookId = Convert.ToInt32(dr["BookId"]),
                        Date = Convert.ToDateTime(dr["TimeTaken"]),
                        UserName = Convert.ToString(dr["UserName"])
                    }
                );
            }
            return booksHistory;
        }
    }
}
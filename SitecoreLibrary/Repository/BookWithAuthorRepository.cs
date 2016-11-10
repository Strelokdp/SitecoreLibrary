using SitecoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SitecoreLibrary.Repository
{
    public class BookWithAuthorRepository
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SitecoreConn"].ToString();
            con = new SqlConnection(constr);

        }

        //To view Author details with generic list     
        public List<BooksWithAuthor> GetAllBooksWithAuthors()
        {
            connection();
            List<BooksWithAuthor> booksWithAuthorList = new List<BooksWithAuthor>();


            SqlCommand com = new SqlCommand("GetBooksWithAuthors", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind Author generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                booksWithAuthorList.Add(
                    new BooksWithAuthor
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        BookName = Convert.ToString(dr["BookName"]),
                        AuthorName = Convert.ToString(dr["AuthorName"]),
                    }
                );
            }

            return booksWithAuthorList;
        }

    }
}
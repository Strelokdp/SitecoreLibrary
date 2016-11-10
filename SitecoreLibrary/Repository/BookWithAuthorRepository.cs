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
                        Id = Convert.ToInt32(dr["BookToAuthorId"]),
                        BookName = Convert.ToString(dr["BookName"]),
                        FirstName = Convert.ToString(dr["FirstName"]),
                        LastName = Convert.ToString(dr["LastName"]),
                    }
                );
            }

            return booksWithAuthorList;
        }

        //To Add Book details   
        public bool AddBookWithAuthor(BooksWithAuthor obj)
        {

            connection();
            SqlCommand com = new SqlCommand("AddNewBookWithAuthorDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookName", obj.BookName);
            com.Parameters.AddWithValue("@FirstName", obj.FirstName);
            com.Parameters.AddWithValue("@LastName", obj.LastName);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }
        }

        //To Update Book details    
        public bool UpdateBook(BooksWithAuthor obj)
        {

            connection();
            SqlCommand com = new SqlCommand("UpdateBookWithAuthorDetails", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookToAuthorId", obj.Id);
            com.Parameters.AddWithValue("@BookName", obj.BookName);
            com.Parameters.AddWithValue("@FirstName", obj.FirstName);
            com.Parameters.AddWithValue("@LastName", obj.LastName);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
        //To delete Book details    
        public bool DeleteBook(int Id)
        {

            connection();
            SqlCommand com = new SqlCommand("DeleteBookWithAuthorById", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookToAuthorId", Id);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
    }
}
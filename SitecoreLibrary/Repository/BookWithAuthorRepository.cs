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
                        TakenByUserId = Guid.Parse(dr["TakenByUserID"].ToString()),
                        IsTaken = Convert.ToBoolean(dr["IsTaken"])
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
            //com.Parameters.AddWithValue("@TakenByUserID", obj.TakenByUserId);
            //com.Parameters.AddWithValue("@IsTaken", obj.IsTaken);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            return (i >= 1);
        }

        //To Update Book details    
        public bool UpdateBook(BooksWithAuthor obj)
        {

            connection();
            SqlCommand com = new SqlCommand("UpdateBookWithAuthorDetails", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", obj.Id);
            com.Parameters.AddWithValue("@BookName", obj.BookName);
            com.Parameters.AddWithValue("@FirstName", obj.FirstName);
            com.Parameters.AddWithValue("@LastName", obj.LastName);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            return (i >= 1);
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
            return (i >= 1);
        }

        public bool TakeBook(int bookId, Guid userId)
        {

            connection();
            SqlCommand com = new SqlCommand("TakeBookWithUser", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookToAuthorId", bookId);
            com.Parameters.AddWithValue("@UserId", userId);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            return (i >= 1);
        }

        public bool ReturnBook(int bookId)
        {

            connection();
            SqlCommand com = new SqlCommand("ReturnBookWithUser", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookToAuthorId", bookId);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            return (i >= 1);
        }
    }
}
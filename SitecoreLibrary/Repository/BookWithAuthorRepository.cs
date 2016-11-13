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
        private SqlConnection _con;
        
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SitecoreConn"].ToString();
            _con = new SqlConnection(constr);

        }

        public List<BooksWithAuthor> GetAllBooks()
        {
            Connection();
            List<BooksWithAuthor> booksWithAuthorList = new List<BooksWithAuthor>();

            SqlCommand com = new SqlCommand("GetBooksWithAuthors", _con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            _con.Open();
            da.Fill(dt);
            _con.Close();
               
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

        public bool AddBook(BooksWithAuthor obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("AddNewBookWithAuthorDetails", _con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@BookName", obj.BookName);
            com.Parameters.AddWithValue("@FirstName", obj.FirstName);
            com.Parameters.AddWithValue("@LastName", obj.LastName);

            _con.Open();
            int i = com.ExecuteNonQuery();
            _con.Close();
            return (i >= 1);
        }

        public bool UpdateBook(BooksWithAuthor obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateBookWithAuthorDetails", _con);

            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@Id", obj.Id);
            com.Parameters.AddWithValue("@BookName", obj.BookName);
            com.Parameters.AddWithValue("@FirstName", obj.FirstName);
            com.Parameters.AddWithValue("@LastName", obj.LastName);

            _con.Open();
            int i = com.ExecuteNonQuery();
            _con.Close();
            return (i >= 1);
        }

        public bool DeleteBook(int Id)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteBookWithAuthorById", _con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookToAuthorId", Id);
            
            _con.Open();
            int i = com.ExecuteNonQuery();
            _con.Close();
            return (i >= 1);
        }

        public bool TakeBook(int bookId, Guid userId)
        {

            Connection();
            SqlCommand com = new SqlCommand("TakeBookWithUser", _con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookToAuthorId", bookId);
            com.Parameters.AddWithValue("@UserId", userId);

            _con.Open();
            int i = com.ExecuteNonQuery();
            _con.Close();
            return (i >= 1);
        }

        public bool ReturnBook(int bookId)
        {

            Connection();
            SqlCommand com = new SqlCommand("ReturnBookWithUser", _con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@BookToAuthorId", bookId);

            _con.Open();
            int i = com.ExecuteNonQuery();
            _con.Close();
            return (i >= 1);
        }
    }
}
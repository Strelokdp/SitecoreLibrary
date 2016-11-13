using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SitecoreLibrary.DAL.Contracts;
using SitecoreLibrary.ViewModels;

namespace SitecoreLibrary.DAL.Repository
{
    public class BookRepository : GenericRepository, IBookRepository
    {
        private SqlConnection _con;
        
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SitecoreConn"].ToString();
            _con = new SqlConnection(constr);

        }

        public IList<Books> GetAllBooks2()
        {
            return GetFromDbSp("GetBooksWithAuthors", (dr) => new Books
            {
                Id = Convert.ToInt32(dr["BookToAuthorId"]),
                BookRecordId = Convert.ToInt32(dr["BookID"]),
                BookQuantity = Convert.ToInt32(dr["BookQuantity"]),
                BookName = Convert.ToString(dr["BookName"]),
                FirstName = Convert.ToString(dr["FirstName"]),
                LastName = Convert.ToString(dr["LastName"]),
                TakenByUserId = Guid.Parse(dr["TakenByUserID"].ToString()),
                IsTaken = Convert.ToBoolean(dr["IsTaken"])
            });
        }

        public List<Books> GetAllBooks()
        {
            Connection();
            List<Books> booksWithAuthorList = new List<Books>();

            SqlCommand com = new SqlCommand("GetBooksWithAuthors", _con) {CommandType = CommandType.StoredProcedure};
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            _con.Open();
            da.Fill(dt);
            _con.Close();
               
            foreach (DataRow dr in dt.Rows)
            {
                booksWithAuthorList.Add(
                    new Books
                    {
                        Id = Convert.ToInt32(dr["BookToAuthorId"]),
                        BookRecordId = Convert.ToInt32(dr["BookID"]),
                        BookQuantity = Convert.ToInt32(dr["BookQuantity"]),
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

        public bool AddBook(Books obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("AddNewBookWithAuthorDetails", _con)
            {
                CommandType = CommandType.StoredProcedure
            };

            com.Parameters.AddWithValue("@BookName", obj.BookName);
            com.Parameters.AddWithValue("@FirstName", obj.FirstName);
            com.Parameters.AddWithValue("@LastName", obj.LastName);
            com.Parameters.AddWithValue("@BookQuantity", obj.BookQuantity);

            _con.Open();
            int i = com.ExecuteNonQuery();
            _con.Close();
            return (i >= 1);
        }

        public bool UpdateBook(Books obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateBookWithAuthorDetails", _con)
            {
                CommandType = CommandType.StoredProcedure
            };


            com.Parameters.AddWithValue("@Id", obj.Id);
            com.Parameters.AddWithValue("@BookName", obj.BookName);
            com.Parameters.AddWithValue("@FirstName", obj.FirstName);
            com.Parameters.AddWithValue("@LastName", obj.LastName);
            com.Parameters.AddWithValue("@BookQuantity", obj.BookQuantity);

            _con.Open();
            int i = com.ExecuteNonQuery();
            _con.Close();
            return (i >= 1);
        }

        public bool DeleteBook(int Id)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteBookWithAuthorById", _con)
            {
                CommandType = CommandType.StoredProcedure
            };

            com.Parameters.AddWithValue("@BookToAuthorId", Id);
            
            _con.Open();
            int i = com.ExecuteNonQuery();
            _con.Close();
            return (i >= 1);
        }

        public bool TakeBook(int bookId, Guid userId)
        {

            Connection();
            SqlCommand com = new SqlCommand("TakeBookWithUser", _con) {CommandType = CommandType.StoredProcedure};

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
            SqlCommand com = new SqlCommand("ReturnBookWithUser", _con) {CommandType = CommandType.StoredProcedure};

            com.Parameters.AddWithValue("@BookToAuthorId", bookId);

            _con.Open();
            int i = com.ExecuteNonQuery();
            _con.Close();
            return (i >= 1);
        }
    }
}
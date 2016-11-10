using SitecoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SitecoreLibrary.Repository
{
    public class BookRepository
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SitecoreConn"].ToString();
            con = new SqlConnection(constr);

        }
        //To Add Book details    
        public bool AddBook(Book obj)
        {

            connection();
            SqlCommand com = new SqlCommand("AddNewBookDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", obj.Name);
            com.Parameters.AddWithValue("@Author", obj.Author);

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
        //To view Book details with generic list     
        public List<Book> GetAllBooks()
        {
            connection();
            List<Book> BookList = new List<Book>();


            SqlCommand com = new SqlCommand("GetBooks", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind Book generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                BookList.Add(

                    new Book
                    {

                        Bookid = Convert.ToInt32(dr["Id"]),
                        Name = Convert.ToString(dr["Name"]),
                        Author = Convert.ToString(dr["Author"])
                    }
                    );


            }

            return BookList;


        }
        //To Update Book details    
        public bool UpdateBook(Book obj)
        {

            connection();
            SqlCommand com = new SqlCommand("UpdateBookDetails", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookId", obj.Bookid);
            com.Parameters.AddWithValue("@Name", obj.Name);
            com.Parameters.AddWithValue("@Author", obj.Author);
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
            SqlCommand com = new SqlCommand("DeleteBookById", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookId", Id);

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
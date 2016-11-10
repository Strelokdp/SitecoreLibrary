using SitecoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SitecoreLibrary.Repository
{
    public class AuthorRepository
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["SitecoreConn"].ToString();
            con = new SqlConnection(constr);

        }
        //To Add Author details    
        public bool AddAuthor(Author obj)
        {

            connection();
            SqlCommand com = new SqlCommand("AddNewAuthorDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", obj.Name);

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
        //To view Author details with generic list     
        public List<Author> GetAllAuthors()
        {
            connection();
            List<Author> AuthorList = new List<Author>();


            SqlCommand com = new SqlCommand("GetAuthors", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            //Bind Author generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                AuthorList.Add(
                    new Author
                    {
                        Authorid = Convert.ToInt32(dr["Id"]),
                        Name = Convert.ToString(dr["Name"]),
                    }
                );
            }

            return AuthorList;
        }
        //To Update Author details    
        public bool UpdateAuthor(Author obj)
        {

            connection();
            SqlCommand com = new SqlCommand("UpdateAuthorDetails", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@AuthorId", obj.Authorid);
            com.Parameters.AddWithValue("@Name", obj.Name);
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
        //To delete Author details    
        public bool DeleteAuthor(int Id)
        {

            connection();
            SqlCommand com = new SqlCommand("DeleteAuthorById", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@AuthorId", Id);

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
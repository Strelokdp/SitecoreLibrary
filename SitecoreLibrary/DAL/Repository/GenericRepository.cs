using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SitecoreLibrary.DAL.Repository
{
    public abstract class GenericRepository
    {
        protected IList<T> GetFromDbSp<T>(string spName, Func<DataRow, T> processRowFunc)
        {
            string constr = ConfigurationManager.ConnectionStrings["SitecoreConn"].ToString();

            using (var _con = new SqlConnection(constr))
            {
                SqlCommand com = new SqlCommand(spName, _con) {CommandType = CommandType.StoredProcedure};
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();

                _con.Open();
                da.Fill(dt);
                var list = new List<T>();

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(processRowFunc(dr));
                }

                return list;
            }
        }
    }
}
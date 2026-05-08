using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using WebBanHang.DTO.Entity;


namespace WebBanHang.DAL.Repositories
{
    public class CategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Category> GetAllCategoriesADO()
        {
            List<Category> categories = new List<Category>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Name FROM Categories";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "CategoryTable");

                foreach (DataRow row in ds.Tables["CategoryTable"].Rows)
                {
                    categories.Add(new Category
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString()
                    });
                }
            }
            return categories;
        }
    }
}

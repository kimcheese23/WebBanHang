using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebBanHang.DAL;
using WebBanHang.DTO.Entity;

namespace WebBanHang.GUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryApiController : ControllerBase
    {
        private readonly MyDbContext db;

        public CategoryApiController(MyDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = new List<Category>();
            string? connectionString = db.Database.GetDbConnection().ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Categories", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Category
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return Ok(list);
        }

        [HttpGet("search")]
        public IActionResult Search(string? name)
        {
            var list = new List<Category>();
            string? connectionString = db.Database.GetDbConnection().ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchCategories", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", name ?? "");
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Category
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Create(Category cat)
        {
            string? connectionString = db.Database.GetDbConnection().ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddCategory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", cat.Name);
                    conn.Open();

                    var id = cmd.ExecuteScalar();
                    cat.Id = Convert.ToInt32(id);
                }
            }
            return Ok(cat);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Category cat)
        {
            string? connectionString = db.Database.GetDbConnection().ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateCategory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", cat.Name);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(cat);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            string? connectionString = db.Database.GetDbConnection().ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return NoContent();
            }
            catch (SqlException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
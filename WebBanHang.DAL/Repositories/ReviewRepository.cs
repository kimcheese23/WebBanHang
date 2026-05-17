using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebBanHang.DAL;
using WebBanHang.DTO.Entity;

namespace WebBanHang.DAL.Repositories
{
    public class ReviewRepository
    {
        private readonly MyDbContext _context;

        public ReviewRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckReviewEligibilityAsync(string userId, int productId)
        {
            var query = _context.Database.SqlQueryRaw<bool>(
                "SELECT dbo.fn_CheckReviewEligibility({0}, {1}) AS Value",
                userId, productId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<int> GetValidOrderIdAsync(string userId, int productId)
        {
            var orderId = await _context.Orders
                .Where(o => o.UserId == userId && o.Status == 3)
                .Where(o => _context.OrderDetails.Any(od => od.OrderId == o.Id && od.ProductId == productId))
                .Select(o => o.Id)
                .FirstOrDefaultAsync();

            return orderId;
        }
        public async Task<Review?> GetExistingReviewAsync(string userId, int productId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);
        }
        public async Task AddReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }
    }
}
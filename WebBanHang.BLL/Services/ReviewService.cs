using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.DAL.Repositories;
using WebBanHang.DTO.Entity;
using WebBanHang.DTO.Requests;

namespace WebBanHang.BLL.Services
{
    public class ReviewService
    {
        private readonly ReviewRepository _reviewRepo;

        public ReviewService(ReviewRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public async Task<bool> CheckReviewEligibilityAsync(string userId, int productId)
        {
            return await _reviewRepo.CheckReviewEligibilityAsync(userId, productId);
        }

        public async Task<bool> SubmitReviewAsync(ReviewRequestDTO request)
        {
            bool isEligible = await _reviewRepo.CheckReviewEligibilityAsync(request.UserId!, request.ProductId);
            if (!isEligible) throw new Exception("Bạn cần nhận hàng thành công trước khi đánh giá.");

            string safeContent = FilterBadWords(request.Content);

            var existingReview = await _reviewRepo.GetExistingReviewAsync(request.UserId!, request.ProductId);

            if (existingReview != null)
            {
                existingReview.Rating = request.Rating;
                existingReview.Content = safeContent;
                existingReview.CreatedAt = DateTime.Now;

                await _reviewRepo.UpdateReviewAsync(existingReview);
            }
            else
            {
                int validOrderId = await _reviewRepo.GetValidOrderIdAsync(request.UserId!, request.ProductId);
                var newReview = new Review
                {
                    UserId = request.UserId!,
                    ProductId = request.ProductId,
                    OrderId = validOrderId,
                    Rating = request.Rating,
                    Content = safeContent,
                    CreatedAt = DateTime.Now
                };
                await _reviewRepo.AddReviewAsync(newReview);
            }

            return true;
        }

        private string FilterBadWords(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return content;
            var badWords = new List<string> { "lừa đảo", "hàng giả", "chửi", "đm", "vcl", "ngu" };
            var words = content.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var filteredWords = words.Select(w => badWords.Contains(w.ToLower()) ? "***" : w);
            return string.Join(" ", filteredWords);
        }

        public async Task<Review?> GetExistingReviewAsync(string userId, int productId)
        {
            return await _reviewRepo.GetExistingReviewAsync(userId, productId);
        }
    }
}
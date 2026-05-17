using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebBanHang.BLL.Services;
using WebBanHang.DTO.Requests;

namespace WebBanHang.GUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewApiController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewApiController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("submit")]
        [Authorize]
        public async Task<IActionResult> SubmitReview([FromBody] ReviewRequestDTO request)
        {
            try
            {
                request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                bool success = await _reviewService.SubmitReviewAsync(request);
                return Ok(new { success = true, message = "Cảm ơn bạn đã đánh giá!" });
            }
            catch (Exception ex)
            {
                string exactError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { success = false, message = exactError });
            }
        }
    }
}
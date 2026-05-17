using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanHang.DAL.Repositories;
using WebBanHang.DTO.Responses;

namespace WebBanHang.BLL.Services
{
    public class AdminService
    {
        private readonly AdminRepository _adminRepo;

        public AdminService(AdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public List<AdminOrderDTO> GetOrdersWithADO()
        {
            return _adminRepo.GetOrdersWithADO();
        }

        public bool UpdateOrderStatus(int orderId, int newStatus)
        {
            return _adminRepo.UpdateOrderStatus(orderId, newStatus);
        }

        public async Task<DashboardSummaryDTO> GetDashboardSummaryAsync()
        {
            return await _adminRepo.GetDashboardSummaryAsync();
        }

        public async Task<List<MonthlyRevenueDTO>> GetMonthlyRevenueReportAsync(int year)
        {
            return await _adminRepo.GetMonthlyRevenueReportAsync(year);
        }

        public async Task<List<TopSellingProductDTO>> GetTopSellingProductsAsync(int topCount)
        {
            return await _adminRepo.GetTopSellingProductsAsync(topCount);
        }
    }
}
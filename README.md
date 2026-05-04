# 🛒 WebBanHang - ASP.NET Core Project

Dự án xây dựng hệ thống quản lý bán hàng sử dụng nền tảng ASP.NET Core, được thực hiện trong khuôn khổ môn học **Lập trình Cơ sở dữ liệu**.

---

## 🏗 Cấu trúc dự án (3-Layer Architecture)

Dự án được tổ chức theo mô hình 3 lớp (với việc tách biệt Data Transfer Objects) để đảm bảo tính dễ bảo trì và mở rộng:

- **WebBanHang.GUI (Presentation Layer)**: Giao diện người dùng được xây dựng bằng ASP.NET Core MVC/API kết hợp với Swagger để quản lý tài liệu API.  
- **WebBanHang.BLL (Business Logic Layer)**: Xử lý các nghiệp vụ logic của hệ thống, đóng vai trò trung gian giữa GUI và DAL.  
- **WebBanHang.DAL (Data Access Layer)**: Tương tác trực tiếp với cơ sở dữ liệu SQL Server thông qua Entity Framework Core.  
- **WebBanHang.DTO (Data Transfer Object)**: Chứa các lớp định nghĩa cấu trúc dữ liệu truyền tải giữa các lớp, bao gồm các Entities (như Product, ApplicationUser) và các ViewModel.  

---

## 🚀 Công nghệ & Phiên bản sử dụng

**Ngôn ngữ & Framework**  
- .NET 9.0 (ASP.NET Core, SDK-style project)

**ORM & Database**  
- Entity Framework Core 8.0.4  
- Microsoft.EntityFrameworkCore.SqlServer 8.0.4  
- Microsoft.EntityFrameworkCore.Tools 8.0.4  
- Microsoft SQL Server  

**Identity & Bảo mật**  
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 8.0.4  
- Microsoft.Extensions.Identity.Core 8.0.4  
- Microsoft.Extensions.Identity.Stores 8.0.4  

**API Documentation**  
- Swashbuckle.AspNetCore 10.1.7  
- Microsoft.OpenApi 3.5.3  

**Các thư viện hỗ trợ**  
- Microsoft.Extensions.Configuration 10.0.7  
- System.ComponentModel.Annotations 5.0.0  

**Công cụ phát triển**  
- Visual Studio 2022  
- SQL Server Management Studio (SSMS)  

---

## ✨ Các tính năng chính

- Quản lý sản phẩm: Xem danh sách, chi tiết, thêm, sửa, xóa sản phẩm.  
- Quản lý người dùng: Đăng ký, đăng nhập hệ thống (mở rộng từ IdentityUser).  
- Quản lý đơn hàng: Xử lý luồng đặt hàng và lưu trữ lịch sử giao dịch.  
- Phân quyền: Giới hạn quyền truy cập cho Admin và Client.  

---

## 🛠 Hướng dẫn cài đặt

**Clone project**  
```bash
git clone https://github.com/kimcheese23/WebBanHang.git
```
- Cấu hình Database: Mở file appsettings.json trong project GUI và thay đổi chuỗi kết nối DefaultConnection phù hợp với máy của bạn.
- Migration: Mở Package Manager Console và chạy lệnh(chọn Default project là WebBanHang.DAL):
```
Update-Database
```
- Khởi chạy ứng dụng:
Sử dụng Visual Studio 2022 (F5) 


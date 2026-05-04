using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebBanHang.DAL;

public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

        optionsBuilder.UseSqlServer("Server=.;Database=WebBanHang;Trusted_Connection=True;TrustServerCertificate=True");

        return new MyDbContext(optionsBuilder.Options);
    }
}

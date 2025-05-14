using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SampleProject.Web.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> option) :IdentityDbContext<IdentityUser,IdentityRole,string>(option)
    {

        public DbSet<Product> Products { get; set; }


    }
}

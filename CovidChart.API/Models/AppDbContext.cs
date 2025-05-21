using Microsoft.EntityFrameworkCore;

namespace CovidChart.API.Models
{
    public class AppDbContext (DbContextOptions<AppDbContext> options) :DbContext(options)
    {
        public DbSet<Covid> Covids { get; set; }

    }
}

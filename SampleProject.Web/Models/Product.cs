using Microsoft.EntityFrameworkCore;

namespace SampleProject.Web.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        [Precision(18,2)]
        public decimal Price { get; set; }
        public string UserId { get; set; } = null!;
    }
}

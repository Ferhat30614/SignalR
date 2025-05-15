using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SampleProject.Web.Models;
using System.Threading.Channels;

namespace SampleProject.Web.Services
{
    public class FileService(UserManager<IdentityUser> userManager
        ,IHttpContextAccessor httpContextAccessor
        ,AppDbContext context
        ,Channel<(string,List<Product>)> channel 
        )
    {

                                
        public async Task<bool> AddMessageToQueue()
        {

            var userId = userManager.GetUserId(httpContextAccessor.HttpContext!.User);

            var products= await context.Products.Where(a=>a.UserId==userId).ToListAsync();

            return channel.Writer.TryWrite((userId,products));
            //Burda eğer yazma işlemi olursa true olmazssa false döncek
            //TryWrite tam olarak buna yarıyor yoksa writeasync ilede aynı işlemi yapardık 
        }


    }
}

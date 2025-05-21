using CovidChart.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CovidChart.API.Models
{
    public class CovidService(AppDbContext _context,IHubContext<CovidHub> _hubContext)
    {
        // IQueryable kullandım
        public IQueryable<Covid> GetList()
        {
            return _context.Covids.AsQueryable();   
        }

        public async Task SaveCovid(Covid covid)
        {

            await _context.Covids.AddAsync(covid);      
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList","data");

        }



    }
}

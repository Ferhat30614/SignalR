using CovidChart.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace CovidChart.API.Hubs
{
    public class CovidHub(CovidService _covidService):Hub
    {

        

        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList", _covidService.GetCovidChartList());
        }

    }
}

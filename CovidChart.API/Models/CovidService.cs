using CovidChart.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList",GetCovidChartList());

        }



        public List<CovidChart> GetCovidChartList()
        {

            List<CovidChart> CovidCharts = new List<CovidChart>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText =
                    "select tarih,[1],[2],[3],[4],[5] From \r\n(Select CAST(CovidDate as date) as " +
                    "tarih,City,Count  From Covids) as kaynakTable\r\nPIVOT\r\n(SUM(Count) For City IN " +
                    "([1],[2],[3],[4],[5]) )\r\nas Pivottable\r\norder by tarih asc";
                 
                    
                command.CommandType=System.Data.CommandType.Text;
                _context.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {


                    while (reader.Read()) { 

                        CovidChart cc = new CovidChart();

                        cc.CovidDate = reader.GetDateTime(0).ToShortDateString();


                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {

                            if (System.DBNull.Value.Equals(reader[x]))
                            {
                                cc.Counts.Add(0);    
                            } 
                            else {
                                cc.Counts.Add(reader.GetInt32(x));
                            }

                        }
                        
                        );
                        CovidCharts.Add(cc);  
                    }
                }
            }


            _context.Database.CloseConnection();


            return CovidCharts;



        }


    }
}

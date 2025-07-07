using CovidChart.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidChart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController(CovidService _covidService) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid) 
        {

            await _covidService.SaveCovid(covid);
            IQueryable queryable =  _covidService.GetList();
            return Ok(queryable);   

        }
        
        [HttpGet]
        public IActionResult InitializeCovid() 
        { 

            Random rnd = new Random();

            Enumerable.Range(1, 10).ToList().ForEach( x =>
            {

                foreach(ECity item in Enum.GetValues(typeof(ECity)))
                {
                    Covid newCovid = new Covid() { City=item,Count=rnd.Next(100,1000),CovidDate=DateTime.Now.AddDays(x) };
                     _covidService.SaveCovid(newCovid).Wait();
                    Thread.Sleep(1000);
                }

            });


            return Ok("Covid19 dataları veritabanına kaydedildi");          
        
        }
    }

}

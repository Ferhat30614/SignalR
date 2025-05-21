using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using SampleProject.Web.Hubs;
using SampleProject.Web.Models;
using System.Data;
using System.Threading.Channels;

namespace SampleProject.Web.BackgroundServices
{
    public class CreateExcelBackgroundService (Channel<(string userid, List<Product> products)> channel,
         IFileProvider fileProvider
        ,IServiceProvider serviceProvider,IHubContext<AppHub> appHub) : BackgroundService
    {   
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (await channel.Reader.WaitToReadAsync(stoppingToken))
            {

                await Task.Delay(2000);

                var (userid, products) = await channel.Reader.ReadAsync(stoppingToken);



                var wwwrootFolder = fileProvider.GetDirectoryContents("wwwroot");
                var filesFolder = wwwrootFolder.Single(x => x.Name == "files");

                var newExcelFileName = $"Product-List-{Guid.NewGuid()}.xlsx";

                
                var newExcelFilePath = Path.Combine(filesFolder.PhysicalPath,newExcelFileName);

                var ds = new DataSet();
                ds.Tables.Add(GetTable("Product List", products));

                var wb = new XLWorkbook();
                wb.Worksheets.Add(ds);

                await using var excelFileStream = new FileStream(newExcelFilePath,FileMode.Create);

                wb.SaveAs(excelFileStream);


                

                await appHub.Clients.User(userid).SendAsync("AlertCompleteFile", $"/files/{newExcelFileName}", stoppingToken);





                //hub

                //using (var scope = serviceProvider.CreateScope())
                //{

                //    // using bloğu sayesinde bu scope içindeki servisler (örneğin appHub) iş bittikten sonra dispose edilir.
                //    // Aslında sadece 'scope' nesnesi değil, onunla birlikte oluşturulan scoped servisler de temizlenir.
                //    var appHub = scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>();

                //    await appHub.Clients.User(userid).SendAsync("AlertCompleteFile",$"/files/{newExcelFileName}",stoppingToken);

                //    //burda ben 2. sırada argümanı gönderdim

                //    //using (...) bloğu bittiğinde scope.Dispose() çağrılır.
                //    //Bu da scope içinde yaratılan tüm scoped servisleri(örneğin appHub) otomatik olarak temizler.

                //}

            }


        }

        private DataTable GetTable(string tableName,List<Product> products)
        {

           var table = new DataTable() { TableName=tableName};

           foreach(var item in typeof(Product).GetProperties())
           {
                table.Columns.Add(item.Name);   
           }

            products.ForEach(item => { table.Rows.Add(item.Id, item.Name, item.Description, item.Price, item.UserId); });
                    
           return table;      

        }



    }
}

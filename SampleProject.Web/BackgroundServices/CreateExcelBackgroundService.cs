
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using SampleProject.Web.Models;
using System.Data;
using System.Threading.Channels;

namespace SampleProject.Web.BackgroundServices
{
    public class CreateExcelBackgroundService (Channel<(string userid, List<Product> products)> channel,
         IFileProvider fileProvider
        ,IServiceProvider serviceProvider) : BackgroundService
    {   
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (await channel.Reader.WaitToReadAsync(stoppingToken))
            {

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

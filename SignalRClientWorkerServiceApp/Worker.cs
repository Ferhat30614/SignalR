using Microsoft.AspNetCore.SignalR.Client;
using SignalRClientWorkServiceApp;

namespace SignalRClientWorkerServiceApp
{
    public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
    {
        //private readonly ILogger<Worker> _logger;        
        //private readonly IConfiguration _configuration;

        //public Worker(ILogger<Worker> logger, HubConnection? connection, IConfiguration configuration
        //    )
        //{
        //    _logger = logger;
        //    _configuration = configuration;
        //}

        private HubConnection? connection;

        public override  Task StartAsync(CancellationToken cancellationToken)
        {
            connection = new HubConnectionBuilder().WithUrl(configuration.GetSection("SignalR")["Hub"]!).Build();

            connection?.StartAsync().ContinueWith((result) =>
            {
                logger.LogInformation(result.IsCompletedSuccessfully ? "Connected" : "Connection failed");
            });   
            return base.StartAsync(cancellationToken);  

        }

        public override async  Task StopAsync(CancellationToken cancellationToken)
        {
            await connection!.StopAsync(cancellationToken);
            await connection!.DisposeAsync();
            base.StopAsync(cancellationToken);  
        }
        protected override  Task ExecuteAsync(CancellationToken stoppingToken)
        {

            connection!.On<Product>("ReceiveTypedMessageForAllClient", (product) =>
            {
                logger.LogInformation($" Receive Message: {product.id} - {product.name} - {product.price} ");
            });

            return Task.CompletedTask;      

        }
    }
}

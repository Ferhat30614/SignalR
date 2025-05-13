using Microsoft.AspNetCore.SignalR;
using SignalR.Web.Models;

namespace SignalR.Web.Hubs
{
    public class ExampleTypeSafeHub:Hub<IExampleTypeSafeHub>
    {
        private static int ConnectedClientCount = 0;
        public async Task BroadcastMessageToAllClient(string message)
        {
            await Clients.All.ReceiveMessageForAllClient(message);
        }
        public async Task BroadcastTypedMessageToAllClient(Product product)
        {
            await Clients.All.ReceiveTypedMessageForAllClient(product); 
        }
        
        public async Task BroadcastMessageToCallerClient(string message)
        {
            await Clients.Caller.ReceiveMessageForCallerClient(message);
        }
        public async Task BroadcastMessageToOthersClient(string message)
        {
            await Clients.Others.ReceiveMessageForOthersClient(message);
        }
        public async Task BroadcastMessageToIndividualClient(string connectionId,string message)
        {
            await Clients.Client(connectionId).ReceiveMessageForIndividualClient(message);
        }

        public async Task BroadcastStreamDataToAllClient(IAsyncEnumerable<string> nameAsChunks)
        {
            await foreach (var chunk in nameAsChunks)
            {
                await Task.Delay(1000);
                await Clients.All.ReceiveMessageAsStreamForAllClient(chunk);
            }

        }
        public async Task BroadcastStreamProductToAllClient(IAsyncEnumerable<Product> productAsChunks)
        {
            await foreach (var chunk in productAsChunks)
            {
                await Task.Delay(1000);
                await Clients.All.ReceiveProductAsStreamForAllClient(chunk);
            }

        }

        public async Task BroadcastMessageToGroupClient(string groupName,string message)
        {
            await Clients.Group(groupName).ReceiveMessageForGroupClients(message);
        }
        
        public async Task AddGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.ReceiveMessageForCallerClient($"{groupName} grubuna dahil oldunuz");

            await Clients.Group(groupName).ReceiveMessageForGroupClients($" kullanıcı({Context.ConnectionId}) {groupName} grubuna dahil oldu ");
        }
        public async Task RemoveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.ReceiveMessageForCallerClient($"{groupName} grubundan çıktınız ");

            await Clients.Group(groupName).ReceiveMessageForGroupClients($" kullanıcı({Context.ConnectionId}) {groupName} grubundan ayrıldı ");
        }


        public override async Task OnConnectedAsync()
        {
            ConnectedClientCount++;
            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            await  base.OnConnectedAsync();
        }
        public override async  Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClientCount--;
            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            await base.OnDisconnectedAsync(exception);
        }
    }
}

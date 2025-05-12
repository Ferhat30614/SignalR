using Microsoft.AspNetCore.SignalR.Client;
using SignalRClientConsoleApp;

Console.WriteLine("hello signalR Console client");


var connection = new HubConnectionBuilder().WithUrl("https://localhost:7168/exampleTypeSafeHub").Build();

//await connection.StartAsync(); //eğer bu şekilde yazarsan await kaç dk sürerse sürsün bağlantının gerçekleşene kadar bu satırda bekler diğer kod bloguna geçmez...



connection.StartAsync().ContinueWith((result) =>
{
    Console.WriteLine(result.IsCompletedSuccessfully ? "Connected" : "Connection failed");
});  //bu kod ise bağlantı sonuclanınca ContinueWith kod bloğunu çalıştırır  ve o esnadada derleyici diğer satırlarda çalışır...

connection.On<Product>("ReceiveTypedMessageForAllClient", (product) =>
{
    Console.WriteLine($" Receive Message: {product.id} - {product.name} - {product.price} ");
});


while (true)
{
    var key= Console.ReadLine();
    if (key == "exist") break;


    var product = new Product(125, "Kitap", 138387);
    await connection.InvokeAsync("BroadcastTypedMessageToAllClient", product); // await ile yazınca, evet bu  işlem için bekleniyor ama programın arkada başka işleri varsa onları bloklamıyor. Mesela bu satır çalışırken  yukardaki connetion on tetiklenirse console yazma işlemi yeniden olur... yaaaa   alsana arka plan alsana awaitin faydası
    // yani await sırasında programın diğer asenkron parçaları (örneğin SignalR'dan gelen mesajları dinleyen On fonksiyonu) çalışmaya devam edebilir.




}





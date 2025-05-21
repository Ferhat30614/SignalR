var toastTimeout;



$(document).ready(function () {



    const connection = new window.signalR.HubConnectionBuilder().withUrl("/hub").build();

    connection.start().then(() => {

        console.log("Bağlantı sağlandı.");

    });


    connection.on("AlertCompleteFile", (downloadPath) => {

        clearTimeout(toastTimeout);

        //biz toast mesajı kapatmadık burda sadece timeoutu iptal ettik yani butona bastıktan 3 saniye sonra toast mesaj kapanmıcak ve aşşadaki kodlada toast mesajımın ben içeriğini değiştirmiş olcam sadce 

        $(".toast-body").html(`<p> Excel oluşturma işlemi tamamlanmıştır. Aşağıdaki link ile excel dosyasını indirebilirsiniz </p>
        <a href="${downloadPath}" >indir</a>
        `);

        $("#liveToast").show();






    });

});
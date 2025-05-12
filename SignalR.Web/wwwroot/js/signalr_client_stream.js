$(document).ready(function () {




    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub")
        .configureLogging(signalR.LogLevel.Information).build();

    async function start() {

        try {

            await connection.start().then(() => {
                /*$("#connectionId").html(`connectionId : ${connection.connectionId}`);*/
                console.log("hub ile bağlantı kuruldu")
            });

        }
        catch (err) {
            console.error("hub ile bağlantı kurulamadı", err);
            setTimeout(() => start(), 3000);
        }

    }

    connection.onclose(async () => {
         await  start();
    });

    start();

   



















})
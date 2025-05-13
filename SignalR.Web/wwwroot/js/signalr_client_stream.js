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


    const receiveMessageAsStreamForAllClient = "ReceiveMessageAsStreamForAllClient";
    const broadcastStreamDataToAllClient = "BroadcastStreamDataToAllClient";


    connection.on(receiveMessageAsStreamForAllClient, (name) => {

        $("#streamBox").append(`<p>${name}</p>`);
    })



    $("#btn_FromClient_ToHub").click(function () {

        const names = $("#txt_stream").val();

        const namesAsChunk = names.split(";");

        const subject = new signalR.Subject();

        connection.send(broadcastStreamDataToAllClient, subject).catch(err => console.error(err))

        namesAsChunk.forEach(name => {
            subject.next(name)
        });

        subject.complete();


    })












    start();

   



















})
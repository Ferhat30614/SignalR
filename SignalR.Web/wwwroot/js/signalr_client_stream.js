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

    const broadcastStreamProductToAllClient = "BroadcastStreamProductToAllClient";
    const receiveProductAsStreamForAllClient = "ReceiveProductAsStreamForAllClient";


    connection.on(receiveMessageAsStreamForAllClient, (name) => {

        $("#streamBox").append(`<p>${name}</p>`);
    })
    connection.on(receiveProductAsStreamForAllClient, (product) => {

        $("#streamBox").append(`<p>${product.id}-${product.name}-${product.price}-</p>`);
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

    $("#btn_FromClient_ToHub2").click(function () {

        var productList = [
            { id: 1, name:"araba",price:123000 },
            { id: 2, name:"ev",price:12200 },
            { id: 3, name:"yat",price:16330 }
        ] 

        const subject = new signalR.Subject();

        connection.send(broadcastStreamProductToAllClient , subject).catch(err => console.error(err))

        productList.forEach(product => {
            subject.next(product)
        });

        subject.complete();


    })












    start();

   



















})
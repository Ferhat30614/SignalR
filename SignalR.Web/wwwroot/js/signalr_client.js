$(document).ready(function () {



    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub")
        .configureLogging(signalR.LogLevel.Information).build();


    const broadcastMessageToAllClientHubMethodCall = "BroadcastMessageToAllClient";
    const receiveMessageForAllClientClientMethodCall = "ReceiveMessageForAllClient";


    const receiveMessageForCallerClient = "ReceiveMessageForCallerClient";
    const broadcastMessageToCallerClient = "BroadcastMessageToCallerClient";


    const receiveMessageForOthersClient = "ReceiveMessageForOthersClient";
    const broadcastMessageToOthersClient = "BroadcastMessageToOthersClient";


    const receiveMessageForIndividualClient = "ReceiveMessageForIndividualClient";
    const broadcastMessageToIndividualClient = "BroadcastMessageToIndividualClient";


    const receiveTypedMessageForAllClient = "ReceiveTypedMessageForAllClient";
    const broadcastTypedMessageToAllClient = "BroadcastTypedMessageToAllClient";


    const receiveConnectedClientCountAllClient = "ReceiveConnectedClientCountAllClient";


    const groupA = "GroupA";
    const groupB = "GroupB";
    let currentGroupList = [];

    function refreshGroupList() {

        $("#groupList").empty();

        currentGroupList.forEach(x => { 

            $("#groupList").append(`<p>${x}</p>`)

        })

    }


    $("#btn-groupA-add").click(function () {

        if (currentGroupList.includes(groupA)) return;

        connection.invoke("AddGroup", groupA).then(() => {

            currentGroupList.push(groupA);
            refreshGroupList();

        })


    })

    $("#btn-groupA-remove").click(function () {

        if (!currentGroupList.includes(groupA)) return;

        connection.invoke("RemoveGroup", groupA).then(() => {

            currentGroupList = currentGroupList.filter(x => x !== groupA);
            refreshGroupList();

        })

    })

    $("#btn-groupB-add").click(function () {

        if (currentGroupList.includes(groupB)) return;
        connection.invoke("AddGroup", groupB).then(() => {

            currentGroupList.push(groupB);
            refreshGroupList();

        })

    })

    $("#btn-groupB-remove").click(function () {

        if (!currentGroupList.includes(groupB)) return;

        connection.invoke("RemoveGroup", groupB).then(() => {

            currentGroupList = currentGroupList.filter(x => x !== groupB);
            refreshGroupList();

        })

    })


    $("#btn-groupA-send-message").click(function () {

        const message="GoupA Mesajıdır"

        connection.invoke("BroadcastMessageToGroupClient", groupA, message)
        console.log("mesaj Gönderildi");

    })

    $("#btn-groupB-send-message").click(function () {

        const message="GoupB Mesajıdır"

        connection.invoke("BroadcastMessageToGroupClient", groupB, message)
        console.log("mesaj Gönderildi");

    })                      


    function start() {
        connection.start().then(() => {

            $("#connectionId").html(`connectionId : ${connection.connectionId}`);


            console.log("hub ile bağlantı kuruldu")
        });
    }

    try{
        start();
    }
    catch {
        setTimeout(() => start(),5000);
    }


    const span_client_count = $("#span-connected-client-count"); //burda benim spanime eriştim bnu şekilde
    connection.on(receiveConnectedClientCountAllClient, (count) => {
        span_client_count.text(count);

        console.log("Bağlı kullanıcı sayısı", count);
    });



    connection.on(receiveMessageForAllClientClientMethodCall, (message) => {
        console.log("Gelen mesaj",message);
    });
    connection.on(receiveTypedMessageForAllClient, (product) => {
        console.log("Gelen ürün ", product);
    });


    connection.on(receiveMessageForCallerClient, (message) => {
        console.log("(Caller) Gelen mesaj",message);
    });



    connection.on(receiveMessageForOthersClient, (message) => {
        console.log("(Others) Gelen mesaj",message);
    });

    connection.on(receiveMessageForIndividualClient, (message) => {
        console.log("(Indıvıdual) Gelen mesaj",message);
    });

    connection.on("ReceiveMessageForGroupClients", (message) => {
        console.log(" Gelen mesaj",message);
    });

    $("#btn-send-message-all-client").click(function () {
        const message = "hello world";
        connection.invoke(broadcastMessageToAllClientHubMethodCall, message)
        console.log("mesaj Gönderildi");

    })

    $("#btn-send-typed-message-all-client").click(function () {
        const product = { id: 1, name: "ferhat", price: 123 };
        connection.invoke(broadcastTypedMessageToAllClient, product)
        console.log(" ürün Gönderildi");

    })

    $("#btn-send-message-caller-client").click(function () {
        const message = "hello world";
        connection.invoke(broadcastMessageToCallerClient, message)
        console.log("mesaj Gönderildi");

    })

    $("#btn-send-message-others-client").click(function () {
        const message = "hello world";
        connection.invoke(broadcastMessageToOthersClient, message)
        console.log("mesaj Gönderildi");

    })

    $("#btn-send-message-individual-client").click(function () {
        const message = "hello world";
        const connectionId = $("#text-connectionId").val();
        connection.invoke(broadcastMessageToIndividualClient, connectionId, message)
        console.log("mesaj Gönderildi");

    })





})
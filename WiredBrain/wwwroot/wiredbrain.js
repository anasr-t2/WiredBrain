//import { signalR } from "./signalr";



let connection = null;
WebSocket = undefined;
EventSource = undefined;
setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/coffeehub")
        .build();

    connection.on("ReceiveOrderUpdate", (update) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = update;
    });

    connection.on("NewOrder", (order) => {
        const statusDiv = document.getElementById("othersOrders");
        statusDiv.innerHTML = "Someone ordered " + order.product + ", size: " + order.size;
    });

    connection.on("GetCurrentConnections", (connections) => {
        const connectionsList = document.getElementById("user-list");
        connectionsList.innerHTML = "";
        connections.forEach(connection => connectionsList.innerHTML += `<li> ${connection} </li>`)
        console.log(connections);
    });

    connection.on("Finished", () => {
        connection.stop();
    });

    connection.start()
        .catch(err => console.error(err.toString())); 
};

setupConnection();


document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    const product = document.getElementById("product").value;
    const size = document.getElementById("size").value;

    fetch("/Coffee",
        {
            method: "POST",
            body: JSON.stringify({ product, size }),
            headers: {
                'content-type': 'application/json'
            }
        })
        .then(response => response.text())
        .then(id => {
            console.log(id);
            connection.invoke("GetUpdatesForOrder", id)
        });
});

document.getElementById("show-all-connections").addEventListener("click", e => {
        
    
});
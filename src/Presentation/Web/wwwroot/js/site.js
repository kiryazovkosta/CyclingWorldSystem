// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

setTimeout(function () {
    $('#alert').alert('close');
}, 5000);

let connection = null;
setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/activityhub")
        .build();

    connection.on("NotifyActivityCreateAsync", (message) => {
        let messageContainer = document.getElementById("signalr-message-section");
        if (messageContainer !== undefined && messageContainer != null) {
            messageContainer.innerHTML = '';
            let alertContainer = document.createElement('div');
            alertContainer.id = "alert-dialog";
            alertContainer.className = "alert alert-primary";
            let h3 = document.createElement('h3');
            h3.innerText = message;
            alertContainer.appendChild(h3);
            messageContainer.appendChild(alertContainer);

            setTimeout(() => {
                let alertDialog = document.getElementById("alert-dialog");
                if (alertDialog) {
                    alertDialog.remove();
                }
            }, 10000);
        }
    });

    connection
        .start()
        .catch(err => console.error(err.toString()));
};

setupConnection();


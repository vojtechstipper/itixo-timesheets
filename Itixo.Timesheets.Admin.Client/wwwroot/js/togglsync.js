var connection = new signalR.HubConnectionBuilder()
    .withUrl("/togglsynchub")
    .build();

connection.on("TogglSyncFinished", function (msg) {
    toast_show_success(msg);
    ReactAfterSyncEnd();
    document.title = msg;
});

connection.on("TogglSyncError", function (msg) {
    toast_show_danger(msg);
    ReactAfterSyncEnd();
});

connection.on("TogglSyncStart", function (msg) {
    toast_show_notification(msg);
    ReactAfterSyncStart();
    document.title = msg;
});

function ReactAfterSyncEnd() {

    $("#btnTriggerSync").prop("disabled", false);
    $(".synch-loader").hide();
}

function ReactAfterSyncStart() {
    $("#btnTriggerSync").prop("disabled", true);
    $(".synch-loader").show();
}

connection.start().then(function () {

   connection.invoke("RegisterUser").catch(function (err) {
        return console.error(err.toString());
    });

}).catch(function (err) {
    return console.error(err.toString());
});

if (document.getElementById("btnTriggerSync") != null) {
    document.getElementById("btnTriggerSync").addEventListener("click", function (event) {

        ReactAfterSyncStart();

        //connection.invoke("TriggerTogglSync",
        //    dotvvm.viewModels.root.viewModel.TriggerSynchronizationDto().StartDate(),
        //    dotvvm.viewModels.root.viewModel.TriggerSynchronizationDto().EndDate()).catch(function (err) {
        //        return console.error(err.toString());
        //    });
        //event.preventDefault();
    });
}

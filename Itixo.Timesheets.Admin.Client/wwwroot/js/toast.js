function toast_show_success(msg) {
    $("div[id^='notify-']").hide();
    $("#notify-success").show();
    $(".toast").toast("show");
    $(".toast-body").html(msg);
}

function toast_show_danger(msg) {
    $("div[id^='notify-']").hide();
    $("#notify-danger").show();
    $(".toast").toast("show");
    $(".toast-body").html(msg);
}

function toast_show_notification(msg) {
    $("div[id^='notify-']").hide();
    $("#notify-primary").show();
    $(".toast").toast("show");
    $(".toast-body").html(msg);
}
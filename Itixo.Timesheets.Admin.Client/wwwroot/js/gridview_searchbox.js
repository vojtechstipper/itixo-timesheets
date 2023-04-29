$(document).ready(function () {

    $('input[type=search]').on('search', function (e) {
        $('#btnfilter').click();
    });
});

var searchboxHandler = function (args) {

    $('input[type=search]').on('search', function (e) {
        $('#btnfilter').click();
    });

}

dotvvm.events.afterPostback.subscribe(searchboxHandler);
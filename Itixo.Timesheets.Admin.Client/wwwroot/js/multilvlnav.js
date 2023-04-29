// Prevent closing from click inside dropdown
$(document).on('click', '.dropdown-item', function (e) {
    window.location = $(this).attr("href");
});

// make it as accordion for smaller screens
if ($(window).width() < 992) {
    $('.dropdown-menu a').click(function (e) {
        e.preventDefault();
        if ($(this).next('.submenu').length) {
            $(this).next('.submenu').toggle();
        }
        $('.dropdown').on('hide.bs.dropdown', function () {
            $(this).find('.submenu').hide();
        })
    });
}
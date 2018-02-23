$(function() {

    $(window).scroll(function() {
        if ($(this).scrollTop() > 100) {
            $(".totop").fadeIn()
        } else {
            $(".totop").fadeOut()
        }
    });
    $(".totop").on("click", function() {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false
    });

    $(".simple-mail").click(function () {
        showFormInDialog(1);
        return false;
    });
});


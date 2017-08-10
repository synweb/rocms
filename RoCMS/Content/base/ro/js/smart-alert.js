smartAlert = function (text, timeout) {
    $(".smart-alert .alert-message").html(text);
    $(".smart-alert").hide();
    $(".smart-alert").fadeToggle(1000, 'easeOutExpo');
    var hideTimeout;
    if (timeout === 0) {
        return;
    }
    if (timeout) {
        hideTimeout = timeout*1000;
    } else {
        hideTimeout = 15*1000;
    }
    setTimeout(function () {
        if ($(".smart-alert").is(":visible")) {
            $(".smart-alert").fadeToggle(1000, 'easeOutExpo');
        }
    }, hideTimeout);

}

$(function() {
    $(".smart-alert .alert-hide").click(function() {
        $(".smart-alert").fadeToggle(1000, 'easeOutExpo');
        return false;
    });
});
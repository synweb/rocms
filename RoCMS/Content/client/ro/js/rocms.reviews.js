var SEND_REVIEW_URL = "/Review/CreateReview";

function showReviewDialog(url) {
    var opts = { url: url, id: 0, moderated: false, doneFunction: function () { } };
    showReviewDialogObj(opts);
}

function clearReviewContainer(container) {
    container.find(".review-author").val("");
    container.find(".review-city").val("");
    container.find(".review-email").val("");
    container.find(".review-text").val("");
    container.find(".review-vk").val("");
}

function sendReview(container, onSuccess, onSendAlways) {
    var url = SEND_REVIEW_URL;
    var $form = container.find("form");
    $.validator.unobtrusive.parse($form, true);
    if ($form.valid()) {


        var vkId = '';
        var vk = /(?:^(?:https?:\/\/)?vk.com\/)([a-zA-Z0-9\-_]+)(?:&?.*)/
            .exec(container.find(".review-vk").val());
        if (vk && vk[1]) {
            vkId = vk[1];
        }

        var message = {
            Author: container.find(".review-author").val(),
            Email: container.find(".review-email").val(),
            Text: container.find(".review-text").val(),
            VK: vkId
        };
        $.ajax({
            url: url,
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(message)
        }).done(function () {
            if (onSuccess) {
                onSuccess();
            }
        }).always(function () {
            if (onSendAlways) {
                onSendAlways();
            }
        });
    } else {
        $form.validate().focusInvalid();
        onSendAlways();
    }
}



//$(function () {
//    $(".add-review").click(function () {
//        var url = SEND_REVIEW_URL;
//        showReviewDialog(url);
//        return false;
//    });
//});
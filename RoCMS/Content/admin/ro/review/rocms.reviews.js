/// <reference path="../../../base/vendor/jquery/core/jquery-2.0.2.js" />
var SEND_REVIEW_URL = "/Review/CreateReview";

function showReviewDialog(url) {
    var opts = { url: url, id: 0, moderated: false, doneFunction: function () {}};
    showReviewDialogObj(opts);
}

function clearReviewContainer(container) {
    container.find(".review-author").val("");
    container.find(".review-city").val("");
    container.find(".review-email").val("");
    container.find(".review-text").val("");
    container.find(".review-response").val("");
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
        if(vk && vk[1]) {
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

//TODO: убрать дублирование между диалоговой формой и обычной, вынести редактор админки в отдельный функционал
function showReviewDialogObj(opts) {
    var options = {
        title: 'Отзыв',
        width: 600,
        modal: true,
        draggable: false,
        resizable: false,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var $div = $dialog.find("form");
            $.validator.unobtrusive.parse($div, true);
        },
        buttons: [
			{
			    text: "Отправить",
			    click: function () {
			        var $dialog = $(this).dialog("widget");
			        var that = this;
			        var $div = $dialog.find("form");
			        $.validator.unobtrusive.parse($div, true);
			        if ($div.valid()) {
			            var message = {
			                Response: $dialog.find(".review-response").val(),
			                Author: $dialog.find(".review-author").val(),
			                City: $dialog.find(".review-city").val(),
			                Email: $dialog.find(".review-email").val(),
			                Text: $dialog.find(".review-text").val(),
			                ReviewId: opts.id,
			                Moderated: opts.moderated,
			            };
			            var url = (opts.id > 0 ? opts.url : "/Admin/AddReview/");
			            postJSON(url,
			                message,
			                function() {

			                    $(that).dialog("close");
			                    opts.doneFunction();
			                    location.reload();
			                });
			        } else {
			            $div.validate().focusInvalid();
			        }
			    }
			},
			{
			    text: "Отмена",
			    click: function () {
			        $(this).dialog("close");
			    }
			}
        ]
    };
    showDialogFromUrl(opts.url, options);
}

$(function () {
    $(".add-review").click(function () {
        var url = SEND_REVIEW_URL;
        showReviewDialog(url);
        return false;
    });



    $('#adminContent').on("click", ".admin-reviews .button-edit", function () {
        var reviewId = $(this).parent().data("reviewId");
        //var container = $(this).closest(".admin-review").find(".review-container");
        var editUrl = "/Admin/EditReview/" + reviewId;
        //var getUrl = "/Admin/GetReview/" + reviewId;
        var moderated = $(this).closest(".admin-review").find(".switch-on").length > 0;

        showReviewDialogObj({
            id: reviewId,
            moderated: moderated,
            doneFunction: function () {
                //container.load(getUrl);
            },
            url: editUrl
        });
        return false;
    });

    $('#adminContent').on("click", ".admin-reviews .button-create", function () {
        var createUrl = "/Admin/CreateReview/";
        showReviewDialog(createUrl);
        return false;
    });

    $('#adminContent').on("click", ".admin-reviews .button-delete", function () {
        if (!confirmRemoval()) { return false; }
        var reviewId = $(this).parent().data("reviewId");
        var container = $(this).closest(".admin-review");
        var url = "/api/review/" + reviewId + "/delete";
        blockUI();
        postJSON(url, "", function (data) {
            if (data.succeed) {
                container.hide(1000, function () { container.remove(); });
            } else {
                if (data.message) {
                    alert(message);
                }
            }
        }).fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
            .always(function () {
                unblockUI();
            });
        return false;
    });
});
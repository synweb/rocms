/// <reference path="../../../base/ro/js/rocms.app.js" />
/// <reference path="../../../base/ro/js/rocms.helpers.js" />
App.Admin = {};

ko.validation.init({
    decorateInputElement: true,
    errorElementClass: "input-validation-error"

});


$(function () {

    $("#adminContent").on("click", ".main-admin-menu button", function() {
        window.location.href = $(this).data("href");
    });
   
    $("#adminContent").on("click", ".slide-create", function () {
        var sliderId = $(this).parents(".slider-config").data("sliderId");
        showSlideCreateEditDialog("create", sliderId, function () {
            location.reload();
        });
        return false;
    });
    
    $("#adminContent").on("click", ".slide-edit", function () {
        var slideId = $(".slider-slides option:selected").val();
        showSlideCreateEditDialog("edit", slideId, function () {
            location.reload();
        });
        return false;
    });
    
    
    
    $('#adminContent').on("click", ".slider .delete", function () {
        var url = $(this).attr("href");
        var self = this;
        blockUI();
        postJSON(url, "", function () {
            $(self).closest(".album").remove();
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
        return false;
    });

    $('#adminContent').on("click", ".delete-img-overlay", function () {
        var sliderId = $(".slider-images").data("slider-id");
        var imgId = $(this).parents(".slider-image, .gallery-image").data("img-id");
        var addLinks = $(".slider-image[data-img-id='" + imgId + "'] .add-img-overlay, .gallery-image[data-img-id='" + imgId + "'] .add-img-overlay");
        var removeLinks = $(".slider-image[data-img-id='" + imgId + "'] .delete-img-overlay, .gallery-image[data-img-id='" + imgId + "'] .delete-img-overlay");
        var url = actionUrls.Slider_RemoveImage;
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ sliderId: sliderId, imageId: imgId }),
            contentType: "application/json",
            success: function () {
                addLinks.show(1000);
                removeLinks.hide(1000);
                var selector = ".slider-image[data-img-id='" + imgId + "']";
                $(selector).parents('li').hide(1000, function () { $(selector).parents('li').remove(); });
            }
        });
        return false;
    });
    
    $('#adminContent').on("click", ".add-img-overlay", function () {
        var sliderId = $(".slider-images").data("slider-id");
        var imgId = $(this).parents(".slider-image, .gallery-image").data("img-id");
        var addLinks = $(".slider-image[data-img-id='" + imgId + "']  .add-img-overlay, .gallery-image[data-img-id='" + imgId + "'] .add-img-overlay");
        var removeLinks = $(".slider-image[data-img-id='" + imgId + "'] .delete-img-overlay, .gallery-image[data-img-id='" + imgId + "'] .delete-img-overlay");
        var url = actionUrls.Slider_AddImage;
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ sliderId: sliderId, imageId: imgId }),
            contentType: "application/json",
            success: function () {
                addLinks.hide(1000);
                removeLinks.show(1000);
                var listItem = sliderTemplate.replace('_id_', imgId).replace('_id_', imgId);
                $(".slider-image-list").append(listItem);
                $("li[style*='display']").show(1000);
            }
        });
        
        return false;
    });

    
    $('#adminContent').on("click", ".change-pass-link", function () {
        $('.change-pass').show();
        $(this).hide();
        return false;
    });
  

    $('#adminContent').on("click", ".save-password", function () {
        var oldpass = $('.old-pass').val();
        var newpass = $('.new-pass').val();
        var repeatpass = $('.repeat-pass').val();
        if (oldpass && newpass && repeatpass && newpass === repeatpass) {
            blockUI();
            $.ajax({
                url: '/Admin/ChangePassword',
                type: 'POST',
                data: JSON.stringify({
                    oldPassword: oldpass,
                    newPassword: newpass,
                    repeatPassword: repeatpass
                }),
                contentType: "application/json",
                success: function(data) {
                    $('.change-pass').hide();
                    $(".change-pass-link").show();
                }
            }).always(function() {
                unblockUI();
                var oldpass = $('.old-pass').val("");
                var newpass = $('.new-pass').val("");
                var repeatpass = $('.repeat-pass').val("");
            }).fail(function () {
                alert("Произошла ошибка, проверьте правильность ввода.");
            });
        }
        return false;
    });
});

function loaded() {
    $.validator.unobtrusive.parse($("#adminContent"));
}

function openPagePreview() {
    $("#previewBlock").css('visibility', 'visible');
}


//function showReviewDialogObj(opts) {
//    var options = {
//        title: 'Отзыв',
//        width: 500,
//        modal: true,
//        draggable: false,
//        resizable: false,
//        open: function () {
//            var $dialog = $(this).dialog("widget");
//            var $div = $dialog.find("form");
//            $.validator.unobtrusive.parse($div, true);
//        },
//        buttons: [
//			{
//			    text: "Отправить",
//			    click: function () {
//			        var $dialog = $(this).dialog("widget");
//			        var that = this;
//			        var $div = $dialog.find("form");
//			        $.validator.unobtrusive.parse($div, true);
//			        if ($div.valid()) {
//			            var message = {
//			                Author: $dialog.find(".review-author").val(),
//			                City: $dialog.find(".review-city").val(),
//			                Email: $dialog.find(".review-email").val(),
//			                Text: $dialog.find(".review-text").val(),
//			                ReviewId: opts.id,
//			                Moderated: opts.moderated,
//			            };
//			            $.ajax({
//			                url: (opts.id > 0 ? opts.url : "/Admin/AddReview/"),
//			                dataType: 'json',
//			                type: 'POST',
//			                contentType: 'application/json; charset=utf-8',
//			                data: JSON.stringify(message)
//			            }).done(function () {
//			                $(that).dialog("close");
//			                opts.doneFunction();
//			                location.reload();
//			            });
//			        } else {
//			            $div.validate().focusInvalid();
//			        }
//			    }
//			},
//			{
//			    text: "Отмена",
//			    click: function () {
//			        $(this).dialog("close");
//			    }
//			}
//        ]
//    };
//    showDialogFromUrl(opts.url, options);
//}

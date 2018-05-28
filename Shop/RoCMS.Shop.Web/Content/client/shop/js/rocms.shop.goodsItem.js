function goodsReviewLoaded(heartId) {
    var ratingField = $(".leave-goods-review-container-form input.review-rating-value");
    $("ul.rating li > a").click(function () {
        var rating = $("ul.rating");
        rating.toggleClass("onestar", false);
        rating.toggleClass("twostar", false);
        rating.toggleClass("threestar", false);
        rating.toggleClass("fourstar", false);
        rating.toggleClass("fivestar", false);
        var value = parseInt($(this).text());
        ratingField.val(value);
        switch (value) {
            
            case 1:
                rating.toggleClass("onestar", true);
                break;
            case 2:
                rating.toggleClass("twostar", true);
                break;
            case 3:
                rating.toggleClass("threestar", true);
                break;
            case 4:
                rating.toggleClass("fourstar", true);
                break;
            case 5:
                rating.toggleClass("fivestar", true);
                break;
        }
    });

    $("button.review-send").click(function () {
        var authorField = $("input.review-author");
        var author = authorField.val();
        var contactsField = $("input.review-contacts");
        var contacts = contactsField.val();
        var textField = $("textarea.review-text");
        var text = textField.val();
        
        var rating = ratingField.val();


        postJSON("/api/shop/goods/reviews/create", {
            author: author,
            text: text,
            rating: rating,
            authorContact: contacts,
            heartId: heartId
        }, function () {
            authorField.val("");
            contactsField.val("");
            textField.val("");
            ratingField.val("");
            $("form.leave-goods-review-container-form").html("<span class='review-sent'>Спасибо за отзыв!</span>");
        });
    });
};


function createGoodsAwaitingDialog(heartId, onSuccess) {


        showBootstrapDialogFromUrl("/Shop/GoodsAwaitingDialog", {
            removeButtons: true,
            onBeforeShow: function () {
                var $dialog = $(this);
                $dialog.find(".modal-dialog").addClass("modal-md");
            },
            onShow: function () {
                var $dialog = $(this);
                $dialog.find(".create-awaiting").click(function () {
                    var mail = $dialog.find(".email").val();
                    var phone = $dialog.find(".phone").val();

                    if ($.trim(mail) || $.trim(phone)) {
                        postJSON("/api/shop/goods/awaiting/create", { email: mail, phone: phone, heartId: heartId }, function(data) {
                            if (data.succeed !== false) {
                                $dialog.modal("hide");
                                if (onSuccess) {
                                    onSuccess();
                                }
                            } else {
                                alert("Неправильное имя или пароль пользователя");
                            }

                        });
                    } else {
                        $dialog.find(".error-info").show();
                    }
                });

            }
        });
        

}


function goodsLoaded() {

    $(".btn-awaiting").click(function() {
        var self = $(this);
        var heartId = $(this).data("heartId");
        createGoodsAwaitingDialog(heartId, function() {
            self.replaceWith($("<span>Товар добавлен в лист ожидания.</span>"));
        });

        return false;
    });
}
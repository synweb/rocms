$(function () {

    $('#adminContent').on("click", ".mail-mesages-summary .button-delete", function () {
        if (!confirmRemoval()) {
            return false;
        }
        var container = $(this).parents(".mail-mesages-summary");
        var id = container.data("blockId");
        var url = "/api/email/message/" + id + "/delete";
        blockUI();
        postJSON(url, "", function () {
            container.hide(1000, function () { container.remove(); });           
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
        return false;
    });

    $('#adminContent').on("click", ".mail-mesages-summary .button-resend", function () {
        
        var container = $(this).parents(".mail-mesages-summary");
        var id = container.data("blockId");        
        var url = "/api/email/message/" + id + "/resend";
        blockUI();
        
        postJSON(url, "", function (result) {
            $("span.sent-mail-result", container).text(result.data);

            if (result.succeed) {
                $("span.sent-mail-result", container).removeClass("mail-not-sent").addClass("mail-sent");
                $("div.mail-error", container).hide();
                $("p.mail-error-text", container).text("");
            }
            else {                
                $("span.sent-mail-result", container).removeClass("mail-sent").addClass("mail-not-sent");
                $("div.mail-error", container).show();
                $("p.mail-error-text", container).text(result.message);
            }            
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
        return false;
    });

});
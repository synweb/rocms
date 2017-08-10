emailTemplateEditorLoaded = function () {
    $("#adminContent").on("click", ".button-accept", function () {
        blockUI();
        var name = $(".email-template-editor").data("name");
        var url = "/api/email/template/" + name + "/update";
        var html = getTextFromEditor('template_editor');
        postJSON(url, { html: html }, function (result) {
            if (result.succeed) {
            } else {
                if (result.errorType) {
                    alert("Произошла ошибка");
                }
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
}
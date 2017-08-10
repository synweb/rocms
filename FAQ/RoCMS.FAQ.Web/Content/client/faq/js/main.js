function askQuestionFormLoaded() {
    $(".ask-question-form button").on("click", function () {
        var $form = $(this).closest(".ask-question-form");

        $.validator.unobtrusive.parse($form, true);

        if ($form.valid()) {
            $(this).attr("disabled", "disabled");
            var data = {
                AuthorName: $form.find(".name").val(),
                AuthorEmail: $form.find(".email").val(),
                QuestionText: $form.find(".question").val(),
            }
            postJSON('/api/faq/question/ask', data, function (res) {
                if (res.succeed) {
                    $form.find(".name").val("");
                    $form.find(".email").val("");
                    $form.find(".question").val("");
                    $("p.question-asked").show(1000);
                }
                $(this).removeAttr("disabled");
            });
        } else {
            $form.validate().focusInvalid();
        }
    });
}
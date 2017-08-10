function questionListLoaded() {
    $('.moderated-switch').bootstrapSwitch('setOnLabel', '<i class="fa fa-eye"></i>');
    $('.moderated-switch').bootstrapSwitch('setOffLabel', '<i class="fa fa-eye-slash"></i>');
    $('.moderated-switch').bootstrapSwitch('setSizeClass', 'switch-mini');


    $('.moderated-switch').on('switch-change', function (e, data) {
        var id = $(this).closest("li").data("id");
        var url;
        if (data.value === true) {
            url = "/api/faq/question/" + id + "/accept";
        } else {
            url = "/api/faq/question/" + id + "/hide";
        }
        postJSON(url, null, function (res) {
            if (!res.succeed) {
                smartAlert("Произошла ошибка");
            }
        });
        return false;
    });


    $(".question-list .delete").on("click", function() {
        var $li = $(this).closest("li");
        var id = $li.data("id");
        var url = "/api/faq/question/" + id + "/delete";
        postJSON(url, null, function (res) {
            if (res.succeed) {
                $li.hide(1000);
            }
            else {
                smartAlert("Произошла ошибка");
            }
        });
    });

}


function questionEditorLoaded() {
    createCKEditor("question_content");

    $(".button-create").on("click", function () {
        var data = {
            questionText: $("#QuestionText").val(),
            answerText: getTextFromEditor('question_content'),
        }
        postJSON("/api/faq/question/create", data, function (res) {
            if (res.succeed) {
                var id = res.data;
                window.location.href = "/FAQEditor/EditQuestion/" + id;
            } else {
                smartAlert("Произошла ошибка");
            }
        });
    });

    $(".button-accept").on("click", function () {
        var $form = $("form.question-editor");
        var data = {
            questionId: $form.data("id"),
            moderated: $form.data("moderated"),
            answerSentToAuthor: $form.data("sent"),
            questionText: $("#QuestionText").val(),
            answerText: getTextFromEditor('question_content'),
        }
        postJSON("/api/faq/question/update", data, function(res) {
            if (res.succeed) {
                smartAlert("Вопрос сохранён");
            } else {
                smartAlert("Произошла ошибка");
            }
        });
    });

    $(".button-send-answer").on("click", function () {
        var $form = $("form.question-editor");
        var data = {
            questionId: $form.data("id"),
            moderated: $form.data("moderated"),
            answerSentToAuthor: $form.data("sent"),
            questionText: $("#QuestionText").val(),
            answerText: getTextFromEditor('question_content'),
        }
        postJSON("/api/faq/question/update-send", data, function (res) {
            if (res.succeed) {
                smartAlert("Вопрос сохранён и отправлен");
                $("p.answer-sent-status").html("Ответ отправлен автору.");
            } else {
                smartAlert("Произошла ошибка");
            }
        });
    });
}
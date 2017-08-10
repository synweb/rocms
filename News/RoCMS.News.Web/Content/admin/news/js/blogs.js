function blogEditorLoaded() {
    $("form.personal-blog-editor").on("submit", function () {
        var $form = $(this);
        $.validator.unobtrusive.parse($form, true);

        if ($form.valid()) {
            blockUI();
            var data = {
                blogId: $form.find("input[name=blogId]").val(),
                ownerId: $form.find("input[name=ownerId]").val(),
                relativeUrl: $form.find("input[name=relativeUrl]").val(),
                title: $form.find("input[name=title]").val(),
                subtitle: $form.find("input[name=subtitle]").val()
            }
            postJSON("/api/news/blog/admin/update", data, function (res) {
                if (res.succeed) {
                    smartAlert("Данные успешно обновлены");
                } else {
                    smartAlert("Произошла ошибка, проверьте правильность заполнения формы");
                }
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        } else {
            $form.validate().focusInvalid();
        }
        return false;
    });
}

function blogListEditorLoaded() {
    $(".button-delete").click(function () {
        if (!confirmRemoval()) {
            return false;
        }
        var box = $(this).closest(".blog-summary");
        var id = box.data("blogId");
        if (!id) return false;
        blockUI();
        $.post("/api/news/blog/" + id + "/delete", null, function (res) {
            if (res.succeed) {
                box.hide(1000, function () { box.remove(); });
            } else {
                smartAlert("Ошибка при удалении");
            }
        }).always(unblockUI);
    });
}


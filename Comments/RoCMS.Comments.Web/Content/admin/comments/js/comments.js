onCommentsEditorLoaded = function() {
    $(".comments-tree").on("click", ".btn-delete", function() {
        if (confirm("Вы уверены, что хотите удалить комментарий?")) {
            blockUI();
            var deletedTemplate = $("#deleted-comment-template").html();
            var $container = $(this).closest(".comment-container");
            var id = $container.data("id");
            postJSON("/api/comments/comment/" + id + "/delete", null, function (res) {
                if (res.succeed) {
                    $container.html(deletedTemplate);
                } else {
                    alert("Произошла ошибка");
                }
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        }
        return false;
    });

    $(".comments-tree").on("click", ".btn-moderate-hide", function () {
        blockUI();
        var template = $("#btn-moderate-show-template").html();
        var $commentContainer = $(this).closest(".comment-container");
        var id = $commentContainer.data("id");
        var $btnContainer = $(this).closest("span.comment-moderate");
        postJSON("/api/comments/comment/" + id + "/hide", null, function (res) {
            if (res.succeed) {
                $btnContainer.html(template);
            } else {
                alert("Произошла ошибка");
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

    $(".comments-tree").on("click", ".btn-moderate-show", function () {
        blockUI();
        var template = $("#btn-moderate-hide-template").html();
        var $commentContainer = $(this).closest(".comment-container");
        var id = $commentContainer.data("id");
        var $btnContainer = $(this).closest("span.comment-moderate");
        postJSON("/api/comments/comment/" + id + "/show", null, function (res) {
            if (res.succeed) {
                $btnContainer.html(template);
            } else {
                alert("Произошла ошибка");
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
function clientBlogEditorLoaded() {$('[data-toggle="tooltip"]').tooltip();
    $("[data-val-urlexists]").keyup(function () { return false });

    $("form.personal-blog-editor").on("submit", function () {
        var $form = $(this);
        $.validator.unobtrusive.parse($form, true);

        if ($form.valid()) {
            var data = {
                blogId: $form.find("input[name=blogId]").val(),
                relativeUrl: $form.find("input[name=relativeUrl]").val(),
                title: $form.find("input[name=title]").val(),
                subtitle: $form.find("input[name=subtitle]").val(),
            }
            if (data.blogId !== "0") {
                postJSON("/api/news/blog/client/update", data, function (res) {
                    if (res.succeed) {
                        window.location.href = "/personal";
                    }
                });
            } else {
                postJSON("/api/news/blog/client/create", data, function(res) {
                    if (res.succeed) {
                        window.location.href = "/personal";
                    }
                });
            }
        } else {
            $form.validate().focusInvalid();
        }
        return false;
    });


    $("#userProfile .btn-save-profile").click(function () {
        var $form = $("#userProfile");
        $.validator.unobtrusive.parse($form, true);

        if ($form.valid()) {

            blockUI();
            var data = $form.serializeObject();

            postJSON("/api/news/blog/profile/update", data, function (result) {
                if (result.succeed) {
                    //smartAlert("Данные успешно обновлены");
                } else {
                    //smartAlert("Произошла ошибка, проверьте данные");
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
}

function getTextFromEditor(id) {
    var e = CKEDITOR.instances[id];
    if (e) {
        return e.getData();
    } else {
        return $("#"+id).val();
    }
}



function clientBlogItemEditorLoaded() {
    saveData = function ($form) {
        var data = {
            newsId: $form.find("input[name=newsId]").val(),
            title: $form.find("input[name=title]").val(),
            description: $form.find("textarea[name=description]").val(),
            imageId: $form.find("input[name=imageId]").val(),
            text: getTextFromEditor('textarea_id'),
        }
        alert(data.newsId); 
        if (data.newsId !== "0") {
            postJSON("/api/news/blog/client/post/update", data, function (res) {
                if (res.succeed) {
                    //window.location.href = "/personal";
                }
            });
        } else {
            postJSON("/api/news/blog/client/post/create", data, function (res) {
                if (res.succeed) {
                    //window.location.href = "/personal";
                }
            });
        }
    }

    $("form.post-editor").on("submit", function () {
        var $form = $(this);
        $.validator.unobtrusive.parse($form, true);
        
        if ($form.valid()) {
            if ($("#fileupload").val() != '') {
                // there is a file awaiting
                $("input[name=fileData]").data().submit().always(function() {saveData($form);});
            } else {
                saveData($form);
            }

        } else {
            $form.validate().focusInvalid();
        }
        return false;
    });


    $(".tools .delete-post").click(function() {
        if (confirm("Вы уверены, что хотите удалить этот пост?")) {
            var newsId = $(this).data("newsId");
            postJSON("/api/news/blog/client/post/" + newsId + "/delete", "", function(result) {
                if (result.succeed) {
                    location.href = "/personal";
                } else {
                    smartAlert("Произошла ошибка, попробуйте еще раз");
                }
            });
        }
        return false;
    });
    
}
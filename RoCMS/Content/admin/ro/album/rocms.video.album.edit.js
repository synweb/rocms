function parseYoutubeVideo(url) {
    var id = /(?:^(?:https?:\/\/)?(?:www.)?youtube.com\/?(?:.*)v=)([a-zA-Z0-9\-_]+)(?:&?.*)/
                .exec(url);
    if (!id) {
        id = /(?:^(?:https?:\/\/)?youtu.be\/)([a-zA-Z0-9\-_]+)/.exec(url);
    }
    if (id && id[1]) {
        return id[1];
    }
    return null;
}

function videoAlbumEditorLoaded() {
    $("#albumName").change(function () {
        if ($(this).val()) {
            blockUI();
            postJSON("/api/album/" + $(this).data("albumId") + "/title/" + $(this).val() + "/update")
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        } else {
            $(this).focus();
        }
    });

    $(".video-gallery .delete button").click(function (e) {
        if (confirmRemoval()) {
            var that = $(this);
            blockUI();
            var id = that.data("id");
            postJSON("/api/video/" + id + "/delete").always(function () {
                var container = that.parents(".fade.in.row");
                container.hide(1000, function() {
                    container.remove();
                });
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        }
    });

    $(".fileinput-button").click(function (e) {
        showPromptDialog("Добавление видео", "Скопируйте в поле ссылку на видео", "", "Добавить", "Отмена", function (data) {
            var id = parseYoutubeVideo(data.promptValue);
            if (id) {
                blockUI();
                postJSON("/api/video/album/" + $(".album-editor").data("albumId") + "/" + id + "/add")
                    .fail(function () {
                        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                    })
                    .always(function () {
                        refresh();
                        unblockUI();
                    });
            } else {
                alert("Неверный формат ссылки");
            }
            
        }, null, null, null, 170);
    });

    var updateSortOrder = function () {
        var albumId = $(".album-editor").data("albumId");
        var images = $(".template-download:not(.error)").map(function () {
            return $(this).data("videoId");
        }).get();
        blockUI();
        postJSON("/api/video/album/" + albumId + "/sort/update", images)
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
    };

    //window.jQuery171('#fileupload').bind("fileuploaddone", function (e, data) {
    //    $(data.result).each(function () {
    //        var albumId = $(".album-editor").data("albumId");
    //        var imageId = this.name;

    //        var imgContainer = $(".album-editor").find(".template-download[data-image-id*='" + imageId + "']");
    //        imgContainer.find(".delete").find("button").data("url", "/api/album/" + albumId + "/images/remove/" + imageId);

    //        postJSON("/api/album/" + albumId + "/" + imageId + "/add", "", function (result) {
    //            if (result.succeed == true) {
    //                updateSortOrder();
    //            }
    //        });
    //    });
    //});

    $(".album-editor .files").sortable({
        placeholder: "ui-state-highlight",
        items: ".template-download:not(.error)",
        update: function (event, ui) {
            updateSortOrder();
        }
    });

    $(".album-editor").on("change", ".video-title", function () {
        var videoId = $(this).closest(".template-download").data("videoId");
        var title = $(this).val();
        postJSON("/api/video/" + videoId + "/title/update", { title: title }, function (result) {
            if (result.succeed == true) {

            }
        }).done(function () {

        });
    });

    $(".album-editor").on("change", ".video-description", function () {
        var videoId = $(this).closest(".template-download").data("videoId");
        var description = $(this).val();

        postJSON("/api/video/" + videoId + "/description/update", { description: description }, function (result) {
            if (result.succeed == true) {

            }
        }).done(function () {

        });
    });
}
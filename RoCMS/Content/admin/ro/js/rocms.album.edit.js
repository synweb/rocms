/// <reference path="../../vendor/FU/short/jquery.fileupload.js" />
/// <reference path="../js/admin-ajax.js" />
/// <reference path="../js/admin.dialogs.js" />
/// <reference path="../../vendor/FU/ui/tmpl.min.js" />1

function albumEditorLoaded() {

    //window.jQuery('#fileupload').fileupload();

    //window.jQuery('#fileupload').fileupload('option', {
    //    maxFileSize: 50000000,
    //    resizeMaxWidth: 1920,
    //    resizeMaxHeight: 1200
    //});

    $("#albumName, #albumDescription").change(function () {
        if ($(this).val()) {
            blockUI();
            var data = {
                albumId: $("#albumName").data("album-id"),
                name: $("#albumName").val(),
                description: $("#albumDescription").val()
            };
            postJSON("/api/album/update", data).fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
                .always(function () {
                unblockUI();
            });
        } else {
            $(this).focus();
        }
    });

    //$(".fileinput-button").click(function (e) {
    //    $('#fileupload').click();
    //});



    $(document).on("click", ".image-picking-container .fileinput-button", function (e) {
        $('#fileupload').click();
    });
    InitImageUploader();

    var updateSortOrder = function () {
        var albumId = $(".album-editor").data("albumId");
        var images = $(".template-download:not(.error)").map(function () {
            return $(this).data("imageId");
        }).get();
        blockUI();
        postJSON("/api/album/" + albumId + "/sort/update", images).fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
            .always(function () {
                unblockUI();
            });
    };

    $(".add-from-another").click(function (e) {
        var albumId = $(".album-editor").data("albumId");
        var options = { restrictUpload: true };
        showImagePickDialog(function (imageData) {
            blockUI();
            postJSON("/api/album/" + albumId + "/" + imageData.ID + "/add", "", function (result) {
                if (result.succeed === true) {
                    var templateModel = {
                        files: [],
                        formatFileSize: "Загружено только что",
                        options: ""
                    };
                    templateModel.files.push({
                        name: imageData.ID,
                        url: '/Gallery/Image/' + imageData.ID,
                        thumbnail_url: imageData.Url,
                        delete_url: "/api/album/"+albumId+"/images/remove/"+imageData.ID
                    });
                    var downloadTemplate = window.tmpl("template-download");
                    var newHtml = $(downloadTemplate(templateModel));
                    newHtml.addClass("in");
                    $(".files.gallery").prepend(newHtml);
                    updateSortOrder();
                } else {
                    if (result.errorType === "Sql") {
                        smartAlert("Это изображение уже есть в текущем альбоме");
                    } else {
                        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                    }

                }
            }).fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });

        }, options);
    });


    window.jQuery('#fileupload_album').bind("fileuploadstart", function (e, data) {
        
        blockUI();
    });
    window.jQuery('#fileupload_album').bind("fileuploadalways", function (e, data) {
        unblockUI();
    });
    window.jQuery('#fileupload_album').bind("fileuploaddone", function (e, data) {
        $(data.result.files).each(function () {
            var albumId = $(".album-editor").data("albumId");
            var imageId = this.name;

            var imgContainer = $(".album-editor").find(".template-download[data-image-id*='" + imageId + "']");
            imgContainer.find(".delete").find("button").data("url", "/api/album/" + albumId + "/images/remove/" + imageId);

            postJSON("/api/album/" + albumId + "/" + imageId + "/add", "", function (result) {
                if (result.succeed === true) {
                    setTimeout(updateSortOrder, 500);
                }
            });
        });
    });

    $(".album-editor .files").sortable({
        placeholder: "sortable-placeholder",
        items: ".template-download:not(.error)",
        update: function (event, ui) {
            updateSortOrder();
        }
    });

    $(".album-editor").on("change", ".image-title", function () {
        var albumId = $(".album-editor").data("albumId");
        var imageId = $(this).closest(".template-download").data("imageId");
        var title = $(this).val();
        postJSON("/api/album/" + albumId + "/image/" + imageId + "/title/update", { title: title }, function (result) {
            if (result.succeed == true) {

            }
        }).done(function () {

        });
    });

    $(".album-editor").on("change", ".image-description", function () {
        var albumId = $(".album-editor").data("albumId");
        var imageId = $(this).closest(".template-download").data("imageId");
        var description = $(this).val();

        postJSON("/api/album/" + albumId + "/image/" + imageId + "/description/update", { description: description }, function (result) {
            if (result.succeed == true) {

            }
        }).done(function () {

        });
    });

    $(".album-editor").on("change", ".image-destination-url", function () {
        var albumId = $(".album-editor").data("albumId");
        var imageId = $(this).closest(".template-download").data("imageId");
        var destinationUrl = $(this).val();

        postJSON("/api/album/" + albumId + "/image/" + imageId + "/destinationUrl/update", { destinationUrl: destinationUrl }, function (result) {
            if (result.succeed == true) {

            }
        }).done(function () {

        });
    });
}
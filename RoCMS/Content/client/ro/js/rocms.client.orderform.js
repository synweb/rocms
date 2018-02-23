$.fn.pop = function () {
    var top = this.get(-1);
    this.splice(this.length - 1, 1);
    return top;
};

function onOrderFormLoaded(formElementId, orderFormId, maxFilesCount, fileAttachmentEnabled, redirectUrl, successMessage, metricsCode) {

    var $container = $("#"+ formElementId +".order-container");


    $(".fileinput-button", $container).click(function (e) {
        $('#fileupload', $container).click();
    });

    if (fileAttachmentEnabled.toLowerCase() == "true") {

        $('#fileupload', $container).fileupload({
            sequentialUploads: true,
            replaceFileInput: false,
            dataType: 'json',
            autoUpload: false,
            url: $('#fileupload', $container).data('url'),
            processdone: function(e, data) {

                var filename = data.files[0].name;

                var liid = "li" + ($("#attachments li", $container).length + 1);
                data.liid = liid;
                var li = $("<li  id='" +
                    liid +
                    "'><a href='#' class='remove-file' title='Удалить'><i class='fa fa-trash-o'>&nbsp;</i></a>&nbsp;" +
                    filename +
                    "</li>");
                li.data(data);

                $("#attachments", $container).append(li);

                if ($("#attachments li", $container).length >= maxFilesCount) {
                    $(".fileinput-button", $container).hide();
                }
            },
            start: function() {
                $("button.send", $container).attr("disabled", true);
                $(".fileinput-button", $container).hide();
                $(".loading", $container).show();
            },
            done: function(e, data) {
                $(data.result).each(function() {

                    $(".fileinput-button", $container).show();
                    $(".loading", $container).hide();
                    $("button.send", $container).removeAttr("disabled");

                    if (this.error) {
                        smartAlert(this.error);
                    } else {

                        var li = $("#" + data.liid, $container);
                        li.data('attach-id', this.file_id);


                    }
                });
            }
        });

    }
    var $div = $container.find(".message-container");
    $.validator.unobtrusive.parse($div, true);

    var clearForm = function () {


        $("input, textarea", $container).each(function () {
            $(this).val("");
        });
        if (fileAttachmentEnabled.toLowerCase() == "true") {
            
            $(".file-upload", $container).show();
            $(".fileinput-button", $container).show();
            $(".loading", $container).hide();
            $(".fileupload-restriction", $container).show();


            $("#attachments", $container).html('');
            $("#fileupload", $container).val('');

            $(".fileinput-button", $container).show();
            
        }
    };

    $(document).on('click', "#attachments .remove-file", function () {
        $(this).closest("li").remove();
        if ($("#attachments li", $container).length < maxFilesCount) {
            $(".fileinput-button", $container).show();
        }
        return false;
    });

    $(".clear", $container).click(function () {
        clearForm();
    });

    $(".send", $container).click(function () {
        var form = $("form.message-container", $container);
        $.validator.unobtrusive.parse(form, true);

        var that = $(this);
        $container.find('.message-state').html("");


        var saveData = function () {
            that.attr("disabled", "disabled");

            var message = {

                Fields: form.serializeObject(),
                OrderFormId: orderFormId,
                Email: form.find(".email").val(),
                Name: form.find(".name").val(),
                Phone: form.find(".phone").val(),
                AttachIds: $("#attachments li", $container).map(function () {
                    return $(this).data("attachId");
                }).get()

            };
            sendMessage(message, "order", function (result) {
                var msg;
                if (result.succeed === true) {

                    if (!successMessage) {
                        successMessage = "Ваша заявка принята, мы с вами свяжемся в ближайшее время."
                    }
                    msg = '<div class="alert alert-success">' + successMessage + '</div>';
                    if (window.yaCounter) {
                        window.yaCounter.reachGoal(metricsCode);
                    }
                    clearForm();
                    if (redirectUrl)
                    {
                        window.location.href = redirectUrl;
                    }
                } else {
                    msg = "Произошла ошибка, попробуйте еще раз.";
                }
                $container.find('.message-state').html(msg);
            }, function () { that.removeAttr("disabled"); });


        };
        if (form.valid()) {

            var attachments = $("#attachments li", $container);

            var uploadAttach = function () {

                if (attachments.length > 0) {

                    var elem = attachments.pop();
                    $(elem).data().submit().always(function () { uploadAttach(); });
                } else {
                    saveData();
                }
            };

            uploadAttach();
        } else {
            form.validate().focusInvalid();
        }
        return false;
    });
}


function showFormInDialog(formId) {
    var url = '/Home/OrderForm/' + formId;
    showBootstrapDialogFromUrl(url, {
        removeButtons: true,
        onBeforeShow: function () {
            var $dialog = $(this);
            $dialog.find(".modal-dialog").addClass("modal-lg");
        }
    });
}
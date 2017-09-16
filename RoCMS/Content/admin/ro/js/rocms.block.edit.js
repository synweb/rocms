/// <reference path="../../../base/vendor/jquery/core/jquery-2.0.2.js" />
function blockEditorLoaded() {
    $(".block-title").change(function () {
        var val = $(this).val();

        if (val && !$('.block-name').val()) {
            $('.block-name').val(textToUrl(val));
        };
    });
    var saveBlock = function (url, context, onSuccess) {
        var $form = context.find("form");
        $.validator.unobtrusive.parse($form, true);
        if ($form.valid()) {
            var title = $('.block-title').val();
            var content = getTextFromEditor('block_content');
            var name = $('.block-name').val();
            var blockId = $('.block-items-info').data('blockId');
            postJSON(url, { Title: title, Content: content, BlockId: blockId, Name: name }, function (result) {
                if (result.Succeed === true) {
                    if (onSuccess) {
                        onSuccess(result.Data);
                    }
                }
            });
        } else {
            $form.validate().focusInvalid();
        }
    };
    $('#adminContent').on("click", "#createBlock", function () {
        var url = $(this).attr('href');
        blockUI();
        saveBlock(url, $(this).closest(".block-items-info"), function (res) {
            window.location.href = '/Admin/EditBlock/' + res;
        });
        return false;
    });
    $('#adminContent').on("click", "#acceptBlock", function () {
        var url = $(this).attr('href');
        blockUI();
        saveBlock(url, $(this).closest(".block-items-info"), function () {
            unblockUI();
        });
        return false;
    });
    $('#adminContent').on("click", ".block-summary .button-delete", function () {
        if (!confirmRemoval()) {
            return false;
        }
        var container = $(this).parents(".block-summary");
        var id = container.data("blockId");
        var url = "/api/block/" + id + "/delete";
        blockUI();
        postJSON(url, "", function (res) {
                if (res.succeed) {
                    container.hide(1000, function () { container.remove(); });
                } else {
                    if (res.message) {
                        alert(res.message);
                    } else {
                        alert("Произошла ошибка. Попробуйте еще раз или свяжитесь с администрацией");
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
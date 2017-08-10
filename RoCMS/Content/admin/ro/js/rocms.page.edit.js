/// <reference path="../../../base/vendor/jquery/core/jquery-2.0.2.js" />

function pageEditorLoad() {
    $('.layout').val($('.layout').data("selectedValue"));

    $(".page-title").change(function () {
        var val = $(this).val();

        if (val && !$('.page-relativeurl').val()) {
            $('.page-relativeurl').val(textToUrl(val));
        };

        if (val && !$('.page-h1').val()) {
            $('.page-h1').val(val);
            $('.page-h1').change();
        };

        if(val && !$(".page-annotation").val()) {
            $('.page-annotation').val(val);
            $('.page-annotation').change();
        }

        $(".title-length").text(val.length);
    });

    $(".page-h1").change(function () {
        var val = $(this).val();
        $(".header-length").text(val.length);
    });

    $(".page-annotation").change(function () {
        var val = $(this).val();
        $(".annotation-length").text(val.length);
    });
}

$(function () {
    $('#adminContent').on("click", ".page-summary .button-copy",
        function() {
            blockUI();
            var id = $(this).closest("li.page-list-item").data("pageId");
            postJSON("/api/page/" + id + "/copy",
                    null,
                    function(res) {
                        if (res.succeed) {
                            window.location.href = "/Admin/EditPage/"+res.data.relativeUrl;
                        } else {
                            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                        }
                    })
                .fail(function() {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function() {
                    unblockUI();
                });
            return false;
        });
    $('#adminContent').on("click", ".page-summary .button-delete", function () {
        if (!confirmRemoval()) {
            return false;
        }
        var container = $(this).closest(".page-summary");
        var dataPageUrl = container.data("pageUrl");
        var url = $(this).attr("href");
        blockUI();
        postJSON(url, "", function () {
            container.hide(1000, function() {
                container.remove();
                $("li[data-page-url=" + dataPageUrl + "]").remove();
            });
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
        return false;
    });

    function updatePage(url, context, onSuccess) {
        var $form = context.find("form");
        $.validator.unobtrusive.parse($form, true);

        if ($form.valid()) {
            var title = $('.page-title').val();
            var header = $('.page-h1').val();

            var relativeUrl = $('.page-relativeurl').val();
            var annot = $('.page-annotation').val();
            
            var content = getTextFromEditor('page_content');
            var keywords = $('.page-keywords').val();
            var layout = $('.layout').val();

            var pageId = $form.data("pageId");
            var parentPageId = $('.parent-page-id').val();
            var hideInSitemap = $('#sitemapHide').is(':checked');
            var scripts = getTextFromEditor('page_scripts');
            var styles = getTextFromEditor('page_styles');
            var additionalHeaders = getTextFromEditor('page_headers');


            var data = {
                Title: title,
                Header: header,
                RelativeUrl: relativeUrl,
                Annotation: annot,
                Content: content,
                Keywords: keywords,
                ParentPageId: parentPageId,
                Layout: layout,
                PageId: pageId,
                HideInSitemap: hideInSitemap,
                Styles: styles,
                Scripts: scripts,
                AdditionalHeaders: additionalHeaders
            };

            blockUI();
            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify(data),
                contentType: "application/json",
                success: function (result) {
                    if (result.Succeed === true) {
                        if (onSuccess) {
                            onSuccess(data);
                        }
                    }
                }
            }).fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
        } else {
            $form.validate().focusInvalid();
        }
    }

    $('#adminContent').on("click", "#createPage", function () {
        var url = $(this).attr('href');
        updatePage(url, $(this).closest(".page-items-info"), function (data) {
            blockUI();
            window.location.href = "/Admin/EditPage/" + data.RelativeUrl;
        });
        return false;
    });

    $('#adminContent').on("click", "#acceptPage", function () {
        var url = $(this).attr('href');
        updatePage(url, $(this).closest(".page-items-info"));
        return false;
    });

    $('#adminContent').on("click", ".editPageLink", function () {
        var url = $(this).attr("href");
        blockUI();
        $.ajax({
            url: url,
            type: 'GET',
            success: function(data) {
                $('#adminContent').html(data);
            }
        }).fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
            .always(function () {
                unblockUI();
            });
        return false;
    });



});


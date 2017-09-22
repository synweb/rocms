/// <reference path="../../../base/vendor/jquery/core/jquery-2.0.2.js" />
/// <reference path="rocms.heart.js" />


App.Admin.Page = function () {
    var self = this;
    $.extend(self, new App.Admin.Heart());
    self.content = ko.observable().extend({ required: true });
}

App.Admin.PageValidationMapping = {
    content: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    }
}

App.Admin.PageFunctions = {
    initPage: function () {
        var self = this;
        self.initHeart();

        if (self.scripts()) {
            setTextToEditor("page_content", self.content());
        }
    },

    preparePageForUpdate: function () {
        var self = this;
        self.prepareHeartForUpdate();

        var content = getTextFromEditor('page_content');
        self.content(content);
    },

    save: function (url, onSuccess) {
        var self = this;
        blockUI();
        postJSON(url, ko.toJS(self), function (result) {
            if (result.succeed === true) {
                if (onSuccess) {
                    onSuccess(result.data);
                }
            }
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
    },

    update: function (onSuccess) {
        var self = this;
        self.save("/Admin/EditPage", function () {
            if (onSuccess) {
                onSuccess();
            }
        });
    },


    create: function (onSuccess) {
        var self = this;
        self.save("/Admin/CreatePage", function (result) {
            if (onSuccess) {
                onSuccess(result);
            }
        });
    },
}

function pageEditorLoad(relativeUrl) {
    //$('.layout').val($('.layout').data("selectedValue"));

    //$(".page-title").change(function () {
    //    var val = $(this).val();

    //    if (val && !$('.page-relativeurl').val()) {
    //        $('.page-relativeurl').val(textToUrl(val));
    //    };

    //    if (val && !$('.page-h1').val()) {
    //        $('.page-h1').val(val);
    //        $('.page-h1').change();
    //    };

    //    if(val && !$(".page-annotation").val()) {
    //        $('.page-annotation').val(val);
    //        $('.page-annotation').change();
    //    }

    //    $(".title-length").text(val.length);
    //});

    //$(".page-h1").change(function () {
    //    var val = $(this).val();
    //    $(".header-length").text(val.length);
    //});

    //$(".page-annotation").change(function () {
    //    var val = $(this).val();
    //    $(".annotation-length").text(val.length);
    //});


    var vm = {
        page: ko.validatedObservable($.extend(new App.Admin.Page(), App.Admin.PageFunctions)),
        parents: ko.observableArray(),
        save: function () {
            vm.page().preparePageForUpdate();
            if (vm.errors()().length) {
                vm.errors().showAllMessages();
                return false;
            }

            if (vm.page().heartId()) {
                vm.page().update();
            } else {
                vm.page().create(function () {
                    blockUI();
                    window.location.href = "/Admin/EditPage/" + vm.page().relativeUrl();
                });
            }
            return false;
        }
    };

    vm.errors = ko.computed(function () {
        return ko.validation.group(vm.page(), { deep: true });
    });

    blockUI();
    $.when(
            getJSON("/api/page/pages/get", "", function (res) {
                $(res).each(function () {
                    vm.parent.push(this);
                });
            }),
            getJSON("/api/page/" + relativeUrl + "/get", "", function (res) {
                if (res.succeed) {
                    var page = $.extend(ko.mapping.fromJS(res.data, App.Admin.PageValidationMapping), App.Admin.PageFunctions);
                    vm.page(page);
                }
            })

    ).then(
        function () {
            ko.applyBindings(vm);
        },
        function() {
            smartAlert("Произошла ошибка");
        }
    ).always(function() {
        unblockUI();
    });

    //getJSON("/api/page/pages/get", "", function (res) {
    //    $(res).each(function () {
    //        vm.parent.push(this);
    //    });
    //});

    //if (relativeUrl) {
    //    blockUI();
    //    getJSON("/api/page/" + relativeUrl + "/get", "", function (res) {
    //        if (res.succeed) {
    //            var page = $.extend(ko.mapping.fromJS(res.data, App.Admin.PageValidationMapping), App.Admin.PageFunctions);
    //            vm.page(page);
    //        }
    //    });
    //}

    //ko.applyBindings(vm);
}

$(function () {
    $('#adminContent').on("click", ".page-summary .button-copy",
        function () {
            blockUI();
            var id = $(this).closest("li.page-list-item").data("pageId");
            postJSON("/api/page/" + id + "/copy",
                    null,
                    function (res) {
                        if (res.succeed) {
                            window.location.href = "/Admin/EditPage/" + res.data.relativeUrl;
                        } else {
                            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
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
    $('#adminContent').on("click", ".page-summary .button-delete", function () {
        if (!confirmRemoval()) {
            return false;
        }
        var container = $(this).closest(".page-summary");
        var dataPageUrl = container.data("pageUrl");
        var url = $(this).attr("href");
        blockUI();
        postJSON(url, "", function () {
            container.hide(1000, function () {
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



    //function updatePage(url, context, onSuccess) {
    //    var $form = context.find("form");
    //    $.validator.unobtrusive.parse($form, true);

    //    if ($form.valid()) {
    //        var title = $('.page-title').val();
    //        var header = $('.page-h1').val();

    //        var relativeUrl = $('.page-relativeurl').val();
    //        var annot = $('.page-annotation').val();

    //        var content = getTextFromEditor('page_content');
    //        var keywords = $('.page-keywords').val();
    //        var layout = $('.layout').val();

    //        var pageId = $form.data("pageId");
    //        var parentPageId = $('.parent-page-id').val();
    //        var hideInSitemap = $('#sitemapHide').is(':checked');
    //        var scripts = getTextFromEditor('page_scripts');
    //        var styles = getTextFromEditor('page_styles');
    //        var additionalHeaders = getTextFromEditor('page_headers');


    //        var data = {
    //            Title: title,
    //            Header: header,
    //            RelativeUrl: relativeUrl,
    //            Annotation: annot,
    //            Content: content,
    //            Keywords: keywords,
    //            ParentPageId: parentPageId,
    //            Layout: layout,
    //            PageId: pageId,
    //            HideInSitemap: hideInSitemap,
    //            Styles: styles,
    //            Scripts: scripts,
    //            AdditionalHeaders: additionalHeaders
    //        };

    //        blockUI();
    //        $.ajax({
    //            url: url,
    //            type: 'POST',
    //            data: JSON.stringify(data),
    //            contentType: "application/json",
    //            success: function (result) {
    //                if (result.Succeed === true) {
    //                    if (onSuccess) {
    //                        onSuccess(data);
    //                    }
    //                }
    //            }
    //        }).fail(function () {
    //            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
    //        })
    //        .always(function () {
    //            unblockUI();
    //        });
    //    } else {
    //        $form.validate().focusInvalid();
    //    }
    //}

    //$('#adminContent').on("click", "#createPage", function () {
    //    var url = $(this).attr('href');
    //    updatePage(url, $(this).closest(".page-items-info"), function (data) {
    //        blockUI();
    //        window.location.href = "/Admin/EditPage/" + data.RelativeUrl;
    //    });
    //    return false;
    //});

    //$('#adminContent').on("click", "#acceptPage", function () {
    //    var url = $(this).attr('href');
    //    updatePage(url, $(this).closest(".page-items-info"));
    //    return false;
    //});

    $('#adminContent').on("click", ".editPageLink", function () {
        var url = $(this).attr("href");
        blockUI();
        $.ajax({
            url: url,
            type: 'GET',
            success: function (data) {
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


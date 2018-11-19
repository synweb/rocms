/// <reference path="../../../base/vendor/jquery/core/jquery-2.0.2.js" />
/// <reference path="rocms.heart.js" />


App.Admin.Page = function () {
    var self = this;
    $.extend(self,  new App.Admin.Heart());
    self.content = ko.observable().extend({ required: true });
}

App.Admin.PageValidationMapping = {
    content: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    }
}

$.extend(App.Admin.PageValidationMapping, App.Admin.HeartValidationMapping);

App.Admin.PageFunctions = {
    initPage: function () {
        var self = this;
        self.initHeart();

        if (self.content()) {
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
        self.save("/api/page/update", function () {
            if (onSuccess) {
                onSuccess();
            }
        });
    },

    create: function (onSuccess) {
        var self = this;
        self.save("/api/page/create", function (result) {
            if (onSuccess) {
                onSuccess(result);
            }
        });
    },
}

$.extend(App.Admin.PageFunctions, App.Admin.HeartFunctions);

function pageEditorLoad(relativeUrl) {
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

    vm.parents.push({ title: "Нет", heartId: null, type: "Выберите..." });

    vm.errors = ko.computed(function () {
        return ko.validation.group(vm.page(), { deep: true });
    });

    blockUI();
    $.when(
            getJSON("/api/heart/hearts/RoCMS.Web.Contract.Models.Page/get", "", function (res) {
                $(res).each(function () {
                    vm.parents.push(this);
                });
            }),
            getJSON("/api/page/" + relativeUrl + "/get", "", function (res) {
                if (res.succeed && res.data) {
                    var page = $.extend(ko.mapping.fromJS(res.data, App.Admin.PageValidationMapping), App.Admin.PageFunctions);
                    temp = page;
                    vm.page(page);
                }
                vm.page().initPage();
            })

    ).then(
        function () {
            vm.parents.remove(function(item) { return item.heartId === vm.page().heartId() });
            ko.applyBindings(vm);

            $(".withsearch").selectpicker();

        },
        function() {
            smartAlert("Произошла ошибка");
        }
    ).always(function() {
        unblockUI();
    });

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


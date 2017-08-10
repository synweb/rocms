/// <reference path="admin-ajax.js" />
/// <reference path="../../../base/vendor/knockout/knockout-3.0.0.debug.js" />

App.Admin.Redirect = function (data) {
    var self = this;
    self.key = ko.observable().extend({ required: true });
    self.value = ko.observable().extend({ required: true });

    self.init = function (data2) {
        self.key(data2.key);
        self.value(data2.value);
    }

    if (data) {
        self.init(data);
    }
}

function redirectListLoaded() {
    blockUI();

    var vm = {
        redirects: ko.observableArray(),
        removeItem: function (item) {
            vm.redirects.remove(item);
        },
        addItem: function () {
            vm.redirects.push(new App.Admin.Redirect());
        },
        save: function () {
            if (vm.errors()().length) {
                vm.errors().showAllMessages();
                return false;
            }
            var data = ko.toJS(vm.redirects);
            postJSON("/api/admin/redirect/save", data, function (result) {
                if (result.succeed) {
                    smartAlert("Данные сохранены");
                }
            });
        }
    }
    vm.errors = ko.computed(function () {
        return ko.validation.group(vm.redirects(), { deep: true });
    });

    getJSON("/api/admin/redirect/get", null, function (result) {
        $(result).each(function () {
            vm.redirects.push(new App.Admin.Redirect(this));
        });
    }).always(function () {
        ko.applyBindings(vm);
        unblockUI();
    });
}
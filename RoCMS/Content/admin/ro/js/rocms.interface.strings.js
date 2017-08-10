/// <reference path="admin-ajax.js" />
/// <reference path="../../../base/vendor/knockout/knockout-3.0.0.debug.js" />



App.Admin.InterfaceString = function (data) {
    var self = this;
    self.key = ko.observable().extend({ required: true });
    self.value = ko.observable();

    self.init = function(data2) {
        self.key(data2.key);
        self.value(data2.value);
    }

    if (data) {
        self.init(data);
    }
}

function interfaceStringListLoaded() {
    blockUI();

    var vm = {
        strings: ko.observableArray(),
        removeItem: function (item) {
            vm.strings.remove(item);
        },
        addItem: function () {
            vm.strings.push(new App.Admin.InterfaceString());
        },
        save: function () {
            var data = ko.toJS(vm.strings);
            postJSON("/api/interface/strings/save", data, function (result) {
                if (result.succeed) {
                    smartAlert("Данные сохранены");
                }
            });
        }
    }

    getJSON("/api/interface/strings/get", null, function(result) {
        $(result).each(function() {
            vm.strings.push(new App.Admin.InterfaceString(this));
        });
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            ko.applyBindings(vm);
            unblockUI();
        });
}
/// <reference path="admin-ajax.js" />
/// <reference path="../../../base/vendor/knockout/knockout-3.0.0.debug.js" />


App.Admin.Widget = function (data) {
        var self = this;
        self.pattern = ko.observable().extend({ required: true });
        self.viewPath = ko.observable().extend({ required: true });

    self.init = function (data2) {
        self.pattern(data2.pattern);
        self.viewPath(data2.viewPath);
    }

    if (data) {
        self.init(data);
    }
}

function widgetListLoaded() {
    blockUI();

    var vm = {
        widgets: ko.observableArray(),
        removeItem: function (item) {
            vm.widgets.remove(item);
        },
        editItem: function (item) {
            var path = item.viewPath().replace("~/", "").replace(/[/]/g, "\\");
            window.location.href = "/Developer/CodeEditor#" + path;
        },
        addItem: function () {
            vm.widgets.push(new App.Admin.Widget());
        },
        save: function () {
            if (vm.errors()().length) {
                vm.errors().showAllMessages();
                return false;
            }
            var data = ko.toJS(vm.widgets);
            postJSON("/api/dev/widgets/save", data, function(result) {
                if (result.succeed) {
                    smartAlert("Данные сохранены");
                }
            });
        }
    }
    vm.errors = ko.computed(function () {
        return ko.validation.group(vm.widgets(), { deep: true });
    });

    getJSON("/api/dev/widgets/get", null, function (result) {
        $(result).each(function () {
            vm.widgets.push(new App.Admin.Widget(this));
        });
    });
    ko.applyBindings(vm);
    unblockUI();
}
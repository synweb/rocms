function ymlExportLoaded() {
    var vm = {
        settings: ko.validatedObservable(),
        exportTasks: ko.observableArray(),
        generateFile: function () {
            if (vm.settings.isValid) {
                postJSON("/api/shop/export/yml/generate", ko.toJS(vm.settings), function(result) {
                    if (result.succeed === true) {
                        vm.exportTasks.push(ko.mapping.fromJS(result.data));
                        alert("Процесс генерации файла запущен");
                    }
                })
                    .fail(function () {
                        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                    })
                    .always(function () {
                        unblockUI();
                    });
            }
            else {
                vm.settings.errors.showAllMessages();
            }
        }
    };

    getJSON("/api/shop/export/yml/tasks", "", function (result) {
        $(result).each(function () {
            vm.exportTasks.push(ko.mapping.fromJS(this));
        });       
    }).fail(function () {
        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
    })
        .always(function () {
            unblockUI();
        });

    getJSON("/api/shop/export/yml/settings/get", "", function (result) {
        vm.settings(new App.Admin.YmlExportSettings(result));        
    }).fail(function () {
        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
    })
        .always(function () {
            unblockUI();
        });

    ko.applyBindings(vm);
}

App.Admin.YmlExportSettings = function (data) {
    var self = this;

    self.siteName = ko.observable().extend({ required: true });
    self.siteDescription = ko.observable().extend({ required: true });
    self.siteUrl = ko.observable().extend({ required: true });
    self.clickRate = ko.observable().extend({ number: true });;
    self.deliveryCost = ko.observable().extend({ number: true });;
    self.pickup = ko.observable();

    if (data) {
        self.siteName(data.siteName);
        self.siteDescription(data.siteDescription);
        self.siteUrl(data.siteUrl);
        if (data.clickRate != 0) {
            self.clickRate(data.clickRate);
        }
        if (data.deliveryCost != 0) {
            self.deliveryCost(data.deliveryCost);
        }
        self.pickup(data.pickup);
    }
};
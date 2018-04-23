function massPriceEditorLoaded() {
    var vm = {
        massChange: ko.validatedObservable(App.Admin.MassPriceChange),
        massChangeTasks: ko.observableArray(),

        send: function () {
            if (vm.massChange.isValid && vm.massChange().value() > 0) {
                blockUI();


                var data = ko.toJS(vm.massChange);

                data.filter.categoryIds = [$.map(data.filter.categoryIds, function (val) { return val.id; })];
                data.filter.manufacturerIds = $.map(data.filter.manufacturerIds, function (val) { return val.id; });

                postJSON("/api/shop/mass/price/change", data, function (result) {
                    if (result.succeed === true) {
                        vm.massChangeTasks.push(ko.mapping.fromJS(result.data));
                        vm.massChange().clear();

                        alert("Процесс изменения цен запущен");
                    }
                    else {
                        alert(result.message);
                    }
                })
                    .fail(function () {
                        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                    })
                    .always(function () {
                        unblockUI();
                    });
            } else {
                vm.massChange.errors.showAllMessages();
            }
        }
    };

    getJSON("/api/shop/mass/price/tasks", "", function (result) {
        $(result).each(function () {
            vm.massChangeTasks.push(ko.mapping.fromJS(this));
        });
    });

    ko.applyBindings(vm);
}

App.Admin.MassPriceChangeGoodsFilter = {
    manufacturerIds: ko.observableArray(),
    categoryIds: ko.observableArray()
};

App.Admin.MassPriceChange = {
    filter: ko.observable(App.Admin.MassPriceChangeGoodsFilter),
    increase: ko.observable("true"),
    value: ko.observable(0).extend({ required: true, number: true }),
    comment: ko.observable(),

    addCategory: function () {
        var self = this;
        showCategoriesDialog(function (category) {
            var result = $.grep(self.filter().categoryIds(), function (e) {
                return e.id == category.id;
            });
            if (result.length == 0) {
                self.filter().categoryIds.push(category);
            }
        });
    },
    removeCategory: function (category, parent) {
        parent.filter().categoryIds.remove(function (item) {
            return item.id == category.id;
        });
    },
    addManufacturer: function () {
        var self = this;
        showManufacturersDialog(function (manufacturer) {
            var result = $.grep(self.filter().manufacturerIds(), function (e) {
                return e.id == manufacturer.id;
            });
            if (result.length == 0) {
                self.filter().manufacturerIds.push(manufacturer);
            }
        });
    },
    removeManufacturer: function (manufacturer, parent) {
        parent.filter().manufacturerIds.remove(function (item) {
            return item.id == manufacturer.id;
        });
    },
    clear: function () {
        var self = this;
        self.increase("true");
        self.value(0);
        self.comment("");
        self.filter().categoryIds.removeAll();
        self.filter().manufacturerIds.removeAll();
    }
}
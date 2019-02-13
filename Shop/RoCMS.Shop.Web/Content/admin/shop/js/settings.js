function shopSettingsLoaded() {
    var vm = {
        settings: ko.validatedObservable(),
    };

    getJSON("/api/shop/settings/get", "", function (result) {
        vm.settings(new App.Admin.ShopSettings(result));
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });

    ko.applyBindings(vm);
}

App.Admin.ShopSettings = function (data) {
    var self = this;

    self.deliveryCost = ko.observable().extend({ number: true });
    self.selfPickupCost = ko.observable().extend({ number: true });
    self.shopUrl = ko.observable().extend({ required: true });
    self.defaultPageSize = ko.observable(10).extend({ required: true });

    self.specsInFilter = ko.observableArray();

    self.courierCities = ko.observable().extend({ required: true });

    if (data) {
        if (data.deliveryCost != 0) {
            self.deliveryCost(data.deliveryCost);
        }
        if (data.selfPickupCost != 0) {
            self.selfPickupCost(data.selfPickupCost);
        }
        if (data.courierCities) {
            self.courierCities(data.courierCities);
        }
        if (data.shopUrl) {
            self.shopUrl(data.shopUrl);
        }
        if (data.defaultPageSize) {
            self.defaultPageSize(data.defaultPageSize);
        }

        if (data.specsInFilter.length > 0) {
            $(data.specsInFilter).each(function () {
                self.specsInFilter.push(this);
            });
        }
    }

    self.addSpec = function() {
        var self = this;
        showSpecDialog(function (item) {
            var spec = {
                id: item.specId(),
                name: item.name
            };

            var result = $.grep(self.specsInFilter(), function (e) {
                return e.id === item.specId();
            });

            if (result.length === 0) {
                self.specsInFilter.push(spec);
            }
        });
    }

    self.removeSpec = function (spec) {
        console.log("spec: ");
        console.log(spec);
        self.specsInFilter.remove(function (item) {
            console.log("item: " + item.id);    
            console.log(item);
            return item.id === spec.id;
        });
    }

    self.save = function () {
        blockUI();
        postJSON("/api/shop/settings/update", ko.toJS(self), function (result) {
            if (result.succeed === true) {
            }
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
    };
};
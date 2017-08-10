function regularClientsLoaded() {
    var vm = {
        regularClientDiscounts: ko.observableArray(),
        selectedItem: ko.observable(),
        createDiscount: function () {
            var discount = new App.Admin.RegularClientDiscount();
            vm.regularClientDiscounts.push(discount);
            vm.selectItem(discount);
        },
        selectItem: function (item) {
            vm.selectedItem(ko.validatedObservable(item));
        }
    };
    blockUI();
    getJSON("/api/shop/regularClients/get", "", function (result) {
        $(result).each(function () {
            vm.regularClientDiscounts.push(new App.Admin.RegularClientDiscount(this));
        });       
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });

    ko.applyBindings(vm);
}


App.Admin.RegularClientDiscount = function (data) {
    var self = this;
    self.discountId = ko.observable();
    self.minimalSum = ko.observable().extend({ required: true });
    self.discount = ko.observable().extend({ required: true });

    if (data) {
        self.discountId(data.discountId);
        self.minimalSum(data.minimalSum);
        self.discount(data.discount);
    }


    self.save = function (root) {
        if (root.selectedItem().isValid()) {
            blockUI();
            var url = self.discountId() ? "/api/shop/regularClients/update" : "/api/shop/regularClients/create";
            postJSON(url, ko.toJS(self), function (result) {
                if (result.succeed === true) {
                    if (result.data) {
                        self.discountId(result.data);
                    }                   
                }
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        } else {
            root.selectedItem().errors.showAllMessages();
        }
    };

    self.remove = function (item, parent) {
        if (parent.selectedItem() && parent.selectedItem()() && (parent.selectedItem()() == item || parent.selectedItem()().discountId() == item.discountId())) {
            parent.selectedItem("");
        }
        if (item.discountId()) {
            blockUI();
            postJSON("/api/shop/regularClients/" + item.discountId() + "/delete", "", function (result) {
                if (result.succeed === true) {
                    parent.regularClientDiscounts.remove(item);
                }
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        } else {
            parent.regularClientDiscounts.remove(item);
        }
    }
}

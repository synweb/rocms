function pickUpPointsLoaded() {
    var vm = {
        pickUpPoints: ko.observableArray(),
        selectedItem: ko.observable(),
        createPickUpPoint: function () {
            var point = new App.Admin.PickUpPoint();
            vm.pickUpPoints.push(point);
            vm.selectItem(point);
        },
        selectItem: function (item) {
            vm.selectedItem(ko.validatedObservable(item));
        },
        cities: ko.observableArray(),
        partners: ko.observableArray(),
        currentCity: ko.observable("Все города"),
        currentPartner: ko.observable("Все партнёры"),

    };
    vm.filteredPickUpPoints = ko.computed(function () {
        var points;
        if (vm.currentPartner() === "Все партнёры" && vm.currentCity() === "Все города") {
            points = vm.pickUpPoints();
        } else {
            points = ko.utils.arrayFilter(vm.pickUpPoints(), function (pp) {
                return (vm.currentCity() === "Все города" || pp.city() === vm.currentCity())
                    && (vm.currentPartner() === "Все партнёры" || pp.partner() && pp.partner().indexOf(vm.currentPartner()) === 0);
            });
        }
        return points.sort(function (left, right) {
            return left.metro() == right.metro() ? 0 : (left.metro() < right.metro() ? -1 : 1);
        });
    });
    vm.cities.push(vm.currentCity());
    vm.partners.push(vm.currentPartner());
    blockUI();
    getJSON("/api/shop/pickUpPoints/get", "", function (result) {
        $(result).each(function () {
            vm.pickUpPoints.push(new App.Admin.PickUpPoint(this));
            if (vm.cities.indexOf(this.city) === -1) {
                vm.cities.push(this.city);
            }
            if (vm.partners.indexOf(this.partner) === -1 && this.partner) {
                vm.partners.push(this.partner);
            }
        });
        unblockUI();
    });

    ko.applyBindings(vm);
}

App.Admin.PickUpPoint = function (data) {
    var self = this;

    self.title = ko.observable().extend({ required: true });
    self.city = ko.observable().extend({ required: true });
    self.address = ko.observable().extend({ required: true });
    self.pickUpPointId = ko.observable();

    self.metro = ko.observable();
    self.schedule = ko.observable();
    self.description = ko.observable();
    self.paymentType = ko.observable();
    self.imageId = ko.observable();
    self.partner = ko.observable();
    self.phone = ko.observable();
    self.howToReach = ko.observable();

    if (data) {
        self.title(data.title);
        self.city(data.city);
        self.address(data.address);
        self.pickUpPointId(data.pickUpPointId);
        self.metro(data.metro);
        self.schedule(data.schedule);
        self.description(data.description);
        self.paymentType(data.paymentType);
        self.imageId(data.imageId);
        self.partner(data.partner);
        self.phone(data.phone);
        self.howToReach(data.howToReach);
    }

    self.pickImage = function () {
        var self = this;
        showImagePickDialog(function (imageData) {
            self.imageId(imageData.ID);
        });
    },

    self.save = function (root) {
        if (root.selectedItem().isValid()) {
            blockUI();
            var url = self.pickUpPointId() ? "/api/shop/pickUpPoint/update" : "/api/shop/pickUpPoint/create";
            postJSON(url, ko.toJS(self), function (result) {
                if (result.succeed === true) {
                    if (result.data) {
                        self.pickUpPointId(result.data);
                    }
                    unblockUI();
                }
            });
        } else {
            root.selectedItem().errors.showAllMessages();
        }
    };

    self.remove = function (item, parent) {
        if (parent.selectedItem() && parent.selectedItem()() && (parent.selectedItem()() == item || parent.selectedItem()().pickUpPointId() == item.pickUpPointId())) {
            parent.selectedItem("");
        }
        if (item.pickUpPointId()) {
            blockUI();
            postJSON("/api/shop/pickUpPoint/" + item.pickUpPointId() + "/delete", "", function (result) {
                if (result.succeed === true) {
                    parent.pickUpPoints.remove(item);
                    unblockUI();
                }
            });
        } else {
            parent.pickUpPoints.remove(item);
        }
    }
};
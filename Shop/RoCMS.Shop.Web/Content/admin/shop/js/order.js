function onShopOrdersLoaded(clientId) {
    var pageSize = 10;
    var vm = {
        orders: ko.observableArray(),
        pickUpPoints: ko.observableArray(),
        loadMoreOrders: function() {
            blockUI();
            var index = vm.orders().length + 1;
            getJSON("/api/shop/orders/page/" + index + "/" + pageSize + "/" + clientId + "/get", {}, function (result) {
                $(result.data.orders).each(function() {

                    var orderVm = $.extend(ko.mapping.fromJS(this), App.Admin.OrderFunctions);
                    
                    orderVm.pickUpPoints = vm.pickUpPoints;
                    orderVm.creationDateView = ko.computed(function () {
                        return dateFormat(this.creationDate(), 'dd.mm.yyyy');
                    }, orderVm);
                    orderVm.shipmentTypeView = ko.computed(function () {
                        switch (this.shipmentType()) {
                            case "Courier":
                                return "Курьер";
                                break;
                            case "Post":
                                return "Почта";
                                break;
                            case "SelfPickup":
                                return "Самовывоз";
                                break;
                        }
                        return "1";
                    }, orderVm);

                    vm.orders.push(orderVm);
                });
                vm.hasMoreOrders(vm.orders().length < result.data.total);
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        },

        hasMoreOrders: ko.observable()
    }
    blockUI();
    var blocks = 2;

    getJSON("/api/shop/pickUpPoints/get", "", function(result) {
        vm.pickUpPoints.push(new App.Admin.PickUpPoint({
            title: 'Не определена',
            pickUpPointId: null
        }));
        $(result).each(function() {
            vm.pickUpPoints.push(new App.Admin.PickUpPoint(this));
        });
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            blocks--;
            if (blocks === 0) {
                unblockUI();
            }
        });
    
    getJSON("/api/shop/orders/page/1/" + pageSize + "/" + clientId + "/get", "", function (result) {
        $(result.data.orders).each(function () {

            var orderVm = $.extend(ko.mapping.fromJS(this), App.Admin.OrderFunctions);

            orderVm.pickUpPoints = vm.pickUpPoints;
            orderVm.creationDateView = ko.computed(function () {
                
                return dateFormat(this.creationDate(), 'dd.mm.yyyy');
            }, orderVm);
            orderVm.shipmentTypeView = ko.computed(function () {
                switch (this.shipmentType()) {
                    case "Courier":
                        return "Курьер";
                        break;
                    case "Post":
                        return "Почта";
                        break;
                    case "SelfPickup":
                        return "Самовывоз";
                        break;
                }
                return "1";
            }, orderVm);

            vm.orders.push(orderVm);
            vm.hasMoreOrders(vm.orders().length < result.data.total);
        });
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            blocks--;
            if (blocks === 0) {
                unblockUI();
            }
        });


    ko.applyBindings(vm);
}

function priceFormat(price) {
    //var str = parseFloat(Math.round(price * 100) / 100).toFixed(2);
    var d = '.';
    var t = " ";
    var c = 2;

    var n = price,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0; 
    var str = s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
    return str;
}


App.Admin.Order = function () {
    var self = this;
    self.orderId = ko.observable();
    self.creationDate = ko.observable();
    self.postCode = ko.observable();
    self.city = ko.observable().extend({ required: true });
    self.house = ko.observable();
    self.street = ko.observable();
    self.goodsInOrder = ko.observableArray();

    self.metro = ko.observable();

    self.frontNumber = ko.observable();
    self.houseIndex = ko.observable();
    self.floor = ko.observable();
    self.intercom = ko.observable();
    self.appartment = ko.observable();
    self.comment = ko.observable();
    self.adminComment = ko.observable();
    self.shipmentType = ko.observable();
    self.shipmentTypeView = ko.observable(); //Для более красивого отображение Enum-а, который едет с сервера
    self.paymentType = ko.observable();
    self.state = ko.observable();

    self.pickUpPointId = ko.observable();
    self.pickUpPoint = ko.observable();

    self.summary = ko.observable();
    self.deliveryPrice = ko.observable();
}



App.Admin.OrderFunctions = {

    //addGoods: function () {
    //    var self = this;
    //    showGoodsDialog(function (goods) {
    //        var result = $.grep(self.goods(), function (e) {
    //            return e.id() == goods.id;
    //        });
    //        if (result.length == 0) {
    //            self.goods.push(ko.mapping.fromJS(goods));
    //        }
    //    });
    //},

    //removeGoods: function (goods) {
    //    var self = this;
    //    alert(JSON.stringify(ko.toJS(self)));
    //    self.goodsInOrder.remove(function (item) {
    //        return item.id() == goods.id();
    //    });
    //},

    updateState: function() {
        var self = this;
        blockUI();
        postJSON("/api/shop/order/updateState", { orderId: self.orderId(), state: self.state() }, function(result) {
            if (result.succeed === true) {
            }
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
    },

    save: function(url, onSuccess) {
        var self = this;
        blockUI();
        postJSON(url, ko.toJS(self), function(result) {
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

    edit: function() {
        var self = this;
        self.dialog(function() {
            self.save("/api/shop/order/update");
        });
    },

    remove: function (item, parent) {
        if (item.orderId()) {
            blockUI();
            postJSON("/api/shop/order/" + item.orderId() + "/delete", "", function (result) {
                if (result.succeed === true) {
                    parent.orders.remove(item);                   
                }
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        } else {
            parent.orders.remove(item);
        }
    },

    dialog: function (onSuccess) {
        var self = ko.validatedObservable(this);
        var dialogContent = $("#orderTemplate").tmpl();
        var options = {
            title: "Заказ",
            width: 650,
            height: 580,
            resizable: false,
            modal: true,
            open: function () {
                
                ko.applyBindings(self, this);
            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {
                        

                        if (self.isValid()) {
                            if (onSuccess) {
                                onSuccess();
                            }
                            $(this).dialog("close");
                        }
                        else {
                            self.errors.showAllMessages();
                        }
                    }

                },
                {
                    text: "Отмена",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ],
            close: function () {
                $(this).dialog('destroy');
                dialogContent.remove();
            }
        };
        dialogContent.dialog(options);
        return dialogContent;
    },
}
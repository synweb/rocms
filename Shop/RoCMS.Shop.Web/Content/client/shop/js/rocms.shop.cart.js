/// <reference path="/Content/client/ro/js/rocms.app.client.js" />




App.Client.Order = {
    postCode: ko.observable(),
    city: ko.observable(),
    house: ko.observable(),
    street: ko.observable(),

    metro: ko.observable(),

    frontNumber: ko.observable(),
    houseIndex: ko.observable(),
    floor: ko.observable(),
    intercom: ko.observable(),

    appartment: ko.observable(),
    comment: ko.observable(),
    shipmentType: ko.observable("courier"),
    paymentType: ko.observable("cash"),
    deliveryPrice: ko.observable(),
    pickUpPointId: ko.observable()
};

App.Client.User = {
    clientId: ko.observable(0),
    email: ko.observable().extend({ email: true, required: true }),
    phone: ko.observable().extend({ required: true }),
    emailNotificationAllowed: ko.observable(true),
    smsNotificationAllowed: ko.observable(),
    userId: ko.observable(),
    name: ko.observable().extend({ required: true }),
    lastName: ko.observable(),
    secondName: ko.observable()
};

App.Client.Order.city.extend({ required: { onlyIf: function () { return App.Client.Order.shipmentType() === 'post' } } });
App.Client.User.lastName.extend({ required: { onlyIf: function () { return App.Client.Order.shipmentType() === 'post' } } });
App.Client.User.secondName.extend({ required: { onlyIf: function () { return App.Client.Order.shipmentType() === 'post' } } });

App.Client.Order.postCode.extend({ required: { onlyIf: function () { return App.Client.Order.shipmentType() === 'post' } } });
App.Client.Order.street.extend({ required: { onlyIf: function () { return App.Client.Order.shipmentType() === 'post' } } });
App.Client.Order.house.extend({ required: { onlyIf: function () { return App.Client.Order.shipmentType() === 'post' } } });

App.Client.Order.courierCity = ko.observable('').extend({ required: { onlyIf: function () { return App.Client.Order.shipmentType() === 'courier' } } });
App.Client.Order.pickUpCity = ko.observable('').extend({ required: { onlyIf: function () { return App.Client.Order.shipmentType() === 'selfPickup' } } });

App.Client.Order.courierCity.subscribe(function (val) {
    App.Client.Order.city(val);
});

App.Client.Order.pickUpCity.subscribe(function (val) {
    App.Client.Order.city(val);
});

App.Client.Order.pickUpPointId.extend({ required: { onlyIf: function () { return App.Client.Order.shipmentType() === 'selfPickup' } } });

var shopCartViewModel = {
    next: function () {

        if (!shopCartViewModel.contacts.isValid()) {
            shopCartViewModel.contacts.errors.showAllMessages();
        }
        return false;
    },

    pickUpPoints: ko.observableArray(),

    filterPickUpPoints: ko.observableArray(),

    withRegister: ko.observable(''),

    authorize: function () {


        shopCartViewModel.account.username(shopCartViewModel.client.email());

        postJSON("/Home/Login", { user: ko.toJS(shopCartViewModel.account) }, function (result) {
            if (result.Succeed === true) {
                App.Client.User.userId(result.Data.UserId);
                App.Account.userId(result.Data.UserId);
                App.Account.isAuthorized(true);
            } else {
                alert("Неправильное имя или пароль пользователя");
            }
        });
    },

    logout: function () {
        postJSON("/Home/LogoutWithoutRedirect", null, function (result) {
            if (result.Succeed === true) {
                App.Client.User.userId('');
                App.Account.isAuthorized(false);
                location.reload();
            } else {
                alert("Произошла ошибка!");
            }
        });
        return false;
    },

    termsAccepted: ko.observable(false),

    password: ko.validatedObservable({
        password: ko.observable().extend({ required: true }),
        confirmPassword: ko.observable().extend({ required: true })
    }),

    account: ko.validatedObservable(App.Account),

    client: ko.validatedObservable(App.Client.User),

    contacts: ko.validatedObservable({
        email: App.Client.User.email,
        phone: App.Client.User.phone,
        city: App.Client.Order.city,
        pickUpCity: App.Client.Order.pickUpCity,
        courierCity: App.Client.Order.courierCity
    }),

    updateUserDiscount: function () {
        postJSON("/api/shop/cart/user/discount/update", {

        }, function (result) {
            if (result.succeed === true) {
                shopCartViewModel.refreshCart();
            }
        });
    },

    setInfo: function (client) {
        shopCartViewModel.client().clientId(client.clientId);
        shopCartViewModel.client().email(client.email);
        shopCartViewModel.client().phone(client.phone);
        shopCartViewModel.client().emailNotificationAllowed(client.emailNotificationAllowed);
        shopCartViewModel.client().smsNotificationAllowed(client.smsNotificationAllowed);
        shopCartViewModel.client().userId(client.userId);
        shopCartViewModel.client().name(client.name);
        shopCartViewModel.client().lastName(client.lastName);
        shopCartViewModel.client().secondName(client.secondName);

        shopCartViewModel.order().postCode(client.address.postCode);
        shopCartViewModel.order().city(client.address.city);
        shopCartViewModel.order().courierCity(client.address.city);
        shopCartViewModel.order().pickUpCity(client.address.city);
        shopCartViewModel.order().house(client.address.house);
        shopCartViewModel.order().street(client.address.street);
        shopCartViewModel.order().metro(client.address.metro);
        shopCartViewModel.order().frontNumber(client.address.frontNumber);
        shopCartViewModel.order().houseIndex(client.address.houseIndex);
        shopCartViewModel.order().appartment(client.address.appartment);
        shopCartViewModel.order().floor(client.address.floor);
        shopCartViewModel.order().intercom(client.address.intercom);
    },

    clearInfo: function () {
        shopCartViewModel.client().clientId(0);
        shopCartViewModel.client().email("");
        shopCartViewModel.client().phone("");
        shopCartViewModel.client().emailNotificationAllowed(true);
        shopCartViewModel.client().smsNotificationAllowed(false);
        shopCartViewModel.client().userId(App.Account.userId());
        shopCartViewModel.client().name("");
        shopCartViewModel.client().lastName("");
        shopCartViewModel.client().secondName("");

        shopCartViewModel.order().postCode("");
        shopCartViewModel.order().city("");
        shopCartViewModel.order().courierCity("");
        shopCartViewModel.order().pickUpCity("");
        shopCartViewModel.order().house("");
        shopCartViewModel.order().street("");
        shopCartViewModel.order().metro("");
        shopCartViewModel.order().frontNumber("");
        shopCartViewModel.order().houseIndex("");
        shopCartViewModel.order().appartment("");
        shopCartViewModel.order().floor("");
        shopCartViewModel.order().intercom("");
    },

    loadClient: function () {
        if (shopCartViewModel.account().userId()) {
            getJSON("/api/shop/client/user/" + shopCartViewModel.account().userId() + "/get", "",
                function (result) {
                    if (result.succeed) {
                        var client = result.data;
                        shopCartViewModel.setInfo(client);

                    } else {
                        shopCartViewModel.clearInfo();
                    }
                });
        }
        else {
            shopCartViewModel.clearInfo();
        }
    },


    order: ko.validatedObservable(App.Client.Order),

    anotherCity: function () {
        $(".post-link").click();
    },

    cartSummary: ko.observable(),

    cart: ko.observable(),

    hasGoods: ko.observable(false),

    loadingCart: ko.observable(true),

    orderInProcess: ko.observable(false),

    processOrder: function () {

        if (shopCartViewModel.termsAccepted() === false) return false;

        //TODO: валидация
        var user = "";

        if (App.Account.isAuthorized() === false && shopCartViewModel.withRegister() === "true") {
            if (!shopCartViewModel.password.isValid()) {
                shopCartViewModel.password.errors.showAllMessages();
                return false;
            }
            user = {
                username: shopCartViewModel.client().email(),
                password: shopCartViewModel.password().password(),
                repeatPassword: shopCartViewModel.password().confirmPassword()
            }
        }

        if (shopCartViewModel.client.isValid() && shopCartViewModel.order.isValid()) {
            shopCartViewModel.orderInProcess(true);
            postJSON("/api/shop/cart/process", {
                order: ko.toJS(shopCartViewModel.order),
                client: ko.toJS(shopCartViewModel.client),
                user: user
            }, function (result) {
                if (result.succeed === true) {
                    if (user) {
                        shopCartViewModel.account().username(user.username);
                        shopCartViewModel.account().password(user.password);
                        shopCartViewModel.authorize();
                    }
                    shopCartViewModel.clear();
                    //alert("Ваша заявка принята");
                    if (shopCartViewModel.order().paymentType() == 'card') {
                        var orderId = result.data;
                        window.location.replace("/Shop/PayForOrder/" + orderId);
                    } else {
                        window.location.replace("/thankyou");
                    }
                } else {
                    alert(result.message);
                }
                shopCartViewModel.orderInProcess(false);
            });
        } else {

            shopCartViewModel.client.errors.showAllMessages();
            shopCartViewModel.order.errors.showAllMessages();

            $('.input-validation-error:first').focus();

            return false;
        }
    },

    refreshCart: function () {
        shopCartViewModel.loadingCart(true);
        getJSON("/api/shop/cart/get", "", function (cart) {

            var obj = ko.mapping.fromJS(cart);
            obj.summary = obj.summary.extend({ numeric: 2 });
            obj.discountedSummary = obj.discountedSummary.extend({ numeric: 2 });
            $(obj.cartItems()).each(function () {
                this.discountedPrice = this.discountedPrice.extend({ numeric: 2 });
                this.summary = this.summary.extend({ numeric: 2 });
            });

            obj.deliveryString = ko.observable('depend');



            obj.finalSummary = function () {


                var shipmentType = shopCartViewModel.order().shipmentType();
                var dsummary = parseFloat(obj.discountedSummary().replace(/[^0-9-.]/g, ''));


                if (shipmentType === 'courier' && shopCartViewModel.order().courierCity() || shipmentType === 'selfPickup' && shopCartViewModel.order().pickUpCity()) {

                    shipmentType === 'courier' ? shopCartViewModel.order().city(shopCartViewModel.order().courierCity()) : shopCartViewModel.order().city(shopCartViewModel.order().pickUpCity());

                    if (dsummary >= 3000) {
                        obj.deliveryString('free');
                        shopCartViewModel.order().deliveryPrice(0);
                        return obj.discountedSummary();
                    }
                    //else if (dsummary >= 1500) {
                    //    var cost = shopCartViewModel.order().shipmentType() == 'selfPickup'
                    //        ? selfPickUpCost : discountedDeliveryCost;
                    //    obj.deliveryString('on');
                    //    shopCartViewModel.order().deliveryPrice(parseFloat(cost));
                    //    var val = ko.observable(dsummary + parseFloat(cost)).extend({ numeric: 2 });
                    //    return val();
                    //}
                    else {
                        var cost = shopCartViewModel.order().shipmentType() == 'selfPickup'
                            ? selfPickUpCost : deliveryCost;
                        obj.deliveryString('on');
                        shopCartViewModel.order().deliveryPrice(parseFloat(cost));
                        var val = ko.observable(dsummary + parseFloat(cost)).extend({ numeric: 2 });
                        return val();
                    }

                } else {
                    shopCartViewModel.order().deliveryPrice("");
                    obj.deliveryString('depend');
                    return obj.discountedSummary();
                }


            };

            shopCartViewModel.cart(obj);
            shopCartViewModel.hasGoods($(obj.cartItems()).length > 0);
        }).always(function () {
            shopCartViewModel.loadingCart(false);
        });
    },

    addItem: function (heartId, count, packId) {
        postJSON("/api/shop/cart/" + heartId + "/" + packId + "/" + count + "/add", "", function (result) {
            if (result.succeed === true) {
                shopCartViewModel.refreshCartSummary();
            }
        });
    },

    changeItemCount: function (heartId, count, packId) {
        postJSON("/api/shop/cart/" + heartId + "/" + count + "/change", "", function (result) {
            if (result.succeed === true) {
                shopCartViewModel.refreshCartSummary();
                shopCartViewModel.refreshCart();
            }
        });
    },

    increaseItemCount: function (cartItem, increase) {
        var count = cartItem.quantity();
        if (count == 1 && increase === false) {
            return false;
        }

        increase === true ? count++ : count--;

        cartItem.quantity(count);
        postJSON("/api/shop/cart/" + cartItem.goodsItem.heartId() + "/" + cartItem.packId() + "/" + count + "/change", "", function (result) {
            if (result.succeed === true) {
                shopCartViewModel.refreshCartSummary();
                shopCartViewModel.refreshCart();
            }
        });
    },

    removeItem: function (heartId, packId) {
        postJSON("/api/shop/cart/" + heartId + "/" + packId + "/remove", "", function (result) {
            if (result.succeed === true) {
                shopCartViewModel.refreshCartSummary();
                shopCartViewModel.refreshCart();
            }
        });
    },

    clear: function () {
        postJSON("/api/shop/cart/clear", "", function (result) {
            if (result.succeed === true) {
                shopCartViewModel.refreshCartSummary();
                shopCartViewModel.refreshCart();
            }
        });
    },

    refreshCartSummary: function () {
        getJSON("/api/shop/cart/summary", "", function (cart) {
            var obj = ko.mapping.fromJS(cart);
            obj.cartClass = ko.computed(function () {
                return obj.quantity() != 0 ? "cart-full" : "";
            }, obj);
            obj.summary = obj.summary.extend({ numeric: 2 });
            shopCartViewModel.cartSummary(obj);
        });
    }
};

shopCartViewModel.order().pickUpCity.subscribe(function (newValue) {
    shopCartViewModel.filterPickUpPoints.removeAll();
    if (newValue) {
        $(ko.toJS(shopCartViewModel.pickUpPoints)).each(function () {
            if (this.city.toLowerCase() == $.trim(newValue.toLowerCase())) {
                shopCartViewModel.filterPickUpPoints.push(this);
            }
        });
    }
});

shopCartViewModel.passwordsForRegisterVisible = ko.computed(function () {
    return shopCartViewModel.withRegister() === 'true';
});

shopCartViewModel.authorizeFormVisible = ko.computed(function () {
    return shopCartViewModel.withRegister() === 'oldUser';
});

shopCartViewModel.accountInfoVisible = ko.computed(function () {
    return shopCartViewModel.account().isAuthorized() === false && shopCartViewModel.contacts.isValid();
});

shopCartViewModel.orderInfoVisible = ko.computed(function () {
    return shopCartViewModel.contacts.isValid();
});

shopCartViewModel.nextButtonVisible = ko.computed(function () {
    return !shopCartViewModel.contacts.isValid();
});

shopCartViewModel.noGoods = ko.computed(function () {
    return shopCartViewModel.hasGoods() !== true;
});

$(function () {
    shopCartViewModel.refreshCartSummary();
    ko.applyBindings(shopCartViewModel);
});

function onCartLoaded() {
    shopCartViewModel.refreshCart();
    shopCartViewModel.termsAccepted(false);
    //$("input.phone").mask('+7 (000) 000-00-00');

    getJSON("/api/shop/pickUpPoints/get", "", function (result) {
        $(result).each(function () {
            shopCartViewModel.pickUpPoints.push(ko.mapping.fromJS(this));
        });
        if (shopCartViewModel.order().pickUpCity()) {
            shopCartViewModel.filterPickUpPoints.removeAll();

            $(ko.toJS(shopCartViewModel.pickUpPoints)).each(function () {
                if (this.city.toLowerCase() === $.trim(shopCartViewModel.order().pickUpCity().toLowerCase())) {
                    shopCartViewModel.filterPickUpPoints.push(this);
                }
            });

        }
        unblockUI();
    });

    App.Account.userId.subscribe(function () {
        shopCartViewModel.loadClient();
        shopCartViewModel.updateUserDiscount();
    });
    if (App.Account.isAuthorized() === true) {
        shopCartViewModel.loadClient();
    }
    shopCartViewModel.updateUserDiscount();
}

function onBuy(event, cartUrl) {

    var source = event.target || event.srcElement;

    var $div = $('<div class="goods-in-cart-message"><span>Товар помещен в <a href="' + cartUrl + '">корзину</a> <em class="glyphicon glyphicon-shopping-cart"></em></span></div>');

    $(source).closest(".buttons").append($div);

    var btn = $(source);
    btn.html("В корзине").attr("href", cartUrl).removeAttr("data-bind").unbind("click");
    $div.show(100);
    setTimeout(function () {
        $div.hide(100);
    }, 5000);
}
function currenciesEditorLoaded(onSelected, context) {
    blockUI();
    var vm = {
        currencies: ko.observableArray(),

        selectCurrency: function (item) {
            if (onSelected) {
                onSelected(item);
            }
        },
        createCurrency: function () {
            var self = this;
            var currency = new App.Admin.Currency();
            currency.new(function () {
                self.currencies.push(currency);
            });
        },
        removeCurrency: function(item) {
            blockUI();
            var url = "/api/shop/currency/" + item.currencyId() + "/delete";
            postJSON(url, "", function (result) {
                if (result.succeed) {
                    vm.currencies.remove(item);
                }
                else {
                    alert("Не удалось удалить валюту");
                }
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        }
    }

    getJSON("/api/shop/currencies/get", {}, function (result) {
        $.each(result.data, function (index, el) {
            vm.currencies.push(new App.Admin.Currency(el));
        });
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });

    if (context) {
        ko.applyBindings(vm, context[0]);
    } else {
        ko.applyBindings(vm);
    }

}

App.Admin.Currency = function (data) {
    var self = this;
    self.name = ko.observable().extend({ required: true });
    self.shortName = ko.observable().extend({ required: true });
    self.rate = ko.observable().extend({ required: true });
    self.currencyId = ko.observable();
    self.init = function (data2) {
        self.name(data2.name);
        self.shortName(data2.shortName);
        self.rate(data2.rate);
        self.currencyId(data2.currencyId);
    }

    if (data) {
        self.init(data);
    }

    self.save = function (url, onSuccess) {
        blockUI();
        postJSON(url, ko.toJS(self), function (result) {
            if (result.succeed) {
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
    };

    self.edit = function () {
        var url = "/api/shop/currency/update";
        self.editDialog(function () {
            self.save(url);
        });
    };

    self.new = function (onSuccess) {
        var url = "/api/shop/currency/create";
        self.editDialog(function () {
            self.save(url, onSuccess);
        });
    };

    self.editDialog = function (onSuccess) {
        var dm = ko.validatedObservable(self);
        var dialogContent = $("#currency-item-template").tmpl();
        var options = {
            title: "Валюта",
            width: 700,
            height: 450,
            resizable: false,
            modal: true,
            open: function () {
                var that = this;
                ko.applyBindings(dm, that);
            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {
                        //var $form = $(this).find('form');

                        if (dm.isValid()) {
                            if (onSuccess) {
                                onSuccess();
                            }
                            $(this).dialog("close");
                        }
                        else {
                            dm.errors.showAllMessages();
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


    };

}

function showCurrenciesDialog(onSelected) {
    var options = {
        title: "Валюты",
        modal: true,
        draggable: false,
        resizable: false,
        width: 760,
        height: 550,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var that = this;
            currenciesEditorLoaded(function (item) {
                if (onSelected) {
                    onSelected({ id: item.currencyId(), name: item.shortName() });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/Currencies", options);
}
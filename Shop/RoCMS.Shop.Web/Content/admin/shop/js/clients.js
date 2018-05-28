function onShopClientsLoaded() {
    var vm = {
        clients: ko.observableArray(),
        hasMoreClients: ko.observable(),
        loadMoreClients: function () {
            blockUI();
            var index = vm.clients().length + 1;

            getJSON("/api/shop/clients/page/" + index + "/" + pageSize + "/get", "", function(result) {
                $(result.data.clients).each(function() {
                    vm.clients.push($.extend(ko.mapping.fromJS(this), App.Admin.ClientFunctions));
                });

                vm.hasMoreClients(vm.clients().length < result.data.total);
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        }
    };

    var pageSize = 10;

    blockUI();
    var blocks = 1;

    getJSON("/api/shop/clients/page/1/"+pageSize+"/get", "", function (result) {
        $(result.data.clients).each(function () {
            vm.clients.push($.extend(ko.mapping.fromJS(this), App.Admin.ClientFunctions));
        });

        vm.hasMoreClients(vm.clients().length < result.data.total);       
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


App.Admin.Client = {
    clientId: ko.observable(),
    email: ko.observable().extend({ email: true, required: true }),
    phone: ko.observable().extend({ required: true }),
    emailNotificationAllowed: ko.observable(true),
    smsNotificationAllowed: ko.observable(),
    userId: ko.observable(),
    name: ko.observable().extend({ required: true }),
    lastName: ko.observable(),
    comment: ko.observable(),
    initialAmount: ko.observable(),
}

App.Admin.ClientFunctions = {
    remove: function(item, parent) {
        if (item.clientId()) {
            if (!confirmRemoval()) {
                return;
            }
            blockUI();
            postJSON("/api/shop/client/" + item.clientId() + "/delete", "", function (result) {
                if (result.succeed === true) {
                    parent.clients.remove(item);                   
                }
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        } else {
            parent.clients.remove(item);
        }
    },

    save: function (url, onSuccess) {
        var self = this;
        blockUI();
        postJSON(url, ko.toJS(self), function (result) {
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
            self.save("/api/shop/client/update");
        });
    },

    dialog: function (onSuccess) {
        var self = ko.validatedObservable(this);
        var dialogContent = $("#clientEditTemplate").tmpl();
        var options = {
            title: "Клиент",
            width: 650,
            height: 420,
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


    updateEmail: function () {
        var self = this;
        showPromptDialog("Смена email",
            "Укажите новый email. Учтите, что email используется для авторизации пользователя в системе и должен быть уникален",
            self.email(), "Изменить", "Отмена",
            function (data) {
                var newEmail = data.promptValue;
                blockUI();
                postJSON("/api/shop/client/" + self.clientId() + "/email/" + newEmail + "/change", "", function (result) {
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
            });
    }
}
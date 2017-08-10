App.Admin.dimensions = ko.observableArray();

App.Admin.CompatibleValidationMapping = {
    // customize the creation of the name property so that it provides validation
    name: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    }
};

function compatiblesEditorLoaded(onSelected, context) {
    blockUI();
    getJSON("/api/shop/dimensions/get", "", function (result) {
        $(result).each(function () {
            App.Admin.dimensions.push(ko.mapping.fromJS(this));
        });
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });
    var vm = {
        compatibles: ko.observableArray(),
        createCompatible: function () {
            var self = this;
            var compatible = $.extend(new App.Admin.Compatible(), App.Admin.CompatibleFunctions);
            compatible.create(function () {
                self.compatibles.push(compatible);
            });
        },

        selectCompatible: function (item) {
            if (onSelected) {
                onSelected(item);
            }
        },

        dimensions: ko.observableArray()
    };

    getJSON("/api/shop/compatibles/get", "", function (result) {
        $(result).each(function () {
            var comp = $.extend(ko.mapping.fromJS(this, App.Admin.CompatibleValidationMapping), App.Admin.CompatibleFunctions);
            vm.compatibles.push(comp);
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

App.Admin.Compatible = function () {
    var self = this;

    self.compatibleSetId = ko.observable();
    self.name = ko.observable().extend({ required: true });
    self.compatibleGoods = ko.observableArray();
}

App.Admin.CompatibleFunctions = {
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

    edit: function () {
        var self = this;
        self.dialog(function () {
            self.save("/api/shop/compatible/update");
        });
    },

    remove: function (item, parent) {
        var self = this;
        if (self.compatibleSetId()) {
            blockUI();
            var url = "/api/shop/compatible/" + self.compatibleSetId() + "/delete";
            postJSON(url, "", function (result) {
                if (result.succeed) {
                    parent.compatibles.remove(item);
                }                
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        }
    },

    create: function (onSuccess) {
        var self = this;
        self.dialog(function () {
            self.save("/api/shop/compatible/create", function (result) {
                self.compatibleSetId(result.id);
                if (onSuccess) {
                    onSuccess();
                }
            });
        });
    },

    addGoods: function () {
        var self = this;
        showGoodsDialog(function (goods) {
            var result = $.grep(self.compatibleGoods(), function (e) {
                return e.id() == goods.id;
            });
            if (result.length == 0) {
                self.compatibleGoods.push(ko.mapping.fromJS(goods));
            }
        });
    },
    removeGoods: function (goods, parent) {
        parent.compatibleGoods.remove(function (item) {
            return item.id() == goods.id();
        });
    },

    dialog: function (onSuccess) {
        var self = ko.validatedObservable(this);
        var dialogContent = $("#compatibleTemplate").tmpl();
        var options = {
            title: "Комплект",
            width: 650,
            height: 450,
            resizable: false,
            modal: true,
            open: function () {

                ko.applyBindings({ vm: self }, this);
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
    }
}

function showCompatiblesDialog(onSelected) {
    blockUI();
    var options = {
        title: "Комплекты",
        modal: true,
        draggable: false,
        resizable: false,
        width: 760,
        height: 650,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var that = this;
            unblockUI();
            compatiblesEditorLoaded(function (item) {
                if (onSelected) {
                    onSelected({ compatibleSetId: item.compatibleSetId(), name: item.name() });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/CompatiblesEditor", options);
}

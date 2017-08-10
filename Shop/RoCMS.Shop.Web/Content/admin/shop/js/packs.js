App.Admin.dimensions = ko.observableArray();

function packsEditorLoaded(onSelected, context) {
    blockUI();
    var blocks = 2;
    getJSON("/api/shop/dimensions/get", "", function (result) {
        $(result).each(function () {
            App.Admin.dimensions.push(ko.mapping.fromJS(this));
        });
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            blocks--;
            if (blocks == 0) {
                unblockUI();
            }
        });
    var vm = {
        packs: ko.observableArray(),
        createPack: function () {
            var self = this;
            var pack = $.extend(new App.Admin.Pack(), App.Admin.PackFunctions);
            pack.create(function () {
                self.packs.push(pack);
            });
        },

        selectPack: function (item) {
            if (onSelected) {
                onSelected(item);
            }
        },

        dimensions: ko.observableArray()
    };

    getJSON("/api/shop/packs/get", "", function (result) {
        $(result).each(function () {
            vm.packs.push($.extend(ko.mapping.fromJS(this), App.Admin.PackFunctions));
        });       
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            blocks--;
            if (blocks == 0) {
                unblockUI();
            }
        });

    if (context) {
        ko.applyBindings(vm, context[0]);
    } else {
        ko.applyBindings(vm);
    }
}

App.Admin.CompatibleValidationMapping = {
    name: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    size: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true, number: true });
        }
    },
    dimensionId: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    }
};

App.Admin.Pack = function () {
    var self = this;

    self.packId = ko.observable();
    self.name = ko.observable().extend({ required: true });
    self.defaultDiscount = ko.observable();
    self.size = ko.observable().extend({ required: true, number: true });
    self.dimensionId = ko.observable().extend({ required: true });
}

App.Admin.PackFunctions = {
    save: function (url, onSuccess) {
        var self = this;
        blockUI();
        postJSON(url, ko.toJS(self), function (result) {
            if (result.succeed === true) {
                if (onSuccess) {
                    onSuccess(result.data);
                }               
            }
        }).fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
            .always(function () {
                unblockUI();
            });
    },

    edit: function () {
        var self = this;
        self.dialog(function () {
            self.save("/api/shop/pack/update");
        });
    },

    remove: function (item, parent) {
        var self = this;
        if (self.packId()) {
            blockUI();
            var url = "/api/shop/pack/" + self.packId() + "/delete";
            postJSON(url, "", function (result) {
                if (result.succeed) {
                    parent.packs.remove(item);
                }               
            }).fail(function () {
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
            self.save("/api/shop/pack/create", function (result) {
                self.packId(result.id);
                if (onSuccess) {
                    onSuccess();
                }
            });
        });
    },
    
    dialog: function (onSuccess) {
        var self = ko.validatedObservable(this);
        var dialogContent = $("#packTemplate").tmpl();
        var options = {
            title: "Упаковка",
            width: 650,
            height: 550,
            resizable: false,
            modal: true,
            open: function () {
                
                ko.applyBindings({ vm: self, dimensions: App.Admin.dimensions }, this);
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

function showPacksDialog(onSelected) {
    blockUI();
    var options = {
        title: "Упаковки",
        modal: true,
        draggable: false,
        resizable: false,
        width: 760,
        height: 550,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var that = this;
            unblockUI();
            packsEditorLoaded(function (item) {
                if (onSelected) {
                    onSelected({ packInfo: { packId: item.packId(), name: item.name() } });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/PacksEditor", options);
}

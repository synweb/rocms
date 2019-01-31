/// <reference path="/Content/admin/ro/js/rocms.heart.js" />
/// <reference path="base.js" />

App.Admin.countries = ko.observableArray();

function manufacturersEditorLoaded(onSelected, context) {
    blockUI();
    var vm = {
        manufacturers: ko.observableArray(),
        createManufacturer: function () {
            var self = this;
            var manufacturer = $.extend(new App.Admin.Shop.Manufacturer(), App.Admin.Shop.ManufacturerFunctions);
            manufacturer.create(function () {
                self.manufacturers.push(manufacturer);
            });
        },
        selectManufacturer: function (item) {
            if (onSelected) {
                onSelected(item);
            }
        }
    };

    var blocks = 2;
    getJSON("/api/shop/manufacturers/get", "", function (result) {
        $(result).each(function () {
            var res = $.extend(ko.mapping.fromJS(this, App.Admin.Shop.ManufacturerValidationMapping), App.Admin.Shop.ManufacturerFunctions);
            vm.manufacturers.push(res);
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

    App.Admin.countries.push(ko.mapping.fromJS({ countryId: null, name: 'Не задана' }));
    getJSON("/api/shop/countries/get", "", function (result) {
        $(result).each(function () {
            App.Admin.countries.push(ko.mapping.fromJS(this));
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


App.Admin.Shop.ManufacturerValidationMapping = {
    name: {
        create: function (options) {
            var res = ko.observable(options.data).extend({ required: true });
            return res;
        }
    }
};

$.extend(App.Admin.Shop.ManufacturerValidationMapping, App.Admin.HeartValidationMapping);

App.Admin.Shop.Manufacturer = function () {
    var self = this;

    $.extend(self, new App.Admin.Heart());

    self.heartId = ko.observable();
    self.name = ko.observable().extend({ required: true });;
    self.countryId = ko.observable();
    self.logoImageId = ko.observable();
    self.description = ko.observable();

}

App.Admin.Shop.ManufacturerFunctions = {
    initManufacturer: function () {
        var self = this;
        self.initHeart();

        self.name.subscribe(function (val) {
            if (val) {
                if (!self.title()) {
                    self.title(val);
                }
                if (!self.description()) {
                    self.description(val);
                }

            }
        });

    },

    fetch: function () {
        var self = this;
        if (self.heartId()) {
            blockUI();
            postJSON("/api/shop/manufacturer/" + self.heartId() + "/get", "", function (result) {
                self.init(result);
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
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

    edit: function (onSuccess) {
        var self = this;
        self.dialog("/api/shop/manufacturer/update", function () {
            if (onSuccess) {
                onSuccess();
            }
        });
    },

    remove: function (item, parent) {
        var self = this;
        if (self.heartId()) {
            blockUI();
            var url = "/api/shop/manufacturer/" + self.heartId() + "/delete";
            postJSON(url, "", function (result) {
                if (result.succeed) {
                    parent.manufacturers.remove(item);
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

    pickImage: function () {
        var self = this;
        showImagePickDialog(function (imageData) {
            self.logoImageId(imageData.ID);
        });
    },

    create: function (onSuccess) {
        var self = this;

        self.dialog("/api/shop/manufacturer/create", function () {
            if (onSuccess) {
                onSuccess();
            }
        });
    },

    dialog: function (url, onSave) {
        var self = this;
        var dm = ko.validatedObservable(self);
        var dialogContent = $("#manufacturerTemplate").tmpl();
        var options = {
            title: "Производитель",
            width: 950,
            height: 650,
            resizable: false,
            modal: true,
            open: function () {
                var $form = $(this).find('form');

                if ($("#manufacturerDescription", $form).length) {
                    $("#manufacturerDescription", $form).val(self.description());
                    initContentEditor();
                }
                self.initManufacturer();
                var that = this;
                ko.applyBindings({ vm: dm, countries: App.Admin.countries }, that);


            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {
                        var $dialog = $(this);

                        var text = getTextFromEditor('manufacturerDescription');
                        if (text) {
                            self.description(text);
                        }


                        if (dm.isValid()) {


                            self.save(url, function (data) {
                                if (data) {
                                    self.heartId(data.id);
                                    if (onSave) {
                                        onSave();
                                    }
                                    $dialog.dialog("close");

                                }
                            });

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
    }
}


$.extend(App.Admin.Shop.ManufacturerFunctions, App.Admin.HeartFunctions);


function showManufacturersDialog(onSelected) {
    var options = {
        title: "Производители",
        modal: true,
        draggable: false,
        resizable: false,
        width: 760,
        height: 550,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var that = this;
            manufacturersEditorLoaded(function (item) {
                if (onSelected) {
                    onSelected({ id: item.heartId(), name: item.name() });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/ManufacturersEditor", options);
}
App.Admin.countries = ko.observableArray();

function manufacturersEditorLoaded(onSelected, context) {
    blockUI();
    var vm = {
        manufacturers: ko.observableArray(),
        createManufacturer: function () {
            var self = this;
            var manufacturer = new App.Admin.Manufacturer();
            manufacturer.new(function () {
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
            vm.manufacturers.push(new App.Admin.Manufacturer(this));
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




App.Admin.Manufacturer = function (data) {
    var self = this;

    self.manufacturerId = ko.observable();
    self.name = ko.observable().extend({ required: true });;
    self.countryId = ko.observable();
    self.logoImageId = ko.observable();
    self.description = ko.observable();
    self.url = ko.observable();

    self.init = function (item) {
        self.manufacturerId(item.manufacturerId);
        self.name(item.name);
        self.countryId(item.countryId);
        self.logoImageId(item.logoImageId);
        self.description(item.description);
        self.url(item.url);
    };

    if (data) {
        self.init(data);
    }

    self.fetch = function () {
        if (self.manufacturerId()) {
            blockUI();
            postJSON("/api/shop/manufacturer/" + self.manufacturerId() + "/get", "", function (result) {
                self.init(result); 
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        }
    };

    self.save = function (url, onSuccess) {
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
    };

    self.edit = function () {
        self.dialog(function () {
            self.save("/api/shop/manufacturer/update");
        });
    };

    self.remove = function (item, parent) {
        if (self.manufacturerId()) {
            blockUI();
            var url = "/api/shop/manufacturer/" + self.manufacturerId() + "/delete";
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
    };

    self.pickImage = function () {
        showImagePickDialog(function (imageData) {
            self.logoImageId(imageData.ID);
        });
    };

    self.new = function (onSuccess) {
        self.dialog(function () {
            self.save("/api/shop/manufacturer/create", function (result) {
                self.manufacturerId(result.id);
                if (onSuccess) {
                    onSuccess();
                }
            });
        });
    };

    self.dialog = function (onSuccess) {
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

                var that = this;
                ko.applyBindings({ vm: dm, countries: App.Admin.countries }, that);
                
                
            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {

                        var text = getTextFromEditor('manufacturerDescription');
                        if (text) {
                            self.description(text);
                        }


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
                    onSelected({ id: item.manufacturerId(), name: item.name() });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/ManufacturersEditor", options);
}
function actionsEditorLoaded(onSelected, context) {
    blockUI();
    var vm = {
        actions: ko.observableArray(),
        createAction: function() {
            var self = this;
            var action = $.extend(new App.Admin.Action(), App.Admin.ActionFunctions);
            action.create(function() {
                self.actions.push(action);
            });
        },

        selectAction: function (item) {
            if (onSelected) {
                onSelected(item);
            }
        }
    };

    getJSON("/api/shop/actions/get", "", function (result) {
        $(result).each(function () {
            var obj = $.extend(ko.mapping.fromJS(this), App.Admin.ActionFunctions);
			if(obj.dateOfEnding()) {
				obj.dateOfEnding(dateFormat(obj.dateOfEnding(), 'dd.mm.yyyy'));
			}
            vm.actions.push(obj);
			
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

App.Admin.ActionValidationMapping = {
    name: {
        create: function(options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    description: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    discount: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true, number: true });
        }
    }
};

App.Admin.Action = function() {
    var self = this;

    self.actionId = ko.observable();
    self.name = ko.observable().extend({ required: true });;
    self.description = ko.observable().extend({ required: true });;
    self.dateOfEnding = ko.observable();
    self.discount = ko.observable().extend({ required: true });;
    self.goods = ko.observableArray();
    self.categories = ko.observableArray();
    self.manufacturers = ko.observableArray();
    self.imageId = ko.observable();
    self.active = ko.observable(false);
    self.showInSlider = ko.observable(true);
}

App.Admin.ActionFunctions = {
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
            self.save("/api/shop/action/update");
        });
    },

    remove: function (item, parent) {
        var self = this;
        if (self.actionId()) {
            blockUI();
            var url = "/api/shop/action/" + self.actionId() + "/delete";
            postJSON(url, "", function (result) {
                if (result.succeed) {
                    parent.actions.remove(item);
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
            self.save("/api/shop/action/create", function (result) {
                self.actionId(result.id);
                if (onSuccess) {
                    onSuccess();
                }
            });
        });
    },
    pickImage: function () {
        var self = this;
        showImagePickDialog(function (imageData) {
            self.imageId(imageData.ID);
        });
    },
    dialog: function (onSuccess) {
        var self = ko.validatedObservable(this);
        var dialogContent = $("#actionTemplate").tmpl();
        var options = {
            title: "Акция",
            width: 950,
            height: 650,
            resizable: false,
            modal: true,
            open: function () {


                var $form = $(this).find('form');

                if ($("#actionDescription", $form).length) {
                    $("#actionDescription", $form).val(self().description());
                    initContentEditor();
                }

                ko.applyBindings(self, this);
            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {


                        var text = getTextFromEditor('actionDescription');
                        if (text) {
                            self().description(text);
                        }

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
    addCategory: function () {
        var self = this;
        showCategoriesDialog(function (category) {
            var result = $.grep(self.categories(), function (e) {
                return e.id() == category.id;
            });
            if (result.length == 0) {
                self.categories.push(ko.mapping.fromJS(category));
            }
        });
    },
    removeCategory: function (category, parent) {
        parent.categories.remove(function (item) {
            return item.id() == category.id();
        });
    },
    addManufacturer: function () {
        var self = this;
        showManufacturersDialog(function (manufacturer) {
            var result = $.grep(self.manufacturers(), function (e) {
                return e.id() == manufacturer.id;
            });
            if (result.length == 0) {
                self.manufacturers.push(ko.mapping.fromJS(manufacturer));
            }
        });
    },
    removeManufacturer: function (manufacturer, parent) {
        parent.manufacturers.remove(function (item) {
            return item.id() == manufacturer.id();
        });
    },
    addGoods: function () {
        var self = this;
        showGoodsDialog(function (goods) {
            var result = $.grep(self.goods(), function (e) {
                return e.id() == goods.id;
            });
            if (result.length == 0) {
                self.goods.push(ko.mapping.fromJS(goods));
            }
        });
    },
    removeGoods: function (goods, parent) {
        parent.goods.remove(function (item) {
            return item.id() == goods.id();
        });
    }
}

function showActionsDialog(onSelected) {
    var options = {
        title: "Акции",
        modal: true,
        draggable: false,
        resizable: false,
        width: 960,
        height: 650,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var that = this;
            actionsEditorLoaded(function (item) {
                if (onSelected) {
                    onSelected({ id: item.actionId(), name: item.name() });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/ActionsEditor", options);
}
/// <reference path="base.js" />
/// <reference path="/Content/admin/ro/js/rocms.heart.js" />

function actionsEditorLoaded(onSelected, context) {
    blockUI();
    var vm = {
        actions: ko.observableArray(),
        parents: ko.observableArray(),
        createAction: function() {
            var self = this;
            var action = $.extend(new App.Admin.Shop.Action(), App.Admin.Shop.ActionFunctions);
            action.parents = vm.parents;
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

    vm.parents.push({ title: "Нет", heartId: null, type: "Выберите..." });


    blockUI();
    $.when(
        getJSON("/api/heart/hearts/RoCMS.Web.Contract.Models.Page/get", "", function (res) {
            $(res).each(function () {
                vm.parents.push(this);
            });
        }),
        getJSON("/api/shop/actions/get", "", function (result) {
            $(result).each(function () {
                var obj = $.extend(ko.mapping.fromJS(this), App.Admin.Shop.ActionFunctions);
                if(obj.dateOfEnding()) {
                    obj.dateOfEnding(dateFormat(obj.dateOfEnding(), 'dd.mm.yyyy'));
                }
                obj.parents = vm.parents();
                vm.actions.push(obj);
			
            });
        })

    ).then(
        function () {
            if (context) {
                ko.applyBindings(vm, context[0]);
            } else {
                ko.applyBindings(vm);
            }

        },
        function () {
            smartAlert("Произошла ошибка");
        }
    ).always(function () {
        unblockUI();
    });

}

App.Admin.Shop.ActionValidationMapping = {
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

$.extend(App.Admin.Shop.ActionValidationMapping, App.Admin.HeartValidationMapping);

App.Admin.Shop.Action = function () {
    var self = this;

    $.extend(self, new App.Admin.Heart());

    self.heartId = ko.observable();
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



App.Admin.Shop.ActionFunctions = {
    initAction: function() {
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

        self.dialog("/api/shop/action/update", function () {
            if (onSuccess) {
                onSuccess();
            }
        });
    },

    remove: function (item, parent) {
        var self = this;
        if (self.heartId()) {
            blockUI();
            var url = "/api/shop/action/" + self.heartId() + "/delete";
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
        self.dialog("/api/shop/action/create", function () {
            if (onSuccess) {
                onSuccess();
            }
        });
    },
    pickImage: function () {
        var self = this;
        showImagePickDialog(function (imageData) {
            self.imageId(imageData.ID);
        });
    },

    dialog: function (url, onSave) {
        var self = this;
        var dm = ko.validatedObservable(this);
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
                    $("#actionDescription", $form).val(self.description());
                    initContentEditor();
                }

                self.initAction();

                ko.applyBindings(self, this);
                $(".withsearch").selectpicker();
            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {
                        var $dialog = $(this);

                        var text = getTextFromEditor('actionDescription');
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

$.extend(App.Admin.Shop.ActionFunctions, App.Admin.HeartFunctions);

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
                    onSelected({ id: item.heartId(), name: item.name() });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/ActionsEditor", options);
}
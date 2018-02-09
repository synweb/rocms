/// <reference path="/Content/base/ro/js/rocms.helpers.js" />

function mapCategoriesToIds(categories) {
    var result = $(categories).map(function () {
        return {
            categoryId: this.categoryId,
            childrenCategories: this.childrenCategories ? mapCategoriesToIds(this.childrenCategories) : []
        }
    }).get();
    return result;
}

function categoriesEditorLoaded(onSelected, context) {
    blockUI();
    var vm = {
        childrenCategories: ko.observableArray(),
        orderEditingEnabled: ko.observable(false),
        createCategory: function() {
            var self = this;
            var category = new App.Admin.Category();
            category.new(function() {
                self.childrenCategories.push(category);
            });
        },
        selectCategory: function(item) {
            if (onSelected) {
                onSelected(item);
            }
        },
        enableEditOrder: function () {
            this.orderEditingEnabled(!this.orderEditingEnabled());
        },
        saveOrder: function () {
            blockUI();

            var cats = mapCategoriesToIds(ko.toJS(this.childrenCategories));


            postJSON("/api/shop/categories/order/update", cats,
                function (result) {

                })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        }
    };
    getJSON("/api/shop/categories/get", "", function (result) {
        $(result).each(function () {
            vm.childrenCategories.push(new App.Admin.Category(this));
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

App.Admin.CategoryValidationMapping = {

};

$.extend(App.Admin.CategoryValidationMapping, App.Admin.HeartValidationMapping);

App.Admin.Category = function (data) {
    var self = this;


    $.extend(self, new App.Admin.Heart());

    self.categoryId = ko.observable();
    self.name = ko.observable().extend({ required: true });
    self.description = ko.observable();
    self.parentCategoryId = ko.observable();
    self.imageId = ko.observable();
    self.metaDescription = ko.observable();
    self.childrenCategories = ko.observableArray();
    self.parentCategory = ko.observable({ name: "" });
    self.hidden = ko.observable(false);
    self.relativeUrl = ko.observable().extend({ required: true });
    self.orderFormSpecs = ko.observableArray();


    self.name.subscribe(function(val) {
        if (!self.relativeUrl() && val) {
            self.relativeUrl(textToUrl(val));
        }
    });

    self.init = function (data) {
        self.categoryId(data.categoryId);
        self.parentCategoryId(data.parentCategoryId);
        self.name(data.name);
        self.description(data.description);
        self.metaDescription(data.metaDescription);
        self.imageId(data.imageId);
        self.hidden(data.hidden);
        self.relativeUrl(data.relativeUrl);
        if (data.parentCategory) {
            self.parentCategory(data.parentCategory);
        }
        $(data.childrenCategories).each(function () {
            self.childrenCategories.push(new App.Admin.Category(this));
        });
        $(data.orderFormSpecs).each(function () {
            self.orderFormSpecs.push(new App.Admin.Spec(this));
        });
    };

    self.fetch = function() {
    };

    self.addChild = function () {
        var category = new App.Admin.Category();
        category.parentCategoryId(self.categoryId());
        category.parentCategory().name = self.name();
        category.new(function () {
            self.childrenCategories.push(category);
        });
    };

    self.clearParentCategory = function() {
        self.parentCategory({name : ""});
        self.parentCategoryId("");
    };
    self.editParentCategory = function () {
        showCategoriesDialog(function (result) {
            if (result.id != self.categoryId()) {
                self.parentCategory(result);
                self.parentCategoryId(result.id);
            }
            else {
                alert("Нельзя установить родительской категорией текущую категорию");
            }
        });
    };
    self.new = function (onCreate) {
        self.dialog(function(closeDialog) {
            var url = "/api/shop/category/create";
            self.save(url, function (result) {
                if (closeDialog) {
                    closeDialog();
                }
                self.categoryId(result.id);
                if (onCreate) {
                    onCreate();
                }
            });
        });
    };

    self.edit = function () {
        var url = "/api/shop/category/update";
        self.dialog(function (closeDialog) {
            self.save(url, function () {
                if (closeDialog) {
                    closeDialog();
                }
            });
        });
    };

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

    self.remove = function (item, parent) {
        if (self.categoryId()) {
            blockUI();
            var url = "/api/shop/category/" + self.categoryId() + "/delete";
            postJSON(url, "", function (result) {
                if (result.succeed) {
                    parent.childrenCategories.remove(item);
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
            self.imageId(imageData.ID);
            $('.remove-image').show();
        });
    };

    self.removeImage = function() {
        self.imageId("");
        $('.remove-image').hide();
    };

    self.addSpec= function () {
        showSpecDialog(function (item) {
            var result = $.grep(self.orderFormSpecs(), function (e) {
                return e.specId() === item.specId();
            });
            if (result.length === 0) {
                self.orderFormSpecs.push(item);
            }
        });
    },
    self.removeSpec= function (spec, parent) {
        parent.orderFormSpecs.remove(function (item) {
            return item.specId() === spec.specId();
        });
    },

    self.moveUp = function (item, parent) {
        var index = parent.childrenCategories.indexOf(item);
        if (index <= 0) return false;

        parent.childrenCategories.remove(item);
        parent.childrenCategories.splice(index - 1, 0, item);
    };

    self.moveDown = function (item, parent) {
        var index = parent.childrenCategories.indexOf(item);
        if (index == parent.childrenCategories.length - 1) return false;

        parent.childrenCategories.remove(item);
        parent.childrenCategories.splice(index + 1, 0, item);
    };



    self.dialog = function (onSuccess) {
        var dm = ko.validatedObservable(self);
        var dialogContent = $("#categoryTemplate").tmpl();
        var options = {
            title: "Категория",
            width: 900,
            height: 650,
            resizable: false,
            modal: true,
            open: function () {
                var $form = $(this).find('form');

                if ($("#categoryDescription", $form).length) {
                    $("#categoryDescription", $form).val(self.description());
                    initContentEditor();
                }

                self.initHeart();

                var that = this;
                
                ko.applyBindings(dm, that);
            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {
                        var $form = $(this).find('form');

                        var text = getTextFromEditor('categoryDescription');
                        if (text) {
                            self.description(text);
                        }

                        self.prepareHeartForUpdate();

                        var $dialog = $(this);

                        if ($("#categoryDescription", $form).length) {
                            $("#categoryDescription", $form).val(self.description());
                            initContentEditor();
                        }
                       
                        if (dm.isValid()) {
                            if (onSuccess) {
                                onSuccess(function () {
                                    $dialog.dialog("close");
                                });
                            }
                            
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

    if (data)
        self.init(data);
}


$.extend(App.Admin.Category, App.Admin.HeartFunctions);

function showCategoriesDialog(onSelected) {
    var options = {
        title: "Категории",
        modal: true,
        draggable: false,
        resizable: false,
        width: 760,
        height: 550,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var that = this;
            categoriesEditorLoaded(function (item) {
                if (onSelected) {
                    onSelected({ id: item.categoryId(), name: item.name() });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/CategoriesEditor", options);
}
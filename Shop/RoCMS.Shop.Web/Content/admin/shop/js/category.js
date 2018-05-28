/// <reference path="/Content/base/ro/js/rocms.helpers.js" />
/// <reference path="base.js" />

function mapCategoriesToIds(categories) {
    var result = $(categories).map(function () {
        return {
            heartId: this.heartId,
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
        createCategory: function () {
            var self = this;

            var category = $.extend(new App.Admin.Shop.Category(), App.Admin.Shop.CategoryFunctions);
            category.newCategory(function () {
                self.childrenCategories.push(category);
            });
        },
        selectCategory: function (item) {
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
            var res = $.extend(ko.mapping.fromJS(this, App.Admin.Shop.CategoryValidationMapping), App.Admin.Shop.CategoryFunctions);
            vm.childrenCategories.push(res);
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

App.Admin.Shop.CategoryValidationMapping = {
    name: {
        create: function (options) {
            var res = ko.observable(options.data).extend({ required: true });
            return res;
        }
    },
    childrenCategories: {
        create: function (options) {
            //пишется для одного элемента массива
            var res = $.extend(ko.mapping.fromJS(options.data, App.Admin.Shop.CategoryValidationMapping), App.Admin.Shop.CategoryFunctions);
            return res;
        }
    },
    parentCategory: {
        create: function (options) {
            if (options.data) {
                var res = ko.observable(options.data);
                return res;
            } else {
                return ko.observable({ name: "" });
            }
        }
    }
};

$.extend(App.Admin.Shop.CategoryValidationMapping, App.Admin.HeartValidationMapping);

App.Admin.Shop.Category = function () {
    var self = this;


    $.extend(self, new App.Admin.Heart());

    self.name = ko.observable().extend({ required: true });
    self.description = ko.observable();
    self.parentCategoryId = ko.observable();
    self.imageId = ko.observable();

    self.childrenCategories = ko.observableArray();
    self.parentCategory = ko.observable({ name: "" });
    self.hidden = ko.observable(false);

    self.orderFormSpecs = ko.observableArray();




    //self.init = function (data) {
    //    self.heartId(data.heartId);
    //    self.parentCategoryId(data.parentCategoryId);
    //    self.name(data.name);
    //    self.description(data.description);

    //    self.imageId(data.imageId);
    //    self.hidden(data.hidden);

    //    if (data.parentCategory) {
    //        self.parentCategory(data.parentCategory);
    //    }
    //    $(data.childrenCategories).each(function () {
    //        self.childrenCategories.push(new App.Admin.Shop.Category(this));
    //    });
    //    $(data.orderFormSpecs).each(function () {
    //        self.orderFormSpecs.push(new App.Admin.Spec(this));
    //    });
    //};



    //if (data)
    //    self.init(data);
}

App.Admin.Shop.CategoryFunctions = {
    initCategory: function () {
        var self = this;
        self.initHeart();

        //var cats = self.childrenCategories();
        //self.childrenCategories.removeAll();
        //$(cats).each(function () {

        //    var res = $.extend(ko.mapping.fromJS(this, App.Admin.Shop.CategoryValidationMapping), App.Admin.Shop.CategoryFunctions);
        //    res.initCategory();
        //    self.childrenCategories.push(res);
        //});

        //$(data.orderFormSpecs).each(function () {
        //    self.orderFormSpecs.push(new App.Admin.Spec(this));
        //});

        if ($("#categoryDescription").length) {
            $("#categoryDescription").val(self.description());
            initContentEditor();
        }

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

    prepareCategoryForUpdate: function () {
        var self = this;
        self.prepareHeartForUpdate();

        var text = getTextFromEditor('categoryDescription');
        if (text) {
            self.description(text);
        }

    },

    addChild: function () {
        var self = this;
        var category = $.extend(new App.Admin.Shop.Category(), App.Admin.Shop.CategoryFunctions);
        category.parentCategoryId(self.heartId());
        category.parentCategory().name = self.name();
        category.parentCategory().id = self.heartId();
        category.parentHeartId(self.heartId());
        category.newCategory(function () {
            self.childrenCategories.push(category);
        });
    },

    clearParentCategory: function () {
        var self = this;
        self.parentCategoryId("");
        self.parentCategory({ name: "" });
    },

    editParentCategory: function () {
        var self = this;
        showCategoriesDialog(function (result) {
            if (result.id != self.heartId()) {
                self.parentCategory(result);
                self.parentCategoryId(result.id);
            }
            else {
                alert("Нельзя установить родительской категорией текущую категорию");
            }
        });
    },
    newCategory: function (onCreate) {
        var self = this;
        self.dialog("/api/shop/category/create", function () {
            if (onCreate) {
                onCreate();
            }
        });

    },

    edit: function () {
        var self = this;
        self.dialog("/api/shop/category/update", function () {
            
        });
    },

    save: function (url, onSuccess) {
        var self = this;
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
    },

    remove: function (item, parent) {
        var self = this;
        if (self.heartId()) {
            blockUI();
            var url = "/api/shop/category/" + self.heartId() + "/delete";
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
    },

    pickImage: function () {
        var self = this;
        showImagePickDialog(function (imageData) {
            self.imageId(imageData.ID);
            $('.remove-image').show();
        });
    },

    removeImage: function () {
        var self = this;
        self.imageId("");
        $('.remove-image').hide();
    },

    addSpec: function () {
        var self = this;
        showSpecDialog(function (item) {
            var result = $.grep(self.orderFormSpecs(), function (e) {
                return e.specId() === item.specId();
            });
            if (result.length === 0) {
                self.orderFormSpecs.push(item);
            }
        });
    },
    removeSpec: function (spec, parent) {
        var self = this;
        parent.orderFormSpecs.remove(function (item) {
            return item.specId() === spec.specId();
        });
    },

    moveUp: function (item, parent) {
        var self = this;
        var index = parent.childrenCategories.indexOf(item);
        if (index <= 0) return false;

        parent.childrenCategories.remove(item);
        parent.childrenCategories.splice(index - 1, 0, item);
    },

    moveDown: function (item, parent) {
        var self = this;
        var index = parent.childrenCategories.indexOf(item);
        if (index == parent.childrenCategories.length - 1) return false;

        parent.childrenCategories.remove(item);
        parent.childrenCategories.splice(index + 1, 0, item);
    },



    dialog: function (url, onSuccess) {
        var self = this;
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

                self.initCategory();
                var parents = ko.observableArray();
                parents.push({ title: "Нет", heartId: null, type: "Выберите..." });

                if (self.parentCategoryId() && self.parentCategory()) {
                    parents.push({ heartId: self.parentCategory().id, title: self.parentCategory().name, type: 'Категории' });
                    
                }

                var that = this;

                var vm = {
                    dm: dm,
                    parents: parents
                }

                self.parentCategory.subscribe(function () {

                    vm.parents.removeAll();
                    vm.parents.push({ title: "Нет", heartId: null, type: "Выберите..." });
                    if (self.parentCategory().name) {
                        vm.parents.push({
                            heartId: self.parentCategory().id,
                            title: self.parentCategory().name,
                            type: 'Категории'
                        });
                        self.parentHeartId(self.parentCategory().id);
                    }

                    self.parentHeartId.notifySubscribers();

                    
                    setTimeout(function () {
                        $(".withsearch").selectpicker('refresh');

                    }, 100);
                });

                ko.applyBindings(vm, that);

                setTimeout(function() {
                    $(".withsearch").selectpicker();

                }, 100);
                

            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {
                        var $form = $(this).find('form');

                        self.prepareCategoryForUpdate();

                        var $dialog = $(this);

                        //if ($("#categoryDescription", $form).length) {
                        //    $("#categoryDescription", $form).val(self.description());
                        //    initContentEditor();
                        //}



                        if (dm.isValid()) {
                            self.save(url,
                                function (result) {
                                    if (result) {
                                        self.heartId(result.id);
                                    }
                                    if (onSuccess) {
                                        onSuccess();
                                    }
                                    $dialog.dialog("close");
                                });
                        } else {
                            dm.errors.showAllMessages();
                        }
                    }
                },
                {
                    text: "Закрыть",
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

};

$.extend(App.Admin.Shop.CategoryFunctions, App.Admin.HeartFunctions);

function showCategoriesDialog(onSelected) {
    var options = {
        title: "Категории",
        modal: true,
        draggable: false,
        resizable: false,
        width: 900,
        height: 650,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var that = this;
            categoriesEditorLoaded(function (item) {
                if (onSelected) {
                    onSelected({ id: item.heartId(), name: item.name() });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/CategoriesEditor", options);
}
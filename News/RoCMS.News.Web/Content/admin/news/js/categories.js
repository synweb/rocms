

function newsCategoriesEditorLoaded(onSelected, context) {

    function mapCategoriesToIds(categories) {
        var result = $(categories).map(function () {
            return {
                categoryId: this.categoryId,
                childrenCategories: this.childrenCategories ? mapCategoriesToIds(this.childrenCategories) : []
            }
        }).get();
        return result;
    }

    blockUI();
    var vm = {
        childrenCategories: ko.observableArray(),
        orderEditingEnabled: ko.observable(false),
        createCategory: function () {
            var self = this;
            var category = new App.Admin.News.Category();
            category.new(function () {
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
            postJSON("/api/news/categories/order/update", cats,
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
    getJSON("/api/news/categories/get", "", function (result) {
        $(result).each(function () {
            vm.childrenCategories.push(new App.Admin.News.Category(this));
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


App.Admin.News.Category = function(data) {
    var self = this;

    self.relativeUrl = ko.observable();
    self.categoryId = ko.observable();
    self.name = ko.observable().extend({ required: true });
    self.childrenCategories = ko.observableArray();
    self.parentCategoryId = ko.observable();
    self.parentCategory = ko.observable({ name: "" });
    self.hidden = ko.observable(false);

    //self.relativeUrl = ko.observable().extend({ required: true });


    //self.name.subscribe(function (val) {
    //    if (!self.relativeUrl() && val) {
    //        self.relativeUrl(toTranslit(val));
    //    }
    //});

    self.init = function(data) {
        self.relativeUrl(data.relativeUrl);
        self.categoryId(data.categoryId);
        self.parentCategoryId(data.parentCategoryId);
        self.name(data.name);
        self.hidden(data.hidden);
        //self.relativeUrl(data.relativeUrl);
        if (data.parentCategory) {
            self.parentCategory(data.parentCategory);
        }
        $(data.childrenCategories).each(function() {
            self.childrenCategories.push(new App.Admin.News.Category(this));
        });
    };

    self.fetch = function() {
    };

    self.addChild = function() {
        var category = new App.Admin.News.Category();
        category.parentCategoryId(self.categoryId());
        category.parentCategory().name = self.name();
        category.new(function() {
            self.childrenCategories.push(category);
        });
    };

    self.clearParentCategory = function() {
        self.parentCategory({ name: "" });
        self.parentCategoryId("");
    };
    self.editParentCategory = function() {
        showNewsCategoriesDialog(function(result) {
            if (result.id != self.categoryId()) {
                self.parentCategory(result);
                self.parentCategoryId(result.id);
            } else {
                alert("Нельзя установить родительской категорией текущую категорию");
            }
        });
    };
    self.new = function(onCreate) {
        self.dialog(function() {
            var url = "/api/news/category/create";
            self.save(url, function(result) {
                self.categoryId(result.id);
                if (onCreate) {
                    onCreate();
                }
            });
        });
    };

    self.edit = function() {
        self.dialog(function() {
            var url = "/api/news/category/update";
            self.save(url);
        });
    };

    self.save = function(url, onSuccess) {
        blockUI();
        postJSON(url, ko.toJS(self), function(result) {
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

    self.remove = function(item, parent) {
        if (self.categoryId()) {
            blockUI();
            var url = "/api/news/category/" + self.categoryId() + "/delete";
            postJSON(url, "", function(result) {
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

    self.pickImage = function() {
        showImagePickDialog(function(imageData) {
            self.imageId(imageData.ID);
            $('.remove-image').show();
        });
    };

    self.removeImage = function() {
        self.imageId("");
        $('.remove-image').hide();
    };


    self.moveUp = function(item, parent) {
        var index = parent.childrenCategories.indexOf(item);
        if (index <= 0) return false;

        parent.childrenCategories.remove(item);
        parent.childrenCategories.splice(index - 1, 0, item);
    };

    self.moveDown = function(item, parent) {
        var index = parent.childrenCategories.indexOf(item);
        if (index == parent.childrenCategories.length - 1) return false;

        parent.childrenCategories.remove(item);
        parent.childrenCategories.splice(index + 1, 0, item);
    };

    self.dialog = function(onSuccess) {
        var dm = ko.validatedObservable(self);
        var dialogContent = $("#categoryTemplate").tmpl();
        var options = {
            title: "Категория",
            width: 700,
            height: 450,
            resizable: false,
            modal: true,
            open: function() {
                var that = this;

                ko.applyBindings(dm, that);
            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function() {
                        var $form = $(this).find('form');

                        if (dm.isValid()) {
                            if (onSuccess) {
                                onSuccess();
                            }
                            $(this).dialog("close");
                        } else {
                            dm.errors.showAllMessages();
                        }
                    }
                },
                {
                    text: "Отмена",
                    click: function() {
                        $(this).dialog("close");
                    }
                }
            ],
            close: function() {
                $(this).dialog('destroy');
                dialogContent.remove();
            }
        };
        dialogContent.dialog(options);
        return dialogContent;


    };

    if (data)
        self.init(data);

    self.name.subscribe(function(val) {
        if (!self.relativeUrl() && val) {
            self.relativeUrl(textToUrl(val));
        }
    });
}

function showNewsCategoriesDialog(onSelected) {
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
            newsCategoriesEditorLoaded(function (item) {
                if (onSelected) {
                    onSelected({ id: item.categoryId(), name: item.name() });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/NewsEditor/Categories", options);
}
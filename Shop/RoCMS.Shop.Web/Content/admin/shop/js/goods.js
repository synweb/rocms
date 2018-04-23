/// <reference path="/Content/admin/ro/js/rocms.heart.js" />
/// <reference path="base.js" />

App.Admin.manufacturers = ko.observableArray();
App.Admin.usedManufacturers = ko.observableArray();
App.Admin.suppliers = ko.observableArray();
App.Admin.packs = ko.observableArray();
App.Admin.currencies = ko.observableArray();

App.Admin.Shop.GoodsFilter = {
    categoryIds: ko.observable(),
    manufacturerIds: ko.observable(),
    supplierIds: ko.observable(),
    searchPattern: ko.observable(),
    sortBy: ko.observable('CreationDateDesc'),

    categoryName: ko.observable('Выберите категорию'),
    manufacturerName: ko.observable(''),
    supplierName: ko.observable(''),

    clearCategory: function () {
        var self = this;
        self.categoryIds("");
        self.categoryName("Выберите категорию");
    },
    clearingAll: false,
    clearFilter: function () {
        var self = this;

        self.clearingAll = true;

        self.clearCategory();

        self.supplierIds("");
        self.supplierName("");

        self.manufacturerIds("");
        self.manufacturerName("");


        self.clearingAll = false;


        self.searchPattern("");
        self.sortBy("CreationDateDesc");


    }
};

function goodsEditorLoaded(onSelected, context) {
    var loadMoreGoods = function () {

        var pageSize = 10;

        var data = {
            categoryIds: vm.filters().categoryIds() ? [[vm.filters().categoryIds()]] : [],
            manufacturerIds: vm.filters().manufacturerIds() ? [vm.filters().manufacturerIds()] : [],
            supplierIds: vm.filters().supplierIds() ? [vm.filters().supplierIds()] : [],
            searchPattern: vm.filters().searchPattern() ? vm.filters().searchPattern() : "",
            sortBy: vm.filters().sortBy() ? vm.filters().sortBy() : "",
            startIndex: vm.goods().length + 1,
            count: pageSize
        };

        if (data.categoryIds.length === 0 &&
            data.manufacturerIds.length === 0 &&
            data.supplierIds.length === 0 &&
            !data.searchPattern)
            return false;

        blockUI();


        postJSON("/api/shop/goods/filter",
                data,
                function (result) {

                    if (result.length === 0) {
                        vm.hasMore(false);
                    } else {
                        vm.hasMore(true);
                    }

                    $(result).each(function () {
                        $(this.goodsSpecs).each(function () {
                            if (this.spec.valueType != 'Enum') {
                                this.inputValue = this.value;
                            } else {
                                this.inputValue = '';
                            }
                        });
                        var res = $.extend(ko.mapping.fromJS(this, App.Admin.Shop.GoodsItemValidationMapping),
                            App.Admin.Shop.GoodsItemFunctions);

                        vm.goods.push(res);
                    });
                })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
    };

    var reloadGoods = function () {

        vm.goods.removeAll();

        loadMoreGoods();


    };


    var vm = {
        selectedCategory: ko.observable(),
        goods: ko.observableArray(),
        suppliers: ko.observable(App.Admin.suppliers),

        manufacturers: ko.observable(App.Admin.usedManufacturers),
        filters: ko.observable(App.Admin.Shop.GoodsFilter),
        packs: ko.observableArray(),
        currencies: ko.observableArray(),


        copyGoodsItem: function (item) {

            var dataJs = ko.toJS(item); //создаем копию объекта, отвязывая все bindings
            var copy = $.extend(ko.mapping.fromJS(dataJs, App.Admin.Shop.GoodsItemValidationMapping),
                App.Admin.Shop.GoodsItemFunctions);

            copy.heartId = ko.observable(); //сбрасываем айдишник
            copy.relativeUrl(copy.relativeUrl() + "-copy");
            copy.name("Копия " + copy.name());
            copy.create(function () {
                vm.goods.push(copy);
            });
        },

        createGoodsItem: function () {

            var goodsItem = $.extend(new App.Admin.Shop.GoodsItem(), App.Admin.Shop.GoodsItemFunctions);
            if (vm.filters().categoryIds()) {
                goodsItem.categories.push(ko.mapping.fromJS({
                    id: vm.filters().categoryIds(),
                    name: vm.filters().categoryName()
                }));
                goodsItem.parentHeartId(vm.filters().categoryIds());
            }
            goodsItem.create(function () {
                vm.goods.push(goodsItem);
            });

        },

        selectCategory: function () {
            showCategoriesDialog(function (category) {
                vm.filters().categoryName(category.name);
                vm.filters().categoryIds(category.id);
            });
        },

        selectGoods: function (item) {
            if (onSelected) {
                onSelected(item);
            }
        },

        moreGoods: function () {
            loadMoreGoods();
        },

        hasMore: ko.observable(true)

    };

    var inited = 0;

    var checkAndReload = function () {
        if (inited >= 3) {
            reloadGoods();
        }
    }

    
    if (App.Admin.suppliers().length === 0) {
        getJSON("/api/shop/suppliers/get", "", function (result) {
            var man = new App.Admin.Shop.Manufacturer();
            man.name("Выберите...");

            App.Admin.suppliers.push(man);
            $(result).each(function () {
                App.Admin.suppliers.push($.extend(ko.mapping.fromJS(this, App.Admin.Shop.ManufacturerValidationMapping), App.Admin.Shop.ManufacturerFunctions));
            });


            if (lastSupplierId && lastSupplierName) {
                vm.filters().supplierName(lastSupplierName);
                vm.filters().supplierIds(lastSupplierId);

            }
            inited++;
            vm.filters().supplierIds.subscribe(function (newValue) {
                if (App.Admin.Shop.GoodsFilter.clearingAll == false) {
                    reloadGoods();
                }
            });
            checkAndReload();
        });
    } else {
        inited++;
    }

    if (App.Admin.usedManufacturers().length === 0) {
        getJSON("/api/shop/manufacturers/used/get",
            "",
            function (result) {
                var man = new App.Admin.Shop.Manufacturer();
                man.name("Выберите...");
                App.Admin.usedManufacturers.push(man);
                $(result).each(function () {
                    App.Admin.usedManufacturers.push($.extend(
                        ko.mapping.fromJS(this, App.Admin.Shop.ManufacturerValidationMapping),
                        App.Admin.Shop.ManufacturerFunctions));
                });

                if (lastManufacturerId && lastManufacturerName) {
                    vm.filters().manufacturerName(lastManufacturerName);
                    vm.filters().manufacturerIds(lastManufacturerId);
                }
                inited++;
                vm.filters().manufacturerIds.subscribe(function (newValue) {
                    if (App.Admin.Shop.GoodsFilter.clearingAll == false) {
                        reloadGoods();
                    }
                });

                checkAndReload();
            });
    } else {
        inited++;
    }

    if (App.Admin.manufacturers().length === 0) {

        getJSON("/api/shop/manufacturers/get",
            "",
            function (result) {
                var man = new App.Admin.Shop.Manufacturer();
                man.name("Выберите...");
                App.Admin.manufacturers.push(man);
                $(result).each(function () {
                    App.Admin.manufacturers.push($.extend(
                        ko.mapping.fromJS(this, App.Admin.Shop.ManufacturerValidationMapping),
                        App.Admin.Shop.ManufacturerFunctions));
                });
            });
    }

    if (App.Admin.packs().length === 0) {
        getJSON("/api/shop/packs/get",
            "",
            function (result) {
                App.Admin.packs.push(ko.mapping.fromJS({ name: "единицу товара", packId: null }));
                $(result).each(function () {
                    App.Admin.packs.push(ko.mapping.fromJS(this));
                });
            });
    }
    if (App.Admin.currencies().length === 0) {
        getJSON("/api/shop/currencies/get",
            "",
            function (result) {
                $(result.data).each(function () {
                    App.Admin.currencies.push(new App.Admin.Currency(this));
                });
            });
    }

    if (lastCategoryId && lastCategoryName) {
        vm.filters().categoryName(lastCategoryName);
        vm.filters().categoryIds(lastCategoryId);
    }


    if (lastSortBy) {
        vm.filters().sortBy(lastSortBy);
    }

    vm.filters().categoryIds.subscribe(function (newValue) {
        if (App.Admin.Shop.GoodsFilter.clearingAll == false) {
            reloadGoods();
        }
    });

    vm.filters().searchPattern.subscribe(function (newValue) {
        if (App.Admin.Shop.GoodsFilter.clearingAll == false) {
            reloadGoods();
        }
    });

    vm.filters().sortBy.subscribe(function (newValue) {
        if (App.Admin.Shop.GoodsFilter.clearingAll == false) {
            reloadGoods();
        }
    });
    inited++;
    checkAndReload();

    if (context) {
        ko.applyBindings(vm, context[0]);
    } else {
        ko.applyBindings(vm);
    }

};



App.Admin.Shop.GoodsItemValidationMapping = {
    name: {
        create: function (options) {
            var res = ko.observable(options.data).extend({ required: true });
            return res;
        }
    },
    relativeUrl: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    price: {
        create: function (options) {
            return ko.observable(options.data);
        }
    },
    description: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    htmlDescription: {
        create: function (options) {
            return ko.observable(options.data);
        }
    }

};

$.extend(App.Admin.Shop.GoodsItemValidationMapping, App.Admin.HeartValidationMapping);

App.Admin.Shop.GoodsItem = function () {
    var self = this;

    $.extend(self, new App.Admin.Heart());

    self.heartId = ko.observable();
    self.name = ko.observable().extend({ required: true });

    self.notAvailable = ko.observable();
    self.currency = ko.observable();

    self.manufacturerId = ko.observable();
    self.supplierId = ko.observable();
    self.price = ko.observable(0);
    self.dateOfAddition = ko.observable();

    self.description = ko.observable().extend({ required: true });
    self.htmlDescription = ko.observable(" ");
    self.article = ko.observable();

    self.basePackId = ko.observable();

    self.mainImageId = ko.observable();

    self.images = ko.observableArray();
    self.categories = ko.observableArray();
    self.actions = ko.observableArray();
    self.goodsSpecs = ko.observableArray();
    self.packs = ko.observableArray();
    self.compatibleGoods = ko.observableArray();

    self.filename = ko.observable();


}


App.Admin.Shop.GoodsItemFunctions = {
    initGoodsItem: function () {
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
    addSpec: function () {
        var self = this;
        showSpecDialog(function (item) {
            var specValue = {
                spec: item,
                heartId: ko.observable(self.heartId()),
                value: ko.observable(),
                isPrimary: ko.observable(),
                inputValue: ko.observable()
            };


            var result = $.grep(self.goodsSpecs(), function (e) {
                return e.spec.specId() == item.specId();
            });
            if (result.length == 0) {
                self.goodsSpecs.push(specValue);
            }
        });
    },
    removeSpec: function (spec, parent) {
        parent.goodsSpecs.remove(function (item) {
            return item.spec.specId() == spec.spec.specId();
        });
    },
    addCategory: function () {
        var self = this;
        showCategoriesDialog(function (category) {
            var result = $.grep(self.categories(), function (e) {
                return e.id() == category.id;
            });
            if (result.length === 0) {
                self.categories.push(ko.mapping.fromJS(category));
                setTimeout(function () {
                    if (self.categories().length === 1) {
                        self.parentHeartId(category.id);
                        $(".withsearch").selectpicker('refresh');
                    }
                }, 300);

            }
        });
    },
    removeCategory: function (category, parent) {
        parent.categories.remove(function (item) {
            return item.id() == category.id();
        });
        if (parent.categories().length === 0) {
            parent.parentHeartId("");
        }
    },
    addPack: function () {
        var self = this;
        showPacksDialog(function (pack) {
            var result = $.grep(self.packs(), function (e) {
                return e.packInfo.packId() == pack.packInfo.packId;
            });
            if (result.length == 0) {
                self.packs.push(ko.mapping.fromJS(pack));
            }
        });
    },
    removePack: function (pack, parent) {
        parent.packs.remove(function (item) {
            return item.packInfo.packId() == pack.packInfo.packId();
        });
    },
    addCompatibleGoods: function () {
        var self = this;
        showCompatiblesDialog(function (item) {
            var result = $.grep(self.compatibleGoods(), function (e) {
                return e.compatibleSetId() == item.compatibleSetId;
            });
            if (result.length == 0) {
                self.compatibleGoods.push(ko.mapping.fromJS(item));
            }
        });
    },
    removeCompatibleGoods: function (compatible, parent) {
        parent.compatibleGoods.remove(function (item) {
            return item.compatibleSetId() == compatible.compatibleSetId();
        });
    },
    addAction: function () {
        var self = this;
        showActionsDialog(function (action) {
            var result = $.grep(self.actions(), function (e) {
                return e.id() == action.id;
            });
            if (result.length == 0) {
                self.actions.push(ko.mapping.fromJS(action));
            }
        });
    },
    removeAction: function (action, parent) {
        parent.actions.remove(function (item) {
            return item.id() == action.id();
        });
    },
    save: function (url, onSuccess) {
        var self = this;
        self.prepareHeartForUpdate();
        blockUI();
        $(self.goodsSpecs()).each(function () {
            if (this.inputValue()) {
                this.value = this.inputValue();
            }
        });
        postJSON(url, ko.toJS(self), function (result) {
            if (result.succeed === true) {
                if (onSuccess) {
                    onSuccess(result.data);
                }
            } else {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            }
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
    },

    create: function (onSuccess) {
        var self = this;
        self.dialog("/api/shop/goods/create", function () {
            if (onSuccess) {
                onSuccess();
            }
        });
    },

    edit: function (onSuccess) {
        var self = this;
        self.dialog("/api/shop/goods/update", function () {
            if (onSuccess) {
                onSuccess();
            }
        });
    },

    remove: function (item, parent) {
        var self = this;
        if (self.heartId()) {
            blockUI();
            var url = "/api/shop/goods/" + self.heartId() + "/delete";
            postJSON(url, "", function (result) {
                if (result.succeed) {
                    parent.goods.remove(item);
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

    pickLogo: function () {
        var self = this;
        showImagePickDialog(function (imageData) {
            self.mainImageId(imageData.ID);
        });
    },


    pickFile: function () {
        var self = this;
        showFilePickDialog(function (fileData) {
            self.filename(fileData.name);
        });
    },

    addImage: function () {
        var self = this;
        showImagePickDialog(function (imageData) {
            self.images.push(imageData.ID);
        });
    },

    removeImage: function (image, parent) {
        parent.images.remove(function (item) {
            return item == image;
        });
    },

    dialog: function (url, onSave) {
        var self = this;
        var dm = ko.validatedObservable(this);
        var dialogContent = $("#goods-item-template").tmpl();
        var options = {
            title: "Товар",
            width: 960,
            height: 650,
            resizable: false,
            modal: true,
            open: function () {


                var dialog = this;



                var parents = ko.observableArray();
                parents.push({ title: "Нет", heartId: null, type: "Выберите..." });
                $(self.categories()).each(function () {
                    parents.push({ heartId: this.id, title: this.name, type: 'Категории' });
                });



                var model = {
                    vm: dm,
                    manufacturers: App.Admin.manufacturers,
                    suppliers: App.Admin.suppliers,
                    packs: App.Admin.packs,
                    currencies: App.Admin.currencies,
                    parents: parents
                };

                self.categories.subscribe(function () {
                    model.parents.removeAll();
                    model.parents.push({ title: "Нет", heartId: null, type: "Выберите..." });
                    $(self.categories()).each(function () {
                        model.parents.push({ heartId: this.id, title: this.name, type: 'Категории' });
                    });

                    self.parentHeartId.notifySubscribers();
                    $(".withsearch").selectpicker('refresh');
                });

                self.initGoodsItem();

                ko.applyBindings(model, dialog);
                $(".withsearch").selectpicker();

                if ($("#goodsHtmlDescription", $(dialog)).length) {
                    $("#goodsHtmlDescription", $(dialog)).val(dm().htmlDescription());
                    initContentEditor();
                }


            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {
                        var $dialog = $(this);

                        var $form = $(this).find('form');

                        //var e = CKEDITOR.instances['goodsHtmlDescription'];
                        //if (e) {
                        //    dm().htmlDescription(e.getData());
                        //}

                        var text = getTextFromEditor('goodsHtmlDescription');
                        if (text) {
                            dm().htmlDescription(text);
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


}

$.extend(App.Admin.Shop.GoodsItemFunctions, App.Admin.HeartFunctions);

//function goodsItemEditorLoaded(goodsId) {

//    var vm = {
//        goodsItem: ko.observable()
//    };

//    blockUI();
//    getJSON("/api/shop/goods/" + goodsId + "/get", null, function(result) {

//            $(result.goodsSpecs).each(function() {
//                if (this.spec.valueType != 'Enum') {
//                    this.inputValue = this.value;
//                } else {
//                    this.inputValue = '';
//                }
//            });

//            var res = $.extend(ko.mapping.fromJS(result, App.Admin.Shop.GoodsItemValidationMapping), App.Admin.Shop.GoodsItemFunctions);
//            res.name.subscribe(function(val) {
//                if (!res.relativeUrl() && val) {
//                    res.relativeUrl(textToUrl(val));
//                }
//            });
//            vm.goodsItem(res);

//        })
//        .fail(function() {
//            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
//        })
//        .always(function() {
//            unblockUI();
//        });


//}

function showGoodsDialog(onSelected) {
    var options = {
        title: "Товары",
        modal: true,
        draggable: false,
        resizable: false,
        width: 900,
        height: 650,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var that = this;

            getJSON("/api/shop/settings/lastused",
                "",
                function (data) {

                    window.lastCategoryId = data.lastCategory ? data.lastCategory.id : "";
                    window.lastCategoryName = data.lastCategory ? data.lastCategory.name : "";

                    window.lastManufacturerId = data.lastManufacturer ? data.lastManufacturer.id : "";
                    window.lastManufacturerName = data.lastManufacturer ? data.lastManufacturer.name : "";

                    window.lastSupplierId = data.lastSupplier ? data.lastSupplier.id : "";
                    window.lastSupplierName = data.lastSupplier ? data.lastSupplier.name : "";

                    window.lastSortBy = data.lastSortBy;



                    goodsEditorLoaded(function (item) {
                        if (onSelected) {
                            onSelected({ id: item.heartId(), name: item.name() });
                        }
                        $(that).dialog("close");
                    }, $dialog);
                });


        }
    };
    showDialogFromUrl("/ShopEditor/GoodsEditor", options);
}
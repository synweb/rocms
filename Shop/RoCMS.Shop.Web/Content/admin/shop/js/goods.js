﻿/// <reference path="/Content/admin/ro/js/rocms.heart.js" />

App.Admin.manufacturers = ko.observableArray();
App.Admin.usedManufacturers = ko.observableArray();
App.Admin.suppliers = ko.observableArray();
App.Admin.packs = ko.observableArray();
App.Admin.currencies = ko.observableArray();

App.Admin.GoodsFilter = {
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
    var reloadGoods = function () {


        var data = {
            categoryIds: vm.filters().categoryIds() ? [vm.filters().categoryIds()] : [],
            manufacturerIds: vm.filters().manufacturerIds() ? [vm.filters().manufacturerIds()] : [],
            supplierIds: vm.filters().supplierIds() ? [vm.filters().supplierIds()] : [],
            searchPattern: vm.filters().searchPattern() ? vm.filters().searchPattern() : "",
            sortBy: vm.filters().sortBy() ? vm.filters().sortBy() : ""
        };

        vm.goods.removeAll();



        if (!data.categoryIds && !data.manufacturerIds && !data.supplierIds && !data.searchPattern)
            return false;

        blockUI();


        postJSON("/api/shop/goods/filter", data, function(result) {
                $(result).each(function() {
                    $(this.goodsSpecs).each(function() {
                        if (this.spec.valueType != 'Enum') {
                            this.inputValue = this.value;
                        } else {
                            this.inputValue = '';
                        }
                    });
                    var res = $.extend(ko.mapping.fromJS(this, App.Admin.GoodsItemValidationMapping), App.Admin.GoodsItemFunctions);
                    res.name.subscribe(function(val) {
                        if (!res.relativeUrl() && val) {
                            res.relativeUrl(textToUrl(val));
                        }
                    });
                    vm.goods.push(res);
                });
            })
            .fail(function() {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function() {
                unblockUI();
            });
    }


    var vm = {
        selectedCategory: ko.observable(),
        goods: ko.observableArray(),
        suppliers: ko.observable(App.Admin.suppliers),

        manufacturers: ko.observable(App.Admin.usedManufacturers),
        filters: ko.observable(App.Admin.GoodsFilter),
        packs: ko.observableArray(),
        currencies: ko.observableArray(),

        createGoodsItem: function () {

            var goodsItem = $.extend(new App.Admin.GoodsItem(), App.Admin.GoodsItemFunctions);
            if (vm.filters().categoryIds()) {
                goodsItem.categories.push(ko.mapping.fromJS({ id: vm.filters().categoryIds(), name: vm.filters().categoryName() }));
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
        }


    };

    var checkAndReload = function () {
        if (inited == 3) {
            if (lastManufacturerId || lastSupplierId || lastCategoryId) {
                reloadGoods();
            }
        }
    }

    var inited = 0;
    getJSON("/api/shop/suppliers/get", "", function (result) {
        App.Admin.suppliers.push(new App.Admin.Manufacturer({ name: "Все поставщики" }));
        $(result).each(function () {
            App.Admin.suppliers.push(new App.Admin.Manufacturer(this));
        });
        inited++;

        if (lastSupplierId && lastSupplierName) {
            vm.filters().supplierName(lastSupplierName);
            vm.filters().supplierIds(lastSupplierId);

        }

        vm.filters().supplierIds.subscribe(function (newValue) {
            if (App.Admin.GoodsFilter.clearingAll == false) {
                reloadGoods();
            }
        });
        checkAndReload();
    });

    getJSON("/api/shop/manufacturers/used/get", "", function (result) {
        App.Admin.usedManufacturers.push(new App.Admin.Manufacturer({ name: "Все производители" }));
        $(result).each(function () {
            App.Admin.usedManufacturers.push(new App.Admin.Manufacturer(this));
        });
        inited++;
        if (lastManufacturerId && lastManufacturerName) {
            vm.filters().manufacturerName(lastManufacturerName);
            vm.filters().manufacturerIds(lastManufacturerId);
        }

        vm.filters().manufacturerIds.subscribe(function (newValue) {
            if (App.Admin.GoodsFilter.clearingAll == false) {
                reloadGoods();
            }
        });

        checkAndReload();
    });

    getJSON("/api/shop/manufacturers/get", "", function (result) {
        App.Admin.manufacturers.push(new App.Admin.Manufacturer({ name: "Неизвестен" }));
        $(result).each(function () {
            App.Admin.manufacturers.push(new App.Admin.Manufacturer(this));
        });
    });

    getJSON("/api/shop/packs/get", "", function (result) {
        App.Admin.packs.push(ko.mapping.fromJS({ name: "единицу товара", packId: null }));
        $(result).each(function () {
            App.Admin.packs.push(ko.mapping.fromJS(this));
        });
    });

    getJSON("/api/shop/currencies/get", "", function (result) {
        $(result.data).each(function () {
            App.Admin.currencies.push(new App.Admin.Currency(this));
        });
    });

    if (lastCategoryId && lastCategoryName) {
        vm.filters().categoryName(lastCategoryName);
        vm.filters().categoryIds(lastCategoryId);
    }


    if (lastSortBy) {
        vm.filters().sortBy(lastSortBy);
    }



    vm.filters().categoryIds.subscribe(function (newValue) {
        if (App.Admin.GoodsFilter.clearingAll == false) {
            reloadGoods();
        }
    });

    vm.filters().searchPattern.subscribe(function (newValue) {
        if (App.Admin.GoodsFilter.clearingAll == false) {
            reloadGoods();
        }
    });

    vm.filters().sortBy.subscribe(function (newValue) {
        if (App.Admin.GoodsFilter.clearingAll == false) {
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



App.Admin.GoodsItemValidationMapping = {
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

$.extend(App.Admin.GoodsItemValidationMapping, App.Admin.HeartValidationMapping);

App.Admin.GoodsItem = function () {
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
    self.keywords = ko.observable();
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

    self.relativeUrl = ko.observable();
    self.filename = ko.observable();


    self.name.subscribe(function (val) {
        if (!self.relativeUrl() && val) {
            self.relativeUrl(textToUrl(val));
        }
    });
}


App.Admin.GoodsItemFunctions = {
    initGoodsItem: function() {
        var self = this;
        self.initHeart();
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
        self.dialog(function () {
            self.save("/api/shop/goods/create", function (result) {
                self.heartId(result.id);
                if (onSuccess) {
                    onSuccess();
                }
            });
        });
    },

    edit: function () {
        var self = this;
        self.dialog(function () {
            self.save("/api/shop/goods/update");
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

    dialog: function (onSuccess) {
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

                self.initGoodsItem();

                ko.applyBindings({ vm: dm, manufacturers: App.Admin.manufacturers, packs: App.Admin.packs, currencies: App.Admin.currencies }, dialog);

                if ($("#goodsHtmlDescription", $(dialog)).length) {
                    $("#goodsHtmlDescription", $(dialog)).val(dm().htmlDescription());
                    initContentEditor();
                }


            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {
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
    }

    
}

$.extend(App.Admin.GoodsItemFunctions, App.Admin.HeartFunctions);

function showGoodsDialog(onSelected) {
    var options = {
        title: "Товары",
        modal: true,
        draggable: false,
        resizable: false,
        width: 960,
        height: 550,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var that = this;
            goodsEditorLoaded(function (item) {
                if (onSelected) {
                    onSelected({ id: item.heartId(), name: item.name() });
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/GoodsEditor", options);
}
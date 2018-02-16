function specsEditorLoaded(onSelected, context) {
    blockUI();



    var vm = {
        specs: ko.observableArray(),
        createSpec: function () {
            var self = this;
            var spec = new App.Admin.Spec();
            spec.new(function () {
                self.specs.push(spec);
            });
        },
        selectSpec: function (item) {
            if (onSelected) {
                onSelected(item);
            }
        },
        
        saveOrder: function () {
            blockUI();
            var ids = $(ko.toJS(this.specs)).map(function () { return this.specId }).get();
            postJSON("/api/shop/spec/updateorder", ids,
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
    getJSON("/api/shop/specs/get", "", function (result) {
        $(result).each(function () {
            vm.specs.push(new App.Admin.Spec(this));
        });

        // sortable
        function changeElementIndex(observableCollection, oldIndex, newIndex) {
            if (oldIndex < 0 || newIndex < 0)
                return;
            var item = observableCollection()[oldIndex];
            observableCollection.remove(item);
            observableCollection.splice(newIndex, 0, item);
        }
        function reinitSortable() {
            $('.o-sortable').sortable({
                placeholder: 'sortable-placeholder',
                handle: '.sortable-pin',
                start: function (e, ui) {
                    // creates a temporary attribute on the element with the old index
                    $(this).attr('data-previndex', ui.item.index());
                },
                update: function (e, ui) {
                    // gets the new and old index then removes the temporary attribute
                    var newIndex = ui.item.index();
                    var oldIndex = $(this).attr('data-previndex');
                    $(this).removeAttr('data-previndex');
                    changeElementIndex(vm.specs, oldIndex, newIndex);
                    ui.item[0].remove();
                    vm.saveOrder();
                    reinitSortable();
                }
            });

        }
        reinitSortable();
        unblockUI();
    });

    if (context) {
        ko.applyBindings(vm, context[0]);
    } else {
        ko.applyBindings(vm);
    }
    VM = vm;

}

App.Admin.Spec = function (data) {
    var self = this;

    self.specId = ko.observable();
    self.name = ko.observable().extend({ required: true });
    self.valueType = ko.observable().extend({ required: true });
    self.acceptableValues = ko.observable();
    self.prefix = ko.observable();
    self.postfix = ko.observable();
    self.sortOrder = ko.observable();

    self.init = function (item) {
        self.specId(item.specId);
        self.name(item.name);
        self.valueType(item.valueType);
        
        self.prefix(item.prefix);
        self.postfix(item.postfix);
        self.sortOrder(item.sortOrder);


        self.acceptableValues(item.acceptableValues);

    };

    if (data) {
        self.init(data);
    }

    self.fetch = function () {
        if (self.specId()) {
            blockUI();
            postJSON("/api/shop/spec/" + self.specId() + "/get", "", function (result) {
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
        postJSON(url, ko.mapping.toJS(self, { ignore: ["acceptableArray"] }), function (result) {
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
            self.save("/api/shop/spec/update");
        });
    };

    self.remove = function (item, parent) {
        if (self.specId()) {
            blockUI();
            var url = "/api/shop/spec/" + self.specId() + "/delete";
            postJSON(url, "", function (result) {
                if (result.succeed) {
                    parent.specs.remove(item);
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
    
    self.new = function (onSuccess) {
        self.dialog(function () {
            self.save("/api/shop/spec/create", function (result) {
                self.specId(result.id);
                if (onSuccess) {
                    onSuccess();
                }
            });
        });
    };

    self.dialog = function (onSuccess) {
        var dm = ko.validatedObservable(self);
        var dialogContent = $("#specTemplate").tmpl();
        var options = {
            title: "Характеристика",
            width: 650,
            height: 550,
            resizable: false,
            modal: true,
            open: function () {
                ko.applyBindings(dm, this);

                $(this).find("#specVals").tagsinput();

            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {
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
    };
}


function showSpecDialog(onSelected) {
    blockUI();
    var options = {
        title: "Характеристики",
        modal: true,
        draggable: false,
        resizable: false,
        width: 760,
        height: 550,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var that = this;
            unblockUI();
            specsEditorLoaded(function (item) {
                if (onSelected) {
                    onSelected(item);
                }
                $(that).dialog("close");
            }, $dialog);
        }
    };
    showDialogFromUrl("/ShopEditor/SpecsEditor", options);
}
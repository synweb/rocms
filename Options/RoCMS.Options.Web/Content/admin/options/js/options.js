function optionsEditorLoaded(onSelected, context) {
    blockUI();
    var vm = {
        options: ko.observableArray(),
        createOption: function () {
            var self = this;
            var option = $.extend(new App.Admin.Option(), App.Admin.OptionFunctions);
            option.create(function () {
                self.options.push(option);
            });
        },
        selectOption: function (item) {
            if (onSelected) {
                onSelected(item);
            }
        },
    };
    getJSON("/api/options/get", "", function (result) {
        $(result).each(function () {
            var option = $.extend(ko.mapping.fromJS(this, App.Admin.OptionValidationMapping), App.Admin.OptionFunctions);
            vm.options.push(option);
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

App.Admin.OptionValidationMapping = {
    name: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    optionValues: {
        create: function(options) {
            var val = ko.mapping.fromJS(options.data, App.Admin.OptionValueValidationMapping);
            return val;
        }
    }
};

App.Admin.Option = function () {
    var self = this;

    self.id = ko.observable();
    self.name = ko.observable().extend({ required: true });
    self.moderated = ko.observable(true);
    self.optionValues = ko.observableArray();
    self.creationDate = ko.observable();
}

App.Admin.OptionValue = function() {
    var self = this;

    self.id = ko.observable();
    self.value = ko.observable().extend({ required: true });
    self.moderated = ko.observable(true);
    self.optionKeyId = ko.observable();
    self.creationDate = ko.observable();
}

App.Admin.OptionValueValidationMapping = {
    value: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    }
};

App.Admin.OptionFunctions = {
    addValue: function() {
        var val = new App.Admin.OptionValue();
        val.optionKeyId(this.id());
        this.optionValues.push(val);
    },
    removeValue: function(value, parent) {
        parent.optionValues.remove(function (item) {
            return item.id() === value.id();
        });
    },

    fetch: function() {
        var self = this;
        getJSON("/api/options/" + self.id() + "/get", null, function(res) {
            self.id(res.id);
            self.name(res.name);
            self.moderated(res.moderated);
            self.creationDate(res.creationDate);
            self.optionValues.removeAll();
            $(res.optionValues).each(function () {
                var optionValue = ko.mapping.fromJS(this, App.Admin.OptionValueValidationMapping);
                self.optionValues.push(optionValue);
            });
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
                self.fetch();
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
            self.save("/api/options/update");
        });
    },

    remove: function (item, parent) {
        var self = this;
        if (self.id()) {
            blockUI();
            var url = "/api/options/" + self.id() + "/delete";
            postJSON(url, "", function (result) {
                if (result.succeed) {
                    parent.options.remove(item);
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
            self.save("/api/options/create", function (result) {
                self.id(result);
                if (onSuccess) {
                    onSuccess();
                }
            });
        });
    },

    dialog: function (onSuccess) {
        var self = this;
        var dm = ko.validatedObservable(self);
        var dialogContent = $("#optionTemplate").tmpl();
        var options = {
            title: "Характеристика",
            width: 650,
            height: 550,
            resizable: false,
            modal: true,
            open: function () {
                ko.applyBindings(dm, this);
            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {

                        if (dm.isValid()) {
                            //TODO: валидация элементов массива

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
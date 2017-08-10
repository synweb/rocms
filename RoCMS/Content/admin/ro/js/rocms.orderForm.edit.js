/// <reference path="../../../base/vendor/jquery/core/jquery-2.0.2.js" />
/// <reference path="admin-ajax.js" />


function onOrderFormListLoad() {
    $('#adminContent').on("click", ".block-summary .button-delete", function () {
        if (!confirmRemoval()) {
            return false;
        }
        var container = $(this).closest(".block-summary");
        var id = container.data("orderFormId");
        var url = "/api/orderform/" + id + "/delete";
        blockUI();
        postJSON(url, "", function (res) {
            if (res.succeed) {
                container.hide(1000, function () { container.remove(); });
            } else {
                if (res.message) {
                    alert(res.message);
                } else {
                    alert("Произошла ошибка. Попробуйте еще раз или свяжитесь с администрацией");
                }
            }
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
        return false;
    });
}

function onOrderFormEditorLoad(orderFormId) {
    var vm = {
        orderForm: ko.validatedObservable($.extend(new App.Admin.OrderForm(), App.Admin.OrderFormFunctions)),
        save: function () {
            if (vm.errors()().length) {
                vm.errors().showAllMessages();
                return false;
            }

            vm.orderForm().orderFormId() === -1 ? vm.orderForm().create(function (id) {
                window.location.href = "/Admin/EditOrderForm/" + id;
            }) : vm.orderForm().edit();
            return false;
        }
    };

    vm.errors = ko.computed(function () {
        return ko.validation.group(vm.orderForm(), { deep: true });
    });

    if (orderFormId) {
        getJSON("/api/orderform/" + orderFormId + "/get", "", function (res) {
            if (res.succeed) {
                var form = $.extend(ko.mapping.fromJS(res.data, App.Admin.OrderFormValidationMapping), App.Admin.OrderFormFunctions);
                form.fields = ko.observableArray().extend({ minArrayLength: { params: { minLength: 1 }, message: 'Добавьте хотя бы одно поле' } });;
                $(res.data.fields).each(function () {
                    form.fields.push(ko.mapping.fromJS(this, App.Admin.OrderFormFieldValidationMapping));
                });
                vm.orderForm(form);
            }
        });
    }

    ko.applyBindings(vm);
}



App.Admin.OrderForm = function () {
    var self = this;

    self.orderFormId = ko.observable(-1);
    self.emailSubject = ko.observable();
    self.email = ko.observable();
    self.bccEmail = ko.observable();
    self.htmlTemplate = ko.observable();
    self.redirectUrl = ko.observable();
    self.successMessage = ko.observable();
    self.metricsCode = ko.observable();
    self.dateInEmailSubject = ko.observable();
    self.fileAttachmentEnabled = ko.observable();
    self.maxFileAttachmentsCount = ko.observable();
    self.fields = ko.observableArray();
    self.title = ko.observable().extend({ required: true });
    self.emailTemplate = ko.observable();
}

App.Admin.OrderFormField = function () {
    var self = this;

    self.orderFormFieldId = ko.observable();
    self.labelText = ko.observable().extend({ required: true });
    self.valueType = ko.observable().extend({ required: true });
    self.required = ko.observable();
    self.orderFormId = ko.observable();
    self.sortOrder = ko.observable(1);

}

App.Admin.OrderFormValidationMapping = {
    title: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    }
};

App.Admin.OrderFormFieldValidationMapping = {
    labelText: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    valueType: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    }
};

App.Admin.OrderFormFunctions = {
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
        self.save("/api/orderform/update", function () {
            if (onSuccess) {
                onSuccess();
            }
        });
    },


    create: function (onSuccess) {
        var self = this;
        self.save("/api/orderform/create", function (result) {
            if (onSuccess) {
                onSuccess(result);
            }
        });
    },

    addField: function () {
        var self = this;
        var field = new App.Admin.OrderFormField();
        field.sortOrder(self.fields().length);
        self.fields.push(field);
    },

    removeField: function (item, parent) {
        parent.fields.remove(item);
    },

    generateTemplate: function () {
        var self = this;


        self.htmlTemplate("");
        for (var i = 0; i < self.fields().length; i++) {
            self.htmlTemplate(self.htmlTemplate() + "{" + i + "}");
        }
    },

    generateEmailTemplate: function() {
        var self = this;
        self.emailTemplate("{0}");
    }


}
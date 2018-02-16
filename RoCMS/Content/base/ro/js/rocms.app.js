/// <reference path="rocms.helpers.js" />

var IMAGE_THUMBNAIL_URL = "/Gallery/Thumbnail/";

var App = {};
$(function() {

    $(document.body).on("focus", "input[type='text'], textarea", function () {
        if (!$(this).attr("placeholder")) return;
        $(this).select();
        $(this).data("placeholder", $(this).attr("placeholder"));
        $(this).attr("placeholder", "");
    });
    $(document.body).on("blur", "input[type='text'], textarea", function () {
        $(this).attr("placeholder", $(this).data("placeholder"));
    });
});
if (typeof(ko) != "undefined") {
    ko.validation.init({
        decorateElement: true,
        errorElementClass: 'input-validation-error'
    }, true);

//Если не указать - будет использоваться jquery.tmpl, а с ним возникают ошибки
    ko.setTemplateEngine(new ko.nativeTemplateEngine());

    ko.extenders.numeric = function(target, precision) {
        var result = ko.computed({
            read: function() {

                var d = '.';
                var t = " ";
                var c = precision;

                var n = target(),
                    s = n < 0 ? "-" : "",
                    i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
                    j = (j = i.length) > 3 ? j % 3 : 0;
                var str = s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");

                return str;
            },
            write: target
        });

        result.raw = target;
        return result;
    };

    ko.bindingHandlers.thumbnailSrc = {
        init: function(element, valueAccessor) {
            var value = ko.unwrap(valueAccessor());
            var src;
            if (value) {
                src = IMAGE_THUMBNAIL_URL + value;
            } else {
                src = "/Content/admin/ro/img/no-image.png";
            }

            $(element).attr("src", src);
        },
        update: function(element, valueAccessor) {
            var value = valueAccessor();
            var valueUnwrapped = ko.unwrap(value);

            var src;
            if (valueUnwrapped) {
                src = IMAGE_THUMBNAIL_URL + valueUnwrapped;
            } else {
                src = "/Content/admin/ro/img/no-image.png";
            }

            $(element).attr("src", src);
        }
    };

    ko.bindingHandlers.fileupload = {
        update: function(element, valueAccessor) {
            var options = valueAccessor() || {};

            //initialize
            $(element).fileupload(options);
        }
    };

    ko.bindingHandlers.tagsinput = {
        init: function (element, valueAccessor, allBindings) {
            var options = allBindings().tagsinputOptions || {};
            var value = valueAccessor();
            var valueUnwrapped = ko.unwrap(value);

            var el = $(element);

            el.tagsinput(options);

            for (var i = 0; i < valueUnwrapped.length; i++) {
                el.tagsinput('add', valueUnwrapped[i]);
            }

            el.on('itemAdded', function (event) {
                if (valueUnwrapped.indexOf(event.item) === -1) {
                    value.push(event.item);
                }
            })

            el.on('itemRemoved', function (event) {
                value.remove(event.item);
            });
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var value = valueAccessor();
            var valueUnwrapped = ko.unwrap(value);

            var el = $(element);
            var prev = el.tagsinput('items');

            var added = valueUnwrapped.filter(function (i) { return prev.indexOf(i) === -1; });
            var removed = prev.filter(function (i) { return valueUnwrapped.indexOf(i) === -1; });

            // Remove tags no longer in bound model
            for (var i = 0; i < removed.length; i++) {
                el.tagsinput('remove', removed[i]);
            }

            // Refresh remaining tags
            el.tagsinput('refresh');

            // Add new items in model as tags
            for (i = 0; i < added.length; i++) {
                el.tagsinput('add', added[i]);
            }
        }
    };

    ko.validation.rules['isPhone'] = {
        validator: function (value) {
            var phone = value;
            var isMatch = phone.match(/^\+?(\d[\d- ]+)(\([\d- ]+\))?[\d- ]+\d$/gi);
            var formatted = formatPhone(phone);
            return isMatch && (formatted.length > 8);
        },
        message: '*'
    };

    ko.validation.rules['minArrayLength'] = {
        validator: function (obj, params) {
            return obj.length >= params.minLength;
        },
        message: "Array does not meet minimum length requirements"
    };

    

    ko.validation.registerExtenders();

}

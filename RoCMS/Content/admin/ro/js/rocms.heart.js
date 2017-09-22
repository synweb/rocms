/// <reference path="admin-ajax.js" />

App.Admin.Heart = function () {
    var self = this;

    self.title = ko.observable().extend({ required: true });
    self.breadcrumbsTitle = ko.observable();
    self.relativeUrl = ko.observable().extend({ required: true });
    self.metaDescription = ko.observable();
    self.metaKeywords = ko.observable();
    self.parentHeartId = ko.observable(null);
    self.layout = ko.observable("clientLayout");
    self.heartId = ko.observable();
    self.noIndex = ko.observable(false);
    self.styles = ko.observable();
    self.scripts = ko.observable();
    self.additionalHeaders = ko.observable();

    $.extend(self, App.Admin.HeartFunctions);
}

App.Admin.HeartValidationMapping = {
    title: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    relativeUrl: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    }
};

App.Admin.HeartFunctions = {
    initHeart: function () {
        var self = this;
        if (self.scripts()) {
            setTextToEditor("page_scripts", self.scripts());
        }
        if (self.styles()) {
            setTextToEditor("page_styles", self.styles());
        }
        if (self.additionalHeaders()) {
            setTextToEditor("page_headers", self.additionalHeaders());
        }
    },

    prepareHeartForUpdate: function () {
        var self = this;
        var scripts = getTextFromEditor('page_scripts');
        self.scripts(scripts);
        var styles = getTextFromEditor('page_styles');
        self.styles(styles);
        var additionalHeaders = getTextFromEditor('page_headers');
        self.additionalHeaders(additionalHeaders);
    }
}
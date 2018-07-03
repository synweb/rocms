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
    self.noindex = ko.observable(false);
    self.styles = ko.observable();
    self.scripts = ko.observable();
    self.additionalHeaders = ko.observable();
    self.canonicalUrl = ko.observable();

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

        self.metaDescriptionLength = ko.computed(function () {
            return self.metaDescription() ? self.metaDescription().length : 0;
        });

        self.breadcrumbsTitleLength = ko.computed(function () {
            return self.breadcrumbsTitle() ? self.breadcrumbsTitle().length : 0;
        });

        self.titleLength = ko.computed(function () {
            return self.title() ? self.title().length : 0;
        });

        self.editMode = ko.computed(function() {
            return self.canonicalUrl && typeof(self.canonicalUrl()) != "undefined";
        });

        self.title.subscribe(function (val) {
            if (!self.relativeUrl()) {
                self.relativeUrl(textToUrl(val));
            }
            if (!self.breadcrumbsTitle()) {
                self.breadcrumbsTitle(val);
            }
            if (!self.metaDescription()) {
                self.metaDescription(val);
            }
        });

        createACEEditor("page_scripts", $("#page_scripts").data("aceMode"));
        createACEEditor("page_styles", $("#page_styles").data("aceMode"));
        createACEEditor("page_headers", $("#page_headers").data("aceMode"));

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
    },

    generateUrl : function() {
        var self = this;
        if (self.title()) {
            self.relativeUrl(textToUrl(self.title()));
        }
        return false;
    }
}
function newsSettingsLoaded() {
    var vm = {
        settings: ko.validatedObservable(),
        crawlers: ko.observableArray(),
        addFeed: function () {
            var item = new App.Admin.RssCrawler();
            vm.crawlers.push(item);
        },
        removeFeed: function (data) {
            vm.crawlers.remove(data);
        },
        save: function () {
            var dm = ko.validatedObservable(self.settings);
            if (dm.isValid()) {
                blockUI();
                $.when(
                    vm.settings().save(),
                    postJSON("/api/news/settings/crawlers/update",
                        { crawlers: ko.toJS(vm.crawlers) },
                        function(result) {
                            if (result.succeed === true) {
                                smartAlert("Настройки сохранены.");
                            }
                        })
                ).then(
                    function() {
                    },
                    function() {
                        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                    }
                ).always(function() {
                    unblockUI();
                });
            } else {
                dm.errors.showAllMessages();
            }
        },
    };
    //TODO
    currentVm = vm;
    blockUI();
    $.when(
        getJSON("/api/news/settings/get", "", function (result) {
            vm.settings(new App.Admin.NewsSettings(result));
        }),
        getJSON("/api/news/settings/crawlers/get", "", function (result2) {
            result2.data.forEach(function(item) {
                vm.crawlers.push(new App.Admin.RssCrawler(item));
            });
        })
    ).then(
        function () {
            ko.applyBindings(vm);
        },
        function () {
            smartAlert("Произошла ошибка");
        }
    ).always(function () {
        unblockUI();
    });
}

App.Admin.RssCrawler = function(data) {
    var self = this;
    self.rssCrawlerId = ko.observable();
    self.rssFeedUrl = ko.observable();
    self.isEnabled = ko.observable();
    self.checkInterval = ko.observable();
    self.targetCategory = ko.observable();
    self.targetCategoryId = ko.observable();
    self.filters = ko.observableArray();
    self.canUnsetCategory = ko.computed(function () {
        return (self.targetCategory != undefined && self.targetCategory().length > 0);
    });
    self.canSetCategory = ko.computed(function () {
        return !self.canUnsetCategory();
    });
    self.targetCategoryName = ko.computed(function () {
        if (self.targetCategory() && self.targetCategory().name && typeof self.targetCategory().name == 'function') {
            return self.targetCategory().name();
        } else {
            return "";
        }
    });
    self.setCategory = function() {
        var self = this;
        showNewsCategoriesDialog(function (category) {
            self.targetCategory(ko.mapping.fromJS(category));
            self.targetCategoryId(category.id);
        });
    };
    self.unsetCategory = function () {
        self.targetCategory(null);
        self.targetCategoryId(null);
    };
    if (data) {
        self.rssCrawlerId(data.rssCrawlerId);
        self.checkInterval(data.checkInterval);
        self.rssFeedUrl(data.rssFeedUrl);
        self.isEnabled(data.isEnabled);
        self.targetCategory(ko.mapping.fromJS(data.targetCategory));
        self.filters(data.filters);
    } else {
        self.checkInterval(30);
        self.isEnabled(true);
    }
}

App.Admin.NewsSettings = function (data) {
    var self = this;
    self.blogUrl = ko.observable().extend({ required: true });
    self.save = function () {
        blockUI();
        return postJSON("/api/news/settings/update",
            ko.toJS(self),
            function (result) {
                if (result.succeed === true) {
                }
            });
    };
    
    if (data) {
        if (data.blogUrl) {
            self.blogUrl(data.blogUrl);
        }
    }
}
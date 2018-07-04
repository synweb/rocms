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
                console.log(ko.toJS(vm.crawlers));
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
            $(".news-tags").tagsinput({
                //autocomplete_url: '/api/news/tag/pattern/get'
            });
        },
        function () {
            smartAlert("Произошла ошибка");
        }
    ).always(function () {
        unblockUI();
    });
}

App.Admin.RssCrawlerFilter = function (data) {
    var self = this;
    self.rssCrawlerFilterId = ko.observable();
    self.rssCrawlerId = ko.observable();
    self.filter = ko.observable();
    if (data) {
        self.rssCrawlerFilterId(data.rssCrawlerFilterId);
        self.rssCrawlerId(data.rssCrawlerId);
        self.filter(data.filter);
    }
}


App.Admin.ExcludeItem = function (data) {
    var self = this;
    self.excludeItemIndex = ko.observable();
    self.rssCrawlerId = ko.observable();
    self.selector = ko.observable();
    if (data) {
        self.excludeItemIndex(data.excludeItemIndex);
        self.rssCrawlerId(data.rssCrawlerId);
        self.selector(data.selector);
    }
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
    self.excludeItems = ko.observableArray();
    self.imageSelector = ko.observable();
    self.contentContainerSelector = ko.observable();
    self.linkText = ko.observable();
    self.tags = ko.observable();
    self.canUnsetCategory = ko.computed(function () {
        return (self.targetCategory());
    });
    self.canSetCategory = ko.computed(function () {
        return !self.canUnsetCategory();
    });
    self.targetCategoryName = ko.computed(function () {
        if (self.targetCategory() && self.targetCategory().name) {
            return self.targetCategory().name;
        } else {
            return "";
        }
    });
    self.setCategory = function() {
        var self = this;
        showNewsCategoriesDialog(function (category) {
            self.targetCategory(category);
            self.targetCategoryId(category.id);
        });
    };
    self.unsetCategory = function () {
        self.targetCategory(null);
        self.targetCategoryId(null);
    };
    self.addFilter = function () {
        self.filters.push(new App.Admin.RssCrawlerFilter());
    };
    self.removeFilter = function (item) {
        self.filters.remove(item);
    };
    self.addExcludeItem = function () {
        self.excludeItems.push(new App.Admin.ExcludeItem());
    };
    self.removeExcludeItem = function (item) {
        self.excludeItems.remove(item);
    };
    if (data) {
        self.rssCrawlerId(data.rssCrawlerId);
        self.checkInterval(data.checkInterval);
        self.rssFeedUrl(data.rssFeedUrl);
        self.isEnabled(data.isEnabled);
        self.targetCategory(data.targetCategory);
        self.targetCategoryId(data.targetCategoryId);
        self.filters(data.filters);
        self.excludeItems(data.excludeItems);
        self.imageSelector(data.imageSelector);
        self.contentContainerSelector(data.contentContainerSelector);
        self.linkText(data.linkText);
        self.tags(data.tags);
    } else {
        self.checkInterval(30);
        self.isEnabled(true);
        self.linkText("Читать в источнике");
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
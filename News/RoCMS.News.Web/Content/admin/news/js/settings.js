function newsSettingsLoaded() {
    var vm = {
        settings: ko.validatedObservable(),
        crawlers: ko.observableArray(),
        addFeed: function () {
            var item = new App.Admin.RssCrawler();
            vm.crawlers.push(item);
        },
        removeFeed: function (data) {
            vm. crawlers.remove(data);
        },
        save: function () {
            blockUI();
            $.when(
                vm.settings().save(),
                postJSON("/api/news/settings/crawlers/update",
                    { crawlers: ko.toJS(self.crawlers) },
                    function (result) {
                        if (result.succeed === true) {
                            smartAlert("Настройки сохранены.");
                        }
                    })
            ).then(
                function () {
                },
                function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                }
            ).always(function () {
                unblockUI();
            });
        }
    };
    
    blockUI();
    $.when(
        getJSON("/api/news/settings/get", "", function (result) {
            vm.settings(new App.Admin.NewsSettings(result));
        }),
        getJSON("/api/news/settings/crawlers/get", "", function (result) {
            result.data.forEach(function(item) {
                vm.crawlers.push(item);
            });
            vm.settings(new App.Admin.NewsSettings(result));
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

App.Admin.RssCrawler = function (data) {
    self.rssCrawlerId = ko.observable();
    self.rssFeedUrl = ko.observable();
    self.isEnabled = ko.observable();
    self.checkInterval = ko.observable();
    self.targetCategory = ko.observable();
    self.filters = ko.observableArray();
    
}

App.Admin.NewsSettings = function (data) {
    var self = this;

    self.blogUrl = ko.observable().extend({required: true});

    if (data) {
        if (data.blogUrl) {
            self.blogUrl(data.blogUrl);
        }
    }

    self.save = function () {
        var dm = ko.validatedObservable(self);
        if (dm.isValid()) {
            blockUI();
            return postJSON("/api/news/settings/update",
                ko.toJS(self),
                function(result) {
                    if (result.succeed === true) {
                    }
                });
        } else {
            dm.errors.showAllMessages();
        }
    };
}
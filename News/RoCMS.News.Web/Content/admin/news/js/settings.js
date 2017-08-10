function newsSettingsLoaded() {
    var vm = {
        settings: ko.validatedObservable(),
    };

    getJSON("/api/news/settings/get", "", function (result) {
        vm.settings(new App.Admin.NewsSettings(result));
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });

    ko.applyBindings(vm);
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
            postJSON("/api/news/settings/update", ko.toJS(self), function(result) {
                    if (result.succeed === true) {
                    }
                })
                .fail(function() {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function() {
                    unblockUI();
                });
        } else {
            dm.errors.showAllMessages();
        }
    };
}
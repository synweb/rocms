function telegramBotSettingsLoaded() {
    var vm = {
        settings: ko.validatedObservable(),
    };

    getJSON("/api/telegrambot/settings/get", "", function (result) {
        vm.settings(new App.Admin.TelegramBotSettings(result));
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });

    ko.applyBindings(vm);
}

App.Admin.TelegramBotSettings = function (data) {
    var self = this;

    self.proxyServer = ko.observable();
    self.proxyPort = ko.observable().extend({ number: true });
    self.proxyLogin = ko.observable();
    self.proxyPassword = ko.observable();
    self.apiKey = ko.observable();
    self.webHookToken = ko.observable();
    self.allowedUserPhones = ko.observable();

    self.allowedUserPhones.subscribe(function() {
        self.allowedUserPhones(self.allowedUserPhones().replace(/\+/g, ''));
    });

    self.webhookurl = ko.computed(function() {
        if (self.webHookToken()) {
            return "WebHook: " + location.protocol + "//" + location.host+ "/api/webhook/telegrambot/" + self.webHookToken()+ "/receive";
        }
        return "";
    });

    if (data) {
        if (data.proxyPort != 0) {
            self.proxyPort(data.proxyPort);
        }
        if (data.proxyServer) {
            self.proxyServer(data.proxyServer);
        }
        if (data.proxyLogin) {
            self.proxyLogin(data.proxyLogin);
        }
        if (data.proxyPassword) {
            self.proxyPassword(data.proxyPassword);
        }
        if (data.apiKey) {
            self.apiKey(data.apiKey);
        }
        if (data.webHookToken) {
            self.webHookToken(data.webHookToken);
        }
        if (data.allowedUserPhones) {
            self.allowedUserPhones(data.allowedUserPhones);
        }
    }

    self.save = function () {
        blockUI();
        postJSON("/api/telegrambot/settings/update", ko.toJS(self), function (result) {
            if (result.succeed === true) {
            }
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
    };
};
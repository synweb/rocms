/// <reference path="rocms.app.js" />
/// <reference path="../../vendor/knockout/knockout-3.0.0.debug.js" />

App.Account = {
    username: ko.observable(),
    password: ko.observable(),
    isAuthorized: ko.observable(false),
    userId: ko.observable(),


    authorize: function (onSuccess) {
        var self = this;
        postJSON("/Home/Login", { user: ko.toJS(self) }, function (result) {
            if (result.Succeed === true) {
                App.Account.userId(result.Data.UserId);
                App.Account.isAuthorized(true);
                if (onSuccess) {
                    onSuccess();
                }
            } else {
                alert("Неправильное имя или пароль пользователя");
            }
        });
    }
}

$(function () {
    getJSON("/api/user/current/info/get", "", function (user) {
        if (user) {
            App.Account.isAuthorized(true);
            App.Account.username(user.username);
            App.Account.userId(user.userId);
        }
    });
});
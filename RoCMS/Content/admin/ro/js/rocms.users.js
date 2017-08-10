/// <reference path="admin-ajax.js" />
function onUserListLoaded() {
    var vm = {
        users: ko.observableArray(),
        
    }
    blockUI();


    getJSON("/api/users/get", {}, function (res) {
        //alert($.j(res));
        if (res.succeed) {
            $(res.data).each(function() {
                var user = ($.extend(new App.Admin.User(), App.Admin.UserFunctions));
                user.init(this);
                if (this.username !== "synweb") {
                    vm.users.push(user);
                }
            });
        }

    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });

    ko.applyBindings(vm);

}

function onUserEditorLoaded() {
    $('#adminContent').on("click", ".btn-save", function () {
        var array = $(".user-resource-checkbox:checked").map(function() {
            return $(this).data("resourceId");
        }).get();
        var userId = $(".user-resources").data("userId");
        var data = { userId: userId, resourceIds: array };
        postJSON("/api/user/resources/update", data, function(res) {
            if (res.succeed) {
                smartAlert("Права изменены");
            } else {
                smartAlert("Ошибка при изменении прав");
            }
            $('.change-pass').hide();
            $(".change-user-pass-link").show();
        });
    });



    $("#userProfile .btn-save-profile").click(function () {
        var $form = $("#userProfile");
        $.validator.unobtrusive.parse($form, true);

        if ($form.valid()) {

            blockUI();
            var data = $form.serializeObject();

            postJSON("/api/user/profile/update", data, function (result) {
                if (result.succeed) {
                    smartAlert("Данные успешно обновлены");
                } else {
                    smartAlert("Произошла ошибка, проверьте данные");
                }
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        }
        return false;
    });

    $('#adminContent').on("click", ".save-user-password", function () {
        var password = $('.new-user-pass').val();
        var repeatpass = $('.repeat-user-pass').val();
        var userId = $(".user-resources").data("userId");
        if (password && repeatpass && password == repeatpass) {
            blockUI();
            postJSON('/Admin/EditUserPassword',
                { Password: password, UserId: userId },
                function (result) {
                    $('.change-pass').hide();
                    $(".change-user-pass-link").show();
                }).always(function () {
                    unblockUI();
                    var emailpassword = $('.new-user-pass').val("");
                    var repeatpass = $('.repeat-user-pass').val("");
                }).fail(function () {
                    alert("Произошла ошибка, проверьте правильность ввода.");
                });
        }
        else {
            alert("Пароли должны совпадать");
        }
        return false;
    });

    $('#adminContent').on("click", ".change-user-pass-link", function () {
        $('.change-pass').show();
        $(this).hide();
        return false;
    });
}

App.Admin.User = function () {
    var self = this;
    self.username = ko.observable().extend({ required: true });
    self.userId = ko.observable();
    self.creationDate = ko.observable();
    self.password = ko.observable().extend({ required: true });
}

App.Admin.UserFunctions = {
    init: function(data) {
        var self = this;
        self.username(data.username);
        self.userId(data.userId);
        self.creationDate(data.creationDate);
        self.password(data.password);
    },
    deleteUser: function (item, parent) {
        var self = this;
        if (self.userId() === 1) {
            return;
        }
        postJSON("/api/user/"+self.username()+"/delete", null, function (res) {
            if (res.succeed) {
                smartAlert("Пользователь удалён");
                location.reload();
            } else {
                smartAlert("Ошибка при удалении пользователя");
            }
        });
    }
}

$(function () {
    $('#adminContent').on("click", ".admin-add-user", function() {
        showAddUserDialog();
    });

    //$('#adminContent').on("click", ".admin-remove-user", function() {
    //    var container = $('.usernames');
    //    var username = container.val();
    //    if (username == 'admin') {
    //        return false;
    //    }
    //    var url = '/api/user/'+username+'/delete';
    //    $.ajax({
    //        url: url,
    //        type: 'POST',
    //        data: JSON.stringify({ username: username }),
    //        contentType: "application/json"
    //    }).done(function() {
    //        $('.usernames option:contains(' + username + ')').remove();
    //    });
    //    return false;
    //});
});
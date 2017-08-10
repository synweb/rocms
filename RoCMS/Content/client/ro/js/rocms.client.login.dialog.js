function BindAuthorizeButton() {
    $(".authorize-btn").click(function () {
        showBootstrapDialogFromUrl("/Home/LoginBlock", {
            removeButtons: true,
            onBeforeShow: function () {
                var $dialog = $(this);
                $dialog.find(".modal-dialog").addClass("modal-sm");
            },
            onShow: function () {
                var $dialog = $(this);
                $dialog.find(".sign-in").click(function () {
                    var login = $dialog.find(".login").val();
                    var pass = $dialog.find(".pass").val();

                    if ($.trim(login) && $.trim(pass)) {
                        postJSON("/Home/Login", { Username: login, Password: pass }, function (data) {
                            if (data.Succeed != false) {
                                alert("OK");
                            } else {
                                alert("Неправильное имя или пароль пользователя");
                            }

                        });


                    }
                });

            }
        });
        return false;
    });
}
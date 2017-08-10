$(function() {
    $('.register-block').on("click", ".register", function () {
        var $form = $('.register-block');
        $.validator.unobtrusive.parse($form, true);

        if ($form.valid()) {
            var pass = $('.textbox-password').val();
            var passRepeat = $('.textbox-password-repeat').val();
            if (pass != passRepeat) {
                return false;
            }
            var username = $('.textbox-username').val();
            var email = $('.textbox-email').val();
            $.ajax({
                url: "/Home/Register",
                type: 'POST',
                data: JSON.stringify({
                    Username: username,
                    Email: email,
                    Password: pass
                }),
                contentType: "application/json",
                success: function() {
                    document.location.href = "/";
                }
            });
            return false;
        } else {
            $form.validate().focusInvalid();
        }
    });

});
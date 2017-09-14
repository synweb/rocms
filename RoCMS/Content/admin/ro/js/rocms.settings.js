function onSettingsLoaded() {
    $('#adminContent').on("click", ".save-settings", function () {
        var $container = $(".setting-container");
        var form = $container.find("form");
        $.validator.unobtrusive.parse(form, true);

        if (!form.valid()) {
            form.validate().focusInvalid();
            return false;
        }
        var sitename = $('.site-name').val();
        var mainmenuid = $('.mainmenu-id').val();
        var mainpageurl = $('.mainpage-id').val();
        var rootBreadcrumbsTitle = $(".root-breadcrumbs-title").val();
        var reviews = $('.review-checkbox').is(':checked');
        var gallery = $('.gallery-checkbox').is(':checked');
        var yaId = $('.ya-metrika-id').val();
        var timezone = $('select#timezone').val();
        var rooturl = $('.root-url').val();
        var imagemaxheight = $('.image-max-height').val();
        var imagemaxwidth = $('.image-max-width').val();
        var imagequality = $('.image-quality').val();
        var thumbnailSizes = $('.thumbnail-sizes').val();
        var autoemailreplyenabled = $(".auto-email-replyenabled").is(':checked');
        var emailsmtpurl = $('.email-smtp-url').val();
        var emailsmtpport = $('.email-smtp-port').val();
        var smtpsslenabled = $(".smtp-ssl-enabled").is(':checked');
        var emaillogin = $('.email-login').val();
        var orderemailaddress = $('.order-email-address').val();
        var systememailaddress = $('.system-email-address').val();
        var systememailsendername = $('.system-email-sender-name').val();
        var translitEnabled = ($(".translit-enabled:checked").val() === "translit");
        var allowedFileExtensions = $(".allowed-files").val().replace(/ /g, '');
        var youtubeAPIKey = $('#YoutubeAPIKey').val();
        var reviewSort = $('#reviewSort').val();
        var reviewCreatedNotification = $('.review-created-notificaton').is(':checked');


        blockUI();
        $.ajax({
            url: '/Admin/SaveSettings',
            type: 'POST',
            data: JSON.stringify({
                MainMenuId: mainmenuid,
                SiteName: sitename,
                MainPageUrl: mainpageurl,
                Reviews: reviews,
                Gallery: gallery,
                YaMetrika: yaId,
                Timezone: timezone,
                RootUrl: rooturl,
                ImageMaxHeight: imagemaxheight,
                ImageMaxWidth: imagemaxwidth,
                ImageQuality: imagequality,
                AutoEmailReplyEnabled: autoemailreplyenabled,
                EmailSmtpUrl: emailsmtpurl,
                EmailSmtpPort: emailsmtpport,
                SmtpSslEnabled: smtpsslenabled,
                EmailLogin: emaillogin,
                OrderEmailAddress: orderemailaddress,
                SystemEmailAddress: systememailaddress,
                SystemEmailSenderName: systememailsendername,
                TranslitEnabled: translitEnabled,
                RootBreadcrumbsTitle: rootBreadcrumbsTitle,
                AllowedFileExtensions: allowedFileExtensions,
                YoutubeAPIKey: youtubeAPIKey,
                ThumbnailSizes: thumbnailSizes,
                ReviewSort: reviewSort,
                ReviewCreatedNotification: reviewCreatedNotification,
            }),
            contentType: "application/json",
            success: function (data) {

            }
        }).fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
            .always(function () {
                unblockUI();
                $('.change-pass').hide();
                $(".change-email-pass-link").show();
            });
        return false;
    });

    $('#adminContent').on("click", ".change-email-pass-link", function () {
        $('.change-pass').show();
        $(this).hide();
        return false;
    });


    $('#adminContent').on("click", ".save-email-password", function () {
        var emailpassword = $('.new-email-pass').val();
        var repeatpass = $('.repeat-email-pass').val();
        if (emailpassword && repeatpass && emailpassword === repeatpass) {
            blockUI();
            postJSON('/Admin/UpdateEmailPassword',
                { EmailPassword: emailpassword },
                function (result) {
                    $('.change-pass').hide();
                    $(".change-email-pass-link").show();
                }).always(function () {
                    unblockUI();
                    var emailpassword = $('.new-email-pass').val("");
                    var repeatpass = $('.repeat-email-pass').val("");
                }).fail(function () {
                    alert("Произошла ошибка, проверьте правильность ввода.");
                });
        }
        else {
            alert("Пароли должны совпадать");
        }
        return false;
    });

    var btn = $('.get-ya-auth-key');
    var input = $(".ya-metrika-id");
    if (!input.val()) {
        btn.attr("disabled", "disabled");
    }

    input.on("change paste keyup", function () {
        if (input.val()) {
            btn.removeAttr("disabled");
        } else {
            btn.attr("disabled", "disabled");

        }
    });

    $("#adminContent").on("click", ".get-ya-auth-key", function () {
        window.open("https://oauth.yandex.ru/authorize?response_type=code&client_id=12876332b1104b6695e3c211145a3cd6", '_blank');
        showPromptDialog2({
            title: "Введите код подтверждения",
            onOkHandler: function (res) {
                var url = "/api/settings/yandex/auth";
                var code = res.promptValue;
                blockUI();
                postJSON(url, code, function (res2) {
                    if (!res2.succeed) {
                        alert(res2.message);
                        return;
                    }
                    $(".token-expiration").html("(Истекает: " + (new Date(res2.data.expiration)).format('dd.mm.yyyy') + ")");
                })
                    .fail(function () {
                        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                    })
                    .always(function () {
                        unblockUI();
                    });
            }
        });
        return false;
    });
}
$(function () {

    $(".order-callback").click(function () {

        var url = '/Home/OrderCallback';
        showBootstrapDialogFromUrl(url, {
            //removeButtons: true,
            onBeforeShow: function () {
                var $dialog = $(this);
                $dialog.find(".modal-dialog").addClass("modal-md");
            },
            onShow: function () {
                


            },
            onOk: function () {

                var $div = $(".callback-container");
                $.validator.unobtrusive.parse($div, true);
                $div.validate();
                if ($div.valid()) {
                    var sendurl = "/api/message/send/callback";
                    var name = $("#callback-name").val();
                    var phone = $("#callback-phone").val();
                    var time = $("#callback-time").val();
                    var $dialog = $(this);
                    postJSON(sendurl, { Name: name, Phone: phone, Text: time }, function () {
                        $dialog.modal("hide");
                        //$("#callback-order-success").show();
                        $("#callback-name").val('');
                        $("#callback-phone").val('');
                        $("#callback-time").val('');
                        setTimeout(function () {
                            //$("#callback-order-success").hide(1000);

                        }, 10000);
                    });
                } else {
                    $div.validate().focusInvalid();
                }
                return false;
            }
        });
        return false;
    });
});
function formRequestsLoaded() {

	$('#adminContent').on("click", ".formRequest-summary .button-delete", function () {
		if (!confirmRemoval()) {
			return false;
		}
		var container = $(this).parents(".formRequest-summary");
		var url = $(this).attr("href");
		blockUI();
		postJSON(url, "", function () {
			container.hide(1000, function () { container.remove(); });			
		})
            .fail(function () {
		    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
		return false;
    });

    $(".formRequest-summary .form-request-state").change(function() {
        var state = $(this).val();
        var id = $(this).closest(".formRequest-summary").data("blockId");
        postJSON("/api/formrequest/" + id +"/" + state +"/changestate", "", function(result) {
            if (result.Succeed === false) {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            } else {
                smartAlert("Статус обновлен");
            }
        });
    });


    $(".formRequest-summary .form-request-payment-state").change(function () {
        var state = $(this).val();
        var id = $(this).closest(".formRequest-summary").data("blockId");
        postJSON("/api/formrequest/" + id + "/" + state + "/changepaymentstate", "", function (result) {
            if (result.Succeed === false) {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            } else {
                smartAlert("Статус обновлен");
            }
        });
    });


};
$(function () {

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
});
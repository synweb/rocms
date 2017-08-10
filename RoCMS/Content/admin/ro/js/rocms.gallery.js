onAdminGalleryLoaded = function () {


    $('#adminContent').on("click", ".gallery-table td .delete", function () {
        var imageId = $(this).data('id');
        console.log(imageId);
        var that = $(this);
        blockUI();
        postJSON("/api/image/remove/" + imageId, null, function (res) {
                if (res.succeed) {
                    var container = that.closest("tr");
                    container.hide(1000, function() {
                        container.remove();
                    });
                } else {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                }
            })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
        
        return false;
    });
}
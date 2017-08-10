function onVideoAlbumListLoaded() {
    $('#adminContent').on("click", ".video-album-create", function () {
        showAlbumCreateDialog(function (title) {
            //TODO: валидация
            postJSON('/api/video/album/create', title, function () {
                //    RefreshAlbumsList().done(function () {
                //        unblockUI();
                //    });
                refresh();
            });

        });
    });
    $('#adminContent').on("click", ".video-albums-list .delete", function () {
        if (confirmRemoval()) {
            blockUI();
            var that = $(this);
            var id = that.data("albumId");
            postJSON('/api/video/album/' + id + '/delete', null, function () {
                var container = that.parents(".box.video-album");
                container.hide(1000, function () {
                    container.remove();
                });
            }).fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
        }
    });
}

function RefreshAlbumsList() {
    return $.get('/Admin/Albums', function (data) {
        $('#adminContent').html(data);
    });
}
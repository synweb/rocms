/// <reference path="../../../base/vendor/jquery/core/jquery-2.0.2.js" />
/// <reference path="../js/admin-ajax.js" />
/// <reference path="../js/admin.dialogs.js" />


function onAlbumListLoaded() {
    $('#adminContent').on("click", ".album-create", function() {
        showAlbumCreateDialog(function(title) {
            //TODO: валидация
            postJSON('/api/album/create', title, function() {
            //    RefreshAlbumsList().done(function () {
            //        unblockUI();
                //    });
                refresh();
            });
            
        });
    });


    $('#adminContent').on("click", ".albums-list .delete", function () {
        if (!confirmRemoval()) { return false; }
        blockUI();
        var that = $(this);
        var id = that.data("albumId");
        postJSON('/api/album/' + id + '/delete', null, function() {
            var container = that.parents("li.album");
            container.hide(1000, function() {
                 container.remove();
            });
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
    });
}

function RefreshAlbumsList() {
    return $.get('/Admin/Albums', function(data) {
        $('#adminContent').html(data);
    });
}
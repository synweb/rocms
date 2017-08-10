
function galleryLoaded() {
    $(".thumbnail-list li").click(function() {
        var id = $(this).data("imageId");
        var prevId = $(this).data("imagePrev");
        var nextId = $(this).data("imageNext");
        
        var url = actionUrls.Gallery_ViewImage.replace("_id_", id).replace("_prevId_", prevId).replace("_nextId_", nextId);
        var options = {
            dialogClass: 'dialog-invis-title',
            width: 600,
            height: window.innerHeight - 50,
            modal: true,
            open: function() {
                $(".nav-overlay").blur();
                $('.ui-widget-overlay').bind('click', function() {
                    $('.current-image-container').dialog('close');
                });
                $('.image-nav-prev').bind('click', function () {
                    var selector = "li[data-image-id=" + prevId + "]";
                    $(selector).click();
                    $(".current-image-container").dialog('close');
                    return false;
                });
                $('.image-nav-next').bind('click', function() {
                    var selector = "li[data-image-id=" + nextId + "]";
                    $(selector).click();
                    $(".current-image-container").dialog('close');
                    return false;
                });
            },
            resizable: false,
            draggable: false,
        };
        showDialogFromUrl(url, options);
    });
}
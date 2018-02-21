function updateFavMenuState() {
    getJSON("/api/shop/favourite/any", "", function(result) {
        if (result.succeed === true) {
            if (result.data.hasItems === true) {
                $(".menu-fav").find("em").removeClass("fa-star-o").addClass("fa-star");
            } else {
                $(".menu-fav").find("em").removeClass("fa-star").addClass("fa-star-o");
            }
        }
    });
}

function addToFavourites(heartId, onSuccess) {
    postJSON("/api/shop/favourite/" + heartId + "/add", "", function (result) {
        if (result.succeed === true) {
            if (onSuccess) {
                onSuccess();
            }
            updateFavMenuState();
        }
    });
}

function removeFromFavourites(heartId, onSuccess) {
    postJSON("/api/shop/favourite/" + heartId + "/delete", "", function (result) {
        if (result.succeed === true) {
            if (onSuccess) {
                onSuccess();
            }
            updateFavMenuState();
        }
    });
}

$(function() {
    $(document).on("click",
        ".offer .fav",
        function() {
            var heartId = $(this).data("heartId");
            var self = this;
            if ($(this).hasClass("in")) {
                removeFromFavourites(heartId,
                    function() {
                        $(self).removeClass("in");
                        $(self).find("em").removeClass("fa-star").addClass("fa-star-o");
                        $(self).find("span").text("В избранное");
                    });
            } else {
                addToFavourites(heartId,
                    function() {
                        $(self).addClass("in");
                        $(self).find("em").removeClass("fa-star-o").addClass("fa-star");
                        $(self).find("span").text("Убрать из избранного");
                    });
            }
            return false;
        });
});


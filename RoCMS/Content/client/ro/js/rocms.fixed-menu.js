jQuery(window).load(function() {

    $(".fixedmenu ul.my_menu").removeClass("nav");
    $(".fixedmenu ul.my_menu").removeClass("navbar-nav");
    $(".fixedmenu ul.my_menu").addClass("menu");


        var menu = jQuery('.mainmenu');
        var h = 320;
        jQuery(window).scroll(function() {
            if (!menu.isOnScreen() && jQuery(this).scrollTop() > h) {
                jQuery(".fixedmenu").slideDown(200);
            } else {
                jQuery(".fixedmenu").slideUp(200);
            }
        });
        jQuery.fn.isOnScreen = function() {

            var win = jQuery(window);

            var viewport = {
                top: win.scrollTop(),
                left: win.scrollLeft()
            };
            viewport.right = viewport.left + win.width();
            viewport.bottom = viewport.top + win.height();

            if (this.offset()) {
                var bounds = this.offset();
                bounds.right = bounds.left + this.outerWidth();
                bounds.bottom = bounds.top + this.outerHeight();

                return (!(viewport.right < bounds.left || viewport.left > bounds.right || viewport.bottom < bounds.top || viewport.top > bounds.bottom));
            }
        }

});
﻿/// <reference path="../../base/vendor/jquery/core/jquery-2.0.2.js" />
/// <reference path="../../base/ro/js/rocms.helpers.js" />


function toggleHideMenuCookie() {
    var hideMenuCookieName = "hideAdminMenu";
    var hideMenuCookie = getCookie(hideMenuCookieName);
    if (!hideMenuCookie || hideMenuCookie !== "1") {
        if ($('.logo-container').css('display') !== 'none') {
            setCookie(hideMenuCookieName, "1", 31536000);
        }
    } else {
        deleteCookie(hideMenuCookieName);
    }
}

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $(".menu-toggle-button").click(function() {
        $("body").toggleClass("menu-hidden");
        toggleHideMenuCookie();
    });
});
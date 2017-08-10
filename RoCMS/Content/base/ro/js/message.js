/// <reference path="rocms.helpers.js" />

function sendMessage(message, type, callback, always) {
    var url = "/api/message/send/" + type;
    postJSON(url, message, function (result) {
        if (callback)
            callback(result);
    }).always(function () {
        if (always)
            always();
    });
}
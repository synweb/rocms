/// <reference path="../../vendor/jquery/ui-short/jquery.blockUI.js" />

$.validator.methods.date = function (value, element) {
    var ptrn = /^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02\.((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))) (([0,1][0-9])|(2[0-3])):[0-5][0-9]$/;
    return this.optional(element) || ptrn.test(value);
};

$(function() {
//Определяет, IE ли это
    if (window.ActiveXObject || "ActiveXObject" in window) {
        $.ajaxSetup({
            //Запрещаем кэширование ajax для IE
            cache: false
        });
    }
});

function showDialogFromUrl(url, options) {
    $.get(url, function (result) {
        if (!options.position) {
            options.position = { my: "center bottom", at: "center center", of: window};
        }
        if (!options.close) {
            options.close = function () {
                $(this).dialog('destroy');
                $(this).remove();
            }
        }
        
        $(result).dialog(options);
    });
}

function showPromptDialog2(options) {
    return showPromptDialog(
        options.title,
        options.promptText,
        options.defaultValue,
        options.okButtonText,
        options.cancelButtonText,
        options.onOkHandler,
        options.onCancelHandler,
        options.promptWidth,
        options.width,
        options.height
    );
}

function showPromptDialog(title, promptText, defaultValue, okButtonText, cancelButtonText, onOkHandler, onCancelHandler, promptWidth, width, height) {
    var dialogContent = $(
    '<div style="display: none;">' +
        '<form id="promptForm" class="form-horizontal">' +
            '<label for="promptInput"/>' +
            '<input class="form-control" data-val-required="" data-val="true" id="promptInput" type="text"/>' +
        '</form>' +
    '</div>');
    var options = {
        title: title,
        width: width == null ? 480 : width,
        height: height == null ? 200 : height,
        modal: true,
        autoOpen: false,
        resizable: false,
        buttons: [
            {
                text: okButtonText?okButtonText:"OK",
                click: function () {
                    jQuery.validator.unobtrusive.parse($(this).find('#promptForm'));
                    if ($("#promptForm", $(this)).valid()) {
                        this.promptValue = $.trim($('#promptInput', $(this)).val());
                        this.close = function () { $(this).dialog("close"); };
                        onOkHandler(this);
                        $(this).dialog("close");
                    }
                }
            },
            {
                text: cancelButtonText?cancelButtonText:"Отмена",
                click: function () {
                    if (onCancelHandler != null) {
                        onCancelHandler();
                    }
                    $(this).dialog("close");
                }
            }
        ],
        close: function () {
            $(this).dialog('destroy');
            $(this).remove();
        }
    };
    dialogContent.find('input').keypress(function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $(this).parents('.ui-dialog').first().find('.ui-dialog-buttonpane').find('button:first').click();
            return false;
        }
    });

    if (promptText) {
        dialogContent.find('label').html(promptText);
    } else {
        dialogContent.find('label').hide();
    }
    
    dialogContent.find('#promptInput').val(defaultValue).css('width', promptWidth == null ? '100%' : promptWidth);
    dialogContent.dialog(options);
    dialogContent.dialog('open');
    return dialogContent;
}

//options: {removeButtons : bool, onOk : function, onShow : function, onBeforeShow : function}
function showBootstrapDialogFromUrl(url, options) {
    $.get(url, function (result) {
        showBootstrapDialog(result, options);
    });
}

function showBootstrapDialog(content, options) {
    var clone = $('.dialog-container').clone();
    clone.removeClass('dialog-container');
    $("body").append(clone);
    clone.find('.modal-body').html(content);

    if (options.removeButtons === true) {
        clone.find('.modal-footer').remove();
    }
    clone.on("hidden.bs.modal", function () {
        clone.remove();
    });
    clone.on("show.bs.modal", function () {
        if (options.onBeforeShow) {
            options.onBeforeShow.call(clone);
        }
    });
    clone.on("shown.bs.modal", function () {
        if (options.onShow) {
            options.onShow.call(clone);
        }
    });
    clone.find(".button-ok").click(function () {
        if (options.onOk) {
            options.onOk.call(clone);
        }
    });
    clone.modal();
}

function postJSON(url, data, onSuccess) {

    var csrfToken = $("input[name='__RequestVerificationToken']").val();

    return $.ajax({
        headers: csrfToken ? { __RequestVerificationToken: csrfToken } : '',
        url: url,
        type: 'POST',
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function(result) {
            if (onSuccess) {
                onSuccess(result);
            }
        }
    });
}

function getJSON(url, data, onSuccess) {
    return $.ajax({
        url: url,
        type: 'GET',
        data: data,
        contentType: "application/json",
        success: function (result) {
            if (onSuccess) {
                onSuccess(result);
            }
        }
    });
}

function getJSONP(url, data, onSuccess) {
    return $.ajax({
        url: url,
        type: 'GET',
        data: data,
        dataType: "jsonp",
        success: function (result) {
            if (onSuccess) {
                onSuccess(result);
            }
        }
    });
}

function blockUI() {
    $.blockUI({ message: '<h3><img src="/Content/base/ro/img/ajax-loader.gif" /> Пожалуйста, подождите...</h3>' });
}

function unblockUI() {
    $.unblockUI();
}

function isFunction(functionToCheck) {
    var getType = {};
    return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
}

function textToUrl(text) {
    var res;
    if (translitEnabled === true) {
        res = toTranslit(text);
    } else {
        res = toCyrillicUrl(text);
    }
    
    return res;
}

function toCyrillicUrl(text) {
    return text.replace(/([а-яё])|([\s_-])|([^a-z\d])/gi,
    function (all, ch, space, words, i) {
        if (space || words) {
            return space ? '-' : '';
        }
        return ch;
    });
}

function toTranslit(text) {
    return text.replace(/([а-яё])|([\s_-])|([^a-z\d])/gi,
    function (all, ch, space, words, i) {
        if (space || words) {
            return space ? '-' : '';
        }
        var code = ch.charCodeAt(0),
            index = code == 1025 || code == 1105 ? 0 :
                code > 1071 ? code - 1071 : code - 1039,
            t = ['yo', 'a', 'b', 'v', 'g', 'd', 'e', 'zh',
                'z', 'i', 'y', 'k', 'l', 'm', 'n', 'o', 'p',
                'r', 's', 't', 'u', 'f', 'h', 'c', 'ch', 'sh',
                'shch', '', 'y', '', 'e', 'yu', 'ya'
            ];
        return t[index];
    });
}

function convertObjectToUriParams(obj) {
    var objects = Object.keys(obj).map(function (key) {
        if (obj[key] && (($.isArray(obj[key]) == false))) {
            return encodeURIComponent(key) + '=' + encodeURIComponent(obj[key]);
        }
        else if (obj[key] && ($.isArray(obj[key]) == true) && (obj[key].length > 0)) {
            //return encodeURIComponent(key) + '=' + obj[key].map(function (o) {
            //    return encodeURIComponent(o);
            //}).join(',');

            return obj[key].map(function (o, index) {
                return encodeURIComponent(key) + "[" + index + "]=" + encodeURIComponent(o);
            }).join('&');

        }
        else {
            return "";
        }
    });
    objects = jQuery.grep(objects, function (a) {
        return a !== "";
    });

    var str = objects.join('&');
    return str;
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || undefined);
        } else {
            o[this.name] = this.value || undefined;
        }
    });
    return o;
};

function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
      "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function setCookie(name, value, options) {
    options = options || {};

    var expires = options.expires;

    if (typeof expires == "number" && expires) {
        var d = new Date();
        d.setTime(d.getTime() + expires * 1000);
        expires = options.expires = d;
    }
    if (expires && expires.toUTCString) {
        options.expires = expires.toUTCString();
    }

    value = encodeURIComponent(value);

    var updatedCookie = name + "=" + value;

    for (var propName in options) {
        updatedCookie += "; " + propName;
        var propValue = options[propName];
        if (propValue !== true) {
            updatedCookie += "=" + propValue;
        }
    }

    document.cookie = updatedCookie;
}

function deleteCookie(name) {
    setCookie(name, "", {
        expires: -1
    })
}
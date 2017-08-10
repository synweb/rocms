//Извлечение переменных из урла
$(function () {
	$.QueryString = (function (a) {
		if (a == "") return {};
		var b = {};
		for (var i = 0; i < a.length; ++i) {
			var p = a[i].split('=');
			if (p.length != 2) continue;
			b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
		}
		return b;
	})(window.location.search.substr(1).split('&'))
});


function signIn() {
    var username = $(".textbox-username").val();
    var pass = $(".textbox-password").val();
    var returnUrl = $.QueryString["ReturnUrl"];
    var url = "/Home/Login";
    postJSON(url, { Username: username, Password: pass, ReturnUrl: returnUrl }, function (data) {
        if (data.Succeed != false) {
            window.location.href = returnUrl;
        }
    });
    return false;
}

function logout() {
    getJSON("/Home/Logout", null, function () {
        window.location.reload();
    });
    return false;
}
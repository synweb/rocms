$(function () {
    $('.head .btn-toolbar').on("click", "#restart-btn", function () {
        if (!confirm('Вы уверены, что хотите перезапустить сайт? Все сессии и кэши будут сброшены.')) {
            return false;
        }
    });
});
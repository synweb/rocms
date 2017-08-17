/// <reference path="./rocms.menu.js" />

function pageLoaded() {
    $(".order-link").click(function () {
        showOrderDialog();
        return false;
    });
}

function menuPageLoaded() {
    $(".menu-list").on("click", ".deleteLink", function () {
        if (!confirmRemoval()) {
            return false;
        }
        var $this = $(this);
        var url = $(this).attr("href");
        $.post(url, "", function () {
            $this.closest("li").remove();
        });
        return false;
    });
}

function menuEditorLoaded(menuId) {
    var nmenu = new App.Admin.Menu(menuId);
    var $menu = $('.menu-info');

    var vm = {
        menu: nmenu,
        pages: ko.observable(),
        blocks: ko.observable()
    };
    getJSON("/api/page/pages/get", "", function (result) {
        result.splice(0, 0, { relativeUrl: null, title: 'Выберите страницу...' });
        vm.pages(result);
        getJSON("/api/block/blocks/get", "", function (result2) {
            result2.splice(0, 0, { blockId: null, title: 'Выпадающий блок меню...' });
            vm.blocks(result2);
            ko.applyBindings(vm, $menu[0]);
        });
    });

    $(".menu-save-button").click(function () {
        nmenu.save().done(function () {
            if (menuId === undefined) {
                window.location.href = "/Admin/MenuEditor/" + nmenu.menuId;
            } else {
                smartAlert("Данные успешно обновлены");
            }
        });

        return false;
    });
}


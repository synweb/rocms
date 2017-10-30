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
    var nmenu = new App.Admin.Menu();
    var $menu = $('.menu-info');

    var vm = {
        menu: nmenu,
        pages: ko.observable(),
        blocks: ko.observable()
    };



    var initOnLoad = function (pages, blocks) {
        pages.splice(0, 0, { heartId: null, title: 'Выберите страницу...', type: 'Не выбрано' });
        vm.pages(pages);

        blocks.splice(0, 0, { blockId: null, title: 'Выпадающий блок меню...' });
        vm.blocks(blocks);

        ko.applyBindings(vm, $menu[0]);

        $(".withsearch").selectpicker();
    }

    if (menuId) {
        $.when(
            getJSON("/api/heart/hearts/get", ""),
            getJSON("/api/block/blocks/get", ""),
            nmenu.init(menuId)
        ).then(function(result, result2) {
            initOnLoad(result[0], result2[0]);
        });
    } else {
        $.when(
            getJSON("/api/heart/hearts/get", ""),
            getJSON("/api/block/blocks/get", "")
        ).then(function(result, result2) {
            initOnLoad(result[0], result2[0]);
        });
    }

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


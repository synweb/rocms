function priceExportLoaded() {
    var vm = {
        exportTasks: ko.observableArray(),
        startExport: function () {
            $(".start-price-export").attr('disabled', 'disabled');
            postJSON('/api/shop/export/prices/start', null, function (res) { });
        }
    };

    getJSON("/api/shop/export/prices/tasks", "", function (result) {
        $(result).each(function () {
            vm.exportTasks.push(ko.mapping.fromJS(this));
        });
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });

    ko.applyBindings(vm);
}
function onSupportTicketsLoaded() {
    var vm = {
        tickets: ko.observableArray(),
        removeTicket: function(data) {
            alert(4);
        }
    }
    blockUI();

    getJSON("/api/support/tickets/1/10/get", {}, function (res) {
        if (res.succeed) {
            $(res.data.tickets).each(function () {
                vm.tickets.push(this);
            });
        }
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });

    ko.applyBindings(vm);
}

function ticketTypeDescription(ticketType) {
    switch(ticketType) {
        case "Question":
            return "Общий вопрос";
        case "Order":
            return "Работа с заказом";
        case "Conflict":
            return "Конфликт";
        case "ReviewArgument":
            return "Оспаривание отзыва";
        case "Tech":
            return "Технический вопрос";
        case "Payments":
            return "Платежи";
        case "Offer":
            return "Предложение";
        case "Cheating":
            return "Мошенничество";
        case "Other":
            return "Прочее";
    }
}
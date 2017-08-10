function onSupportTicketsLoaded() {
    var vm = {
        tickets: ko.observableArray(),
        ticketTypes: ["question",
                    "order",
                    "conflict",
                    "reviewArgument",
                    "tech",
                    "payments",
                    "offer",
                    "cheating",
                    "other"],

        newTicket: ko.validatedObservable({
            text: ko.observable().extend({ required: true }),
            subject: ko.observable().extend({ required: true }),
            ticketType: ko.observable("question")
        }),
        clearNewTicket: function() {
            vm.newTicket({
                text: ko.observable().extend({ required: true }),
                subject: ko.observable().extend({ required: true }),
                ticketType: ko.observable("question")
            });
        },
        sendMessage: function (ticket) {
            var ticketId = ticket.ticketId();
            if (ticket.reply()) {
                postJSON("/api/support/message/send", { ticketId: ticketId, text: ticket.reply() }, function(res) {
                    if (res.succeed) {
                        getJSON("/api/support/message/" + res.data + "/get", "", function(data) {
                            ticket.messages.unshift(ko.mapping.fromJS(data));
                        });
                        ticket.reply("");
                    } else {
                        alert("Ошибка при отправке сообщения");
                    }
                });
            }
        },
        resolveTicket: function (ticket) {
            postJSON("/api/support/ticket/" + ticket.ticketId() + "/resolve", null, function (res) {
                if (res.succeed) {
                    ticket.resolved(true);
                } else {
                    alert("Ошибка при закрытии тикета");
                }
            });
        },
        createTicket: function () {
            if (vm.newTicket.isValid()) {
                blockUI();
                postJSON("/api/support/ticket/create", ko.toJS(vm.newTicket), function (result) {
                    if (result.succeed) {
                        vm.refreshTickets();
                        vm.clearNewTicket();
                    }
                    else {
                        alert("Произошла ошибка, попробуйте еще раз");
                    }
                }).always(unblockUI);
            }
            else {
                vm.newTicket.errors.showAllMessages();
            }
        },
        showTicket: function (ticket) {
            if (!ticket.ticket()) {
                blockUI();
                getJSON("/api/support/ticket/" + ticket.ticketId() + "/get", {}, function (res) {
                    ticket.ticket($.extend(ko.mapping.fromJS(res), {reply: ko.observable()}));
                    ticket.collapsed(false);
                })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
            }
            else {
                ticket.collapsed(!ticket.collapsed());
            }
        },
        refreshTickets: function () {
            vm.tickets.removeAll();
            blockUI();
            getJSON("/api/support/tickets/1/10/user/get", {}, function (res) {
                if (res.succeed) {
                    $(res.data.tickets).each(function () {
                        vm.tickets.push($.extend(ko.mapping.fromJS(this), { ticket: ko.observable(), collapsed: ko.observable(true) }));
                    });
                }
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        }
    }
    

    vm.refreshTickets();

    var ticketsDOM = $('.support-tickets');
    if (ticketsDOM.length) {
        ko.applyBindings(vm, ticketsDOM[0]);
    } else {
        ko.applyBindings(vm);
    }
    
}

function ticketTypeDescription(ticketType) {
    switch (ticketType.toLowerCase()) {
        case "question":
            return "Общий вопрос";
        case "order":
            return "Работа с заказом";
        case "conflict":
            return "Конфликт";
        case "reviewargument":
            return "Оспаривание отзыва";
        case "tech":
            return "Технический вопрос";
        case "payments":
            return "Платежи";
        case "offer":
            return "Предложение";
        case "cheating":
            return "Мошенничество";
        case "other":
            return "Прочее";
    }
}
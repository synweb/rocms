function onTicketMessagesLoaded() {
    $(document).on("click", ".ticket-answer .send-answer", function() {
        var text = $(".ticket-answer-field").val();
        if (text) {
            var ticketId = $(".ticket-details").data("ticketId");
            postJSON("/api/support/message/send", { ticketId: ticketId, text: text }, function(res) {
                if (res.succeed) {
                    location.reload();
                } else {
                    alert("Ошибка при отправке сообщения");
                }
            });
        } else {
            alert("Текст не может быть пустым");
        }

    });

    $(document).on("click", ".ticket-resolved", function () {
        var ticketId = $(".ticket-details").data("ticketId");
        postJSON("/api/support/ticket/" + ticketId + "/resolve", null, function (res) {
            if (res.succeed) {
                window.location.href = "/TicketsEditor";
            } else {
                alert("Ошибка при закрытии тикета");
            }
        });
    });
    
}
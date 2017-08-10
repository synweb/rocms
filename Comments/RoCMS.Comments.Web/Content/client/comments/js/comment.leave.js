function commentsLoaded(apiUrl) {
    $("button.leave-comment").click(function () {
        leaveComment(apiUrl, $(this).closest(".leave-comment-container"));
    });

    $(".comment-textarea").keydown(function (e) {
        if (e.ctrlKey && e.keyCode === 13) {
            // Ctrl-Enter pressed
            leaveComment(apiUrl, $(this).closest(".leave-comment-container"), true);
        }
    });

    var replyTemplate = $("#reply-template").html();

    $(".comments").on("click", "a.comment-reply", function () {
        $(".leave-reply-container").remove();
        var container = $(this).closest(".comment-container .comment_details");
        $(replyTemplate).insertAfter(container);
        return false;
    });

    $(".comments").on("click", ".hide-leave-reply-container", function () {
        $(".leave-reply-container").remove();
        return false;
    });

    $(".comments").on("click", "button.leave-reply", function () {
        leaveComment(apiUrl, $(this).closest(".leave-reply-container"), true);
    });

    $(".comments").on("keydown", '.reply-textarea', function (e) {
        if (e.ctrlKey && e.keyCode === 13) {
            // Ctrl-Enter pressed
            leaveComment(apiUrl, $(this).closest(".leave-reply-container"), true);
        }
    });
}

function leaveComment(apiUrl, $context, isReply) {
    var form = $context.find("form");
    $.validator.unobtrusive.parse(form, true);

    if (form.valid()) {


        var text = $context.find("textarea").val();
        var name = $context.find(".comment-name").val();
        var email = $context.find(".comment-email").val();
        var url = $context.find(".comment-url").val();
        var data = {
            text: text,
            name: name,
            email: email,
            url: url
        };

        if (isReply) {
            var parentCommentId = $context.closest(".comment-container").data("id");
            data.parentCommentId = parentCommentId;
        }

        postJSON(apiUrl, data, function () {
            location.reload();
        });
    } else {
        form.validate().focusInvalid();
    }
}
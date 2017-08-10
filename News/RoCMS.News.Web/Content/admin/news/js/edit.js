//App.Admin.News.NewsItem = function () {
//    var self = this;
//    self.newsId = ko.observable();
//    self.title = ko.observable();
//    self.relativeUrl = ko.observable();
//    self.text = ko.observable();
//    self.postingDate = ko.observable();
//    self.description = ko.observable();
//    self.metaDescription = ko.observable();
//    self.keywords = ko.observable();
//    self.creationDate = ko.observable();
//    self.authorId = ko.observable();
//    self.imageId = ko.observable();
//    self.commentTopicId = ko.observable();
//    self.tags = ko.observable();
//    self.recordType = ko.observable();
//    self.filename = ko.observable();
//    self.videoId = ko.observable();
//    self.categories = ko.observableArray();
//}

//App.Admin.News.NewsItemFunctions = {

//    addCategory: function () {
//        var self = this;
//        showNewsCategoriesDialog(function (category) {
//            var result = $.grep(self.categories(), function (e) {
//                return e.id() == category.id;
//            });
//            if (result.length == 0) {
//                self.categories.push(ko.mapping.fromJS(category));
//            }
//        });
//    },
//    removeCategory: function (category, parent) {
//        parent.categories.remove(function (item) {
//            return item.id() == category.id();
//        });
//    },
//}

function initNewsEditorKo(context, newsId) {

    var vm = {
        categories: ko.observableArray(),
        addCategory: function () {
            var self = this;
            showNewsCategoriesDialog(function (category) {
                var result = $.grep(self.categories(), function (e) {
                    return e.id() == category.id;
                });
                if (result.length == 0) {
                    self.categories.push(ko.mapping.fromJS(category));
                }
            });
        },
        removeCategory: function (category, parent) {
            parent.categories.remove(function (item) {
                return item.id() == category.id();
            });
        },
        save: function (url, context, onSuccess) {
            
            var $form = context.find("form");
            $.validator.unobtrusive.parse($form, true);
            if ($form.valid()) {
                blockUI();
                var title = $('.news-title').val();
                var relUrl = $('.news-relative-url').val();
                var text = getTextFromEditor('news_content');
                var tags = $('.news-tags').val();
                var keywords = $('.news-keywords').val();
                var description = $('.news-description').val();
                var metaDescription = $('.news-meta-description').val();
                var recType = $('.rectype input[type = "radio"]:checked').val();
                var postingDate = $('.news-publish-date').datepicker("getDate");
                var eventDate = $('.news-event-date').datepicker("getDate");
                var relatedNewsItem = $('.news-related-item').val();
                postingDate.setHours($('.news-publish-time').data('timepicker').hour);
                postingDate.setMinutes($('.news-publish-time').data('timepicker').minute);
                if (eventDate) {
                    eventDate.setHours($('.news-event-time').data('timepicker').hour);
                    eventDate.setMinutes($('.news-event-time').data('timepicker').minute);
                }
                var id = $(".news-items-info").data("newsId");
                var imageId = $(".news-img").data("id");
                var filename = $(".picked-file").data("filename");
                var videoId = $(".video-picked .video-thumb").data("videoId");

                var blogId = $(".news-blog-id").val();

                var cats = ko.toJS(vm.categories());

                postJSON(url, {
                    Title: title,
                    Text: text,
                    Keywords: keywords,
                    Description: description,
                    MetaDescription: metaDescription,
                    PostingDate: postingDate,
                    NewsId: id,
                    ImageId: imageId,
                    RelativeUrl: relUrl,
                    Tags: tags,
                    RecordType: recType,
                    Filename: filename,
                    VideoId: videoId,
                    Categories: cats,
                    EventDate: eventDate,
                    BlogId: blogId,

                    RelatedNewsItemId: relatedNewsItem
                }, function (result) {
                    onSuccess(result);
                }).fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                    .always(function () {
                        unblockUI();
                    });
            } else {
                $form.validate().focusInvalid();
            }
        },
        update: function () {
            var id = $(".news-items-info").data("newsId");
            var url = "/api/news/news/" + id + "/update";
            vm.save(url, $(".news-items-info"), function(res) {
                if (res.succeed) {
                    smartAlert("Новость обновлена", 3);
                } else {
                    smartAlert("Произошла ошибка");
                }

            });
            return false;
        },
        create: function () {
            var url = "/api/news/news/create";
            vm.save(url, $(".news-items-info"), function (res) {
                if (res.succeed) {
                    window.location.href = "/NewsEditor/EditNews/" + res.data;
                }
                else {
                    smartAlert("Произошла ошибка");
                }
            });
            return false;
        }
    };
    if (newsId) {
        getJSON("/api/news/news/" + newsId + "/get", null, function (res) {
            if (res.succeed) {
                $(res.data.categories).each(function () {
                    vm.categories.push(ko.mapping.fromJS(this));
                });
            }
        });
    }
    if (context) {
        ko.applyBindings(vm, context[0]);
    } else {
        ko.applyBindings(vm);
    }
}

function newsEditorLoaded(onSelected, context) {
    var newsId = $(".news-items-info").data("newsId");
    initNewsEditorKo(context, newsId);
    $('#adminContent').on("click", ".news-img", function () {
        pickImage(this, function () {
            $(".removeNewsImage").show();
        });
    });


    $(".news-title").change(function () {
        var val = $(this).val();

        if (val && !$('.news-relative-url').val()) {
            $('.news-relative-url').val(textToUrl(val));

        }
        $(".title-length").text(val.length);
    });

    $(".news-description").change(function () {
        var val = $(this).val();
        $(".description-length").text(val.length);
    });

    var saveNews = function () {

        
    };

    $('#adminContent').on("click", "#acceptNews", function () {
        
    });

    $('#adminContent').on("click", "#createNews", function () {
        
    });
    
    $('#adminContent').on("click", ".news-summary .button-delete", function () {
        if (!confirmRemoval()) {
            return false;
        }
        blockUI();
        var container = $(this).parents(".news-summary");
        var id = container.data("newsId");
        var url = "/api/news/news/" + id + "/delete";
        postJSON(url, "", function () {            
            container.hide(1000, function () { container.remove(); });
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
        return false;
    });
    
    $('#adminContent').on("click", ".admin-news-pages a", function () {
        
        var pageurl = $(this).attr('href');
        blockUI();
        $.ajax({
            url: pageurl,
            success: function(data) {
                $('#adminContent').html(data);
            }
        }).fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
            .always(function () {
                unblockUI();
            });
        //to change the browser URL to the given link location
        if (pageurl != window.location) {
            window.history.pushState({ path: pageurl }, '', pageurl);
        }
        /* the below code is to override back button to get the ajax content without page reload*/
        $(window).bind('popstate', function () {
            $.ajax({
                url: location.pathname, success: function (data) {
                    $('#adminContent').html(data);
                }
            });
        });
        //stop refreshing to the page given in
        return false;
    });
    
    $('#adminContent').on("change", ".admin-news #admin-news-on-page-select", function () {
        blockUI();
        var count = $(this).val();
        postJSON('/NewsEditor/SetNewsOnPageCount', { count: count }, function () {
            var url = '/NewsEditor/News';
            $.ajax({
                url: url,
                success: function(data) {
                    $('#adminContent').html(data);                   
                }
            });
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
        return false;
    });

    $("#newsTags").tagsInput({
        autocomplete_url: '/api/news/tag/pattern/get'
    });

    $.datepicker.setDefaults($.datepicker.regional["ru"]);

    $(".news-publish-date, .news-event-date").datepicker({
        dateFormat: "dd.mm.yy",
        changeMonth: true,
        numberOfMonths: 1

    });

    $(".news-event-date").datepicker({
        dateFormat: "dd.mm.yy",
        changeMonth: true,
        numberOfMonths: 1

    });

    $('.news-publish-time').timepicker({
        minuteStep: 5,
        disableFocus: true,
        showMeridian: false
    });

    $('.news-event-time').timepicker({
        disableFocus: true,
        showMeridian: false,
        defaultTime: ""
    });


    var emptyImg = "/Content/admin/ro/img/no-image.png";

    $(".removeNewsImage").click(function () {
        $(".news-img").attr("src", emptyImg);
        $(".news-img").removeAttr("data-id");
        $(this).hide();
    });

    $(".pick-file").click(function () {
        blockUI();
        showFilePickDialog(function (fileData) {
            $(".picked-file").html(fileData.name);
            $(".picked-file").data("filename", fileData.name);
            $(".picked-file-download").attr("href", "/File/Get/" + fileData.name);
            $(".picked-file-download").attr("download", fileData.name);
            $(".file-not-picked").hide();
            $(".file-picked").show();
        });
        unblockUI();
        return false;
    });

    $(".picked-file-remove").click(function () {
        $(".picked-file").html("");
        $(".picked-file").data("filename", "");
        $(".picked-file-download").attr("href", "/");
        $(".picked-file-download").attr("download", "");
        $(".file-picked").hide();
        $(".file-not-picked").show();
        return false;
    });

    $(".pick-video").click(function() {
        showPromptDialog("Добавление видео", "Скопируйте в поле ссылку на видео", "", "Добавить", "Отмена", function(data) {
            var id = parseYoutubeVideo(data.promptValue);
            if (id) {
                $(".video-picked .video-thumb").attr("src", "http://img.youtube.com/vi/" + id + "/default.jpg");
                $(".video-picked .video-thumb").data("videoId", id);
                $(".picked-video-watch").attr("href", "https://youtu.be/" + id);
                $(".video-not-picked").hide();
                $(".video-picked").show();

            } else {
                alert("Неверный формат ссылки");
            }

        }, null, null, null, 170);
    });

    $(".picked-video-remove").click(function() {

        $(".video-picked .video-thumb").attr("src", emptyImg);
        $(".video-picked .video-thumb").data("videoId", "");
        $(".picked-video-watch").attr("href", "https://youtu.be/");
        $(".video-picked").hide();
        $(".video-not-picked").show();
    });
};
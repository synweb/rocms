/// <reference path="/Content/admin/ro/js/rocms.heart.js" />
/// <reference path="/Content/admin/ro/js/rocms.video.album.edit.js" />



App.Admin.News.NewsItem = function () {
    var self = this;

    $.extend(self, new App.Admin.Heart());

    self.text = ko.observable().extend({ required: true });
    self.postingDate = ko.observable();
    self.eventDate = ko.observable();

    self.description = ko.observable().extend({ required: true });

    self.creationDate = ko.observable();
    self.authorId = ko.observable();
    self.imageId = ko.observable(null);
    self.commentTopicId = ko.observable();
    self.tags = ko.observable();
    self.recordType = ko.observable('Default');
    self.filename = ko.observable();
    self.videoId = ko.observable();
    self.categories = ko.observableArray();

    self.blogId = ko.observable();

}

App.Admin.News.NewsItemValidationMapping = {
    text: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    description: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    }
}

$.extend(App.Admin.News.NewsItemValidationMapping, App.Admin.HeartValidationMapping);

App.Admin.News.NewsItemFunctions = {
    initNewsItem: function () {
        var self = this;
        self.initHeart();

        if (self.text()) {
            setTextToEditor("news_content", self.text());
        }

        
        var date = self.postingDate() ? new Date(self.postingDate()) : new Date();

        $('.news-publish-date').datepicker("setDate", date);
        $('.news-publish-time').timepicker('setTime', date);

        if (self.eventDate()) {
            var eventDate = new Date(self.eventDate());

            $('.news-event-date').datepicker("setDate", eventDate);
            $('.news-event-time').timepicker('setTime', eventDate);
        }

        self.hasImage = ko.computed(function() {
            return self.imageId() !== null;
        });
    },

    prepareNewsItemForUpdate: function () {
        var self = this;
        self.prepareHeartForUpdate();

        var content = getTextFromEditor('news_content');
        self.text(content);


        var postingDate = $('.news-publish-date').datepicker("getDate");
        var eventDate = $('.news-event-date').datepicker("getDate");

        postingDate.setHours($('.news-publish-time').data('timepicker').hour);
        postingDate.setMinutes($('.news-publish-time').data('timepicker').minute);

        self.postingDate(postingDate);

        if (eventDate) {
            eventDate.setHours($('.news-event-time').data('timepicker').hour);
            eventDate.setMinutes($('.news-event-time').data('timepicker').minute);
        }

        self.eventDate(eventDate);

        var filename = $(".picked-file").data("filename");

        self.filename(filename);

        var videoId = $(".video-picked .video-thumb").data("videoId");

        self.videoId(videoId);

        var tags = $('.news-tags').val();

        self.tags(tags);

    },
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
    save: function (url, onSuccess) {
        var self = this;
        postJSON(url, ko.toJS(self), function (result) {
            if (result.succeed === true) {
                if (onSuccess) {
                    onSuccess(result);
                }
            }
        })
            .fail(function () {
                smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
            })
            .always(function () {
                unblockUI();
            });
    },



    update: function () {
        var self = this;
        var url = "/api/news/news/" + self.heartId() + "/update";
        self.save(url, function (res) {

            if (res.succeed) {
                smartAlert("Новость обновлена", 3);
            }

        });
        return false;
    },
    create: function () {
        var self = this;
        var url = "/api/news/news/create";
        self.save(url, function (res) {
            if (res.succeed) {
                blockUI();
                window.location.href = "/NewsEditor/EditNews/" + res.data;
            }
        });
        return false;
    },
    pickImage: function () {
        var self = this;
        showImagePickDialog(function (imageData) {
            self.imageId(imageData.ID);
        });
    },
    clearImage: function() {
        var self = this;
        self.imageId(null);
    }
}

$.extend(App.Admin.News.NewsItemFunctions, App.Admin.HeartFunctions);

function initNewsEditorKo(newsId, onSuccess) {

    var vm = {

        newsItem: ko.validatedObservable($.extend(new App.Admin.News.NewsItem(), App.Admin.News.NewsItemFunctions)),

        parents: ko.observableArray(),

        save: function () {

            vm.newsItem().prepareNewsItemForUpdate();
            if (vm.errors()().length) {
                vm.errors().showAllMessages();
                return false;
            }

            if (vm.newsItem().heartId()) {
                vm.newsItem().update();
            } else {
                vm.newsItem().create();
            }
            return false;
        }


    };

    vm.parents.push({ title: "Нет", heartId: null, type: "Выберите..." });

    vm.errors = ko.computed(function () {
        return ko.validation.group(vm.newsItem(), { deep: true });
    });


    blockUI();
    $.when(
            getJSON("/api/heart/hearts/get", "", function (res) {
                $(res).each(function () {
                    vm.parents.push(this);
                });
            }),
            getJSON("/api/news/news/" + newsId + "/get", "", function (res) {
                if (res.succeed && res.data) {
                    var newsItem = $.extend(ko.mapping.fromJS(res.data, App.Admin.News.NewsItemValidationMapping), App.Admin.News.NewsItemFunctions);
                    vm.newsItem(newsItem);

                    vm.newsItem().categories.removeAll();
                    $(res.data.categories).each(function () {

                        vm.newsItem().categories.push(ko.mapping.fromJS(this));
                    });
                }

                vm.newsItem().initNewsItem();
            })

    ).then(
        function () {
            vm.parents.remove(function (item) { return item.heartId === vm.newsItem().heartId() });

            ko.applyBindings(vm);

            $(".withsearch").selectpicker();

            if (onSuccess) {
                onSuccess();
            }
        },
        function () {
            smartAlert("Произошла ошибка");
        }
    ).always(function () {
        unblockUI();
    });
}

function newsEditorLoaded(newsId) {

    

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


    initNewsEditorKo(newsId, function () {
        $('#adminContent').on("click", ".news-img", function () {
            pickImage(this, function () {
                $(".removeNewsImage").show();
            });
        });


        $("#newsTags").tagsInput({
            autocomplete_url: '/api/news/tag/pattern/get'
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

        $(".pick-video").click(function () {
            showPromptDialog("Добавление видео", "Скопируйте в поле ссылку на видео", "", "Добавить", "Отмена", function (data) {
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

        $(".picked-video-remove").click(function () {

            $(".video-picked .video-thumb").attr("src", emptyImg);
            $(".video-picked .video-thumb").data("videoId", "");
            $(".picked-video-watch").attr("href", "https://youtu.be/");
            $(".video-picked").hide();
            $(".video-not-picked").show();
        });

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
            success: function (data) {
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
                success: function (data) {
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

};
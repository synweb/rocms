/// <reference path="admin-ajax.js" />

App.Admin.Slider = function (s) {
    var self = this;
    self.name = ko.observable();
    self.sliderId = ko.observable();

    self.init = function (slider) {
        self.name(slider.name);
        self.sliderId(slider.sliderId);
    };
    
    if (s) {
        self.init(s);
    }
    
    self.fetch = function() {
        if (self.sliderId()) {
            getJSON("/api/slider/" + self.sliderId() + "/get", {}, function (slider) {
                self.init(slider.data);
            });
        }
    };

    self.remove = function () {       
         if (self.sliderId()) {
             postJSON("/api/slider/" + self.sliderId() + "/delete", "", function () { });
         }       
    };

    self.edit = function (url) {
        url = url.replace("_id_", self.sliderId());
        window.location.href = url;  
    };

    self.create = function(name) {
        self.name(name);
        postJSON("/api/slider/create", ko.toJS(self), function (res) {
            self.sliderId(res.data);
        });
    };

};

App.Admin.Slide = function (slid) {
    var self = this;
    self.title = ko.observable();
    self.slideId = ko.observable();
    self.sliderId = ko.observable();
    self.description = ko.observable();
    self.imageId = ko.observable();
    self.link = ko.observable();
    
    self.fetch = function () {
        if (self.slideId()) {
            getJSON("/api/slide/" + self.slideId() + "/get", {}, function (slide) {
                self.init(slide.data);
            });
        }
    };

    self.init = function (s) {
        
        self.title(s.title);
        self.slideId(s.slideId);
        self.sliderId(s.sliderId);
        self.description(s.description);
        self.imageId(s.imageId);
        self.link(s.link);
    };

    self.edit = function () {
        showSlideCreateEditDialog("edit", self.slideId(), function () {
            self.fetch();
        });
    };

    self.remove = function (item, parent) {
        if (!confirmRemoval()) { return false; }
        if (self.slideId()) {
            postJSON("/api/slide/" + self.slideId() + "/delete", "", function(result) {
                if (result.succeed) {
                    parent.slides.remove(item);
                }
            });
        }
    };

    if (slid) {
        self.init(slid);
    }
};

function onSlidersLoad() {
    var vm = {
        sliders: ko.observableArray(),
        removeItem: function (item) {
            if (confirmRemoval()) {
                vm.sliders.pop(item);
                item.remove();
                return false;
            }
        },
        createItem: function () {
            showPromptDialog("Название слайдера", "Введите название слайдера", "", "Создать", "Отмена", function (data) {
                var name = data.promptValue;
                var slider = new App.Admin.Slider();
                slider.create(name);
                vm.sliders.push(slider);
            }, null, null, null, 200);
            return false;
        }
    };
    blockUI();
    getJSON("/api/slider/sliders/get", {}, function (result) {
        $.each(result.data, function (index, s) {
            vm.sliders.push(new App.Admin.Slider(s));
        });
        ko.applyBindings(vm);
    })
        .fail(function () {
        smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
    })
        .always(function () {
            unblockUI();
        });
}

function onSliderEditorLoad(slider) {

    $('html').on("click", ".slide-img", function () {
        pickImage(this);
    });

    var vm = {
        sliderId: slider.sliderId,
        sliderName: slider.name,
        slides: ko.observableArray(),
        createItem: function () {
            blockUI();
            showSlideCreateEditDialog("create", vm.sliderId, function (s) {
                var slide = new App.Admin.Slide(s);
                slide.fetch();
                vm.slides.push(slide);
            });
            return false;
        },
        editItem: function () {
            blockUI();
            var slideId = $('.slider-slides').val();
            var slide = ko.utils.arrayFirst(vm.slides(), function (item) {
                return slideId == item.slideId();
            });
            slide.edit();

        },
        removeItem: function () {
            if (!confirmRemoval()) { return false; }
            var slideId = $('.slider-slides').val();
            var slide = ko.utils.arrayFirst(vm.slides(), function(item) {
                return slideId == item.slideId();
            });
            vm.slides.remove(slide);
            slide.remove();
        }
    };
    blockUI();
    getJSON("/api/slide/slides/" + vm.sliderId + "/get", {}, function (result) {
        $.each(result.data, function (index, s) {
            vm.slides.push(new App.Admin.Slide(s));
        });
        
        ko.applyBindings(vm);
        
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });    
}

//action: "create" или "edit". при "create" id указывает на слайдер, при "edit" - на слайд
function showSlideCreateEditDialog(action, id, onCreate) {
    var url, postUrl, title, buttonTitle;
    //если в url содержится "create"
    if (action == "create"){
        url = "/Admin/CreateSlide/" + id;
        postUrl = "/api/slide/create";
        title = 'Добавление слайда';
        buttonTitle = "Создать";
    } else {
        url = "/Admin/EditSlide/" + id;
        postUrl = "/api/slide/"+id+"/update";
        title = 'Редактирование слайда';
        buttonTitle = "Применить";
    }
    var options = {
        title: title,
        modal: true,
        resizable: false,
        width: 500,
        height: 550,
        draggable: false,
        buttons: [
			{
                open: function() {
                    unblockUI();
                },
                close: function () {
                    unblockUI();
                },
			    text: buttonTitle,
			    click: function () {
			        blockUI();
			        var $dialog = $(this).dialog("widget");
			        var that = this;
			        var title = $dialog.find(".slide-title").val();
			        var description = $dialog.find(".slide-description").val();
			        var link = $dialog.find(".slide-link").val();
			        var imageId = $dialog.find(".slide-img").data("id");
			        var sliderId = $dialog.find(".slide-creation").data("sliderId");
			        var slideId = $dialog.find(".slide-creation").data("slideId");
			        //TODO: валидация
			        var message = {
			            Title: title,
			            Description: description,
			            ImageId: imageId,
			            Link: link,
			            SliderId: sliderId,
			            SlideId: slideId
			        };
			        $.ajax({
			            url: postUrl,
			            type: 'POST',
			            contentType: 'application/json; charset=utf-8',
			            data: JSON.stringify(message)
			        }).done(function (res) {
			            $(that).dialog("close");
			            if (onCreate && res.succeed) {
			                message.slideId = res.data;
			                onCreate(message);
			            }
			        }).fail(function () {
			            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
			        })
                        .always(function () {
                            unblockUI();
                        });
			    }
			},
			{
			    text: "Отмена",
			    click: function () {
			        $(this).dialog("close");
			    }
			}
        ]
    };

    showDialogFromUrl(url, options);
}
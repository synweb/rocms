/// <reference path="/Content/base/ro/js/rocms.helpers.js" />

App.Admin.GoodsReview = function() {
    var self = this;

    self.goodsReviewId = ko.observable();
    self.author = ko.observable();
    self.heartId = ko.observable();
    self.creationDate = ko.observable();
    self.authorContact = ko.observable();
    self.text = ko.observable();
    self.rating = ko.observable();
    self.moderated = ko.observable();
    self.goodsItem = ko.observable();

};


App.Admin.GoodsReviewValidationMapping = {
    // customize the creation of the name property so that it provides validation
    author: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    authorContact: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    },
    text: {
        create: function (options) {
            return ko.observable(options.data).extend({ required: true });
        }
    }
};

App.Admin.GoodsReviewFunctions = {

    goodsUrl: function() {
        return '/Shop/Goods/' + this.heartId();
    },

    containerId: function () {
        return 'goods-review-container-' + this.goodsReviewId();
    },
    
    init : function (d) {
        var self = this;
        self.goodsReviewId(d.goodsReviewId);
        self.author(d.author);
        self.heartId(d.heartId);
        self.creationDate(d.creationDate);
        self.authorContact(d.authorContact);
        self.text(d.text);
        self.rating(d.rating);
        self.moderated(d.moderated);
        self.goodsItem(d.goodsItem());
    },
    
    create : function (onCreate) {
        var self = this;

        self.dialog(function () {
            var url = "/api/shop/goods/reviews/create";
            self.save(url, function (result) {
                self.goodsReviewId(result.id);
                if (onCreate) {
                    onCreate();
                }
            });
        });
    },

    save : function (url, onSuccess) {
        blockUI();
        var self = this;

        postJSON(url, ko.toJS(self), function (result) {
            if (result.succeed) {
                if (onSuccess) {
                    onSuccess(result.data);
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

    edit: function () {
        var self = this;
        self.dialog(function () {
            self.save("/api/shop/goods/reviews/update");
        });
    },

    dialog: function(onSuccess) {    
        var self = ko.validatedObservable(this);
        var dialogContent = $("#reviewEditorTemplate").tmpl();
        var options = {
            title: "Отзыв о товаре",
            width: 650,
            height: 480,
            resizable: false,
            modal: true,
            open: function () {

                ko.applyBindings({ vm: self, ratingValues: ["", 1, 2, 3, 4, 5] }, this);
            },
            buttons: [
                {
                    text: "Сохранить",
                    click: function () {

                        if (self.isValid()) {
                            if (onSuccess) {
                                onSuccess();
                            }
                            $(this).dialog("close");
                        }
                        else {
                            self.errors.showAllMessages();
                        }
                    }
                },
                {
                    text: "Отмена",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ],
            close: function () {
                $(this).dialog('destroy');
                dialogContent.remove();
            }
        };
        dialogContent.dialog(options);
        return dialogContent;
    },

    remove : function (item, parent) {
        var self = this;
        var container = $('#'+self.containerId());
        container.hide(1000, function () { container.remove(); });
        var url = "/api/shop/goods/reviews/" + self.goodsReviewId() + "/delete";
        postJSON(url);
        
    },

    //Модерация

    accept: function () {
        alert(ko.toJS(this));
        var url = "/api/shop/goods/reviews/" + self.goodsReviewId() + "/accept";
        postJSON(url);
    },

    hide: function() {
        var url = "/api/shop/goods/reviews/" + self.goodsReviewId() + "/hide";
        postJSON(url);
    }
};

function goodsReviewsEditorLoaded() {
    blockUI();

    var vm = {
        goodsItem: ko.observable(),
        
        reviews: ko.observableArray(),
        pickGoods: function() {
            showGoodsDialog(function(goods) {
                loadReviews("/api/shop/goods/reviews/"+goods.id+"/get",vm);
            });
        },
        allGoods: function () {
            loadReviews("/api/shop/goods/reviews/get",vm);
        }
    };
    ko.applyBindings(vm);
    vm.allGoods();
    unblockUI();
}

function loadReviews(url,vm) {
    getJSON(url, null, function (result) {
        vm.reviews.removeAll();
        $(result).each(function () {
            vm.reviews.push($.extend(ko.mapping.fromJS(this, App.Admin.GoodsReviewValidationMapping), App.Admin.GoodsReviewFunctions));
        });
        renderSwitches();
    })
        .fail(function () {
            smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
        })
        .always(function () {
            unblockUI();
        });
}

function renderSwitches() {
        $('.moderated-switch').bootstrapSwitch();
        $('.moderated-switch').bootstrapSwitch('setOnLabel', '<i class="fa fa-eye"></i>');
        $('.moderated-switch').bootstrapSwitch('setOffLabel', '<i class="fa fa-eye-slash"></i>');
        $('.moderated-switch').bootstrapSwitch('setSizeClass', 'switch-small');
        

        $('.moderated-switch').on('switch-change', function (e, data) {
            var reviewId = $(this).parent().data("reviewId");
            var url;
            if (data.value === true) {
                url = "/api/shop/goods/reviews/"+reviewId+"/accept";
            } else {
                url = "/api/shop/goods/reviews/" + reviewId + "/hide";
            }
            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify({
                    reviewId: reviewId
                }),
                contentType: "application/json",
                success: function (data) {
                }
            });
            return false;
        });

}
﻿/// <reference path="../js/admin-ajax.js" />

App.Admin.MenuItem = function () {
    var self = this;
    self.name = ko.observable();
    self.items = ko.observableArray();
    self.menuItemId = ko.observable(-1);
    self.pageUrl = ko.observable();
    self.blockId = ko.observable();

    self.init = function (data) {
        self.name(data.name);
        self.pageUrl(data.pageUrl);
        self.blockId(data.blockId);
        self.menuId = data.menuId;
        self.menuItemId(data.menuItemId);
        $(data.items).each(function () {
            self.items.push(new App.Admin.MenuItem().init(this));
        });
        return self;
    };

    self.addItem = function () {
        var item = new App.Admin.MenuItem();
        item.menuId = self.menuId;
        item.parentMenuItemId = self.menuItemId;
        self.items.push(item);
    };

    self.moveUp = function (item, parent) {
        var index = parent.items.indexOf(item);
        if (index <= 0) return false;

        parent.items.remove(item);
        parent.items.splice(index - 1, 0, item);
    };
    
    self.moveDown = function (item, parent) {
        var index = parent.items.indexOf(item);
        if (index == parent.items.length - 1) return false;

        parent.items.remove(item);
        parent.items.splice(index + 1, 0, item);
    };

    self.remove = function (item, parent) {
        parent.items.remove(item);
    };

    self.pageUrl.subscribe(function (val) {
        if (val == null) {
            return;
        }
        if (!self.name()) {

            var vm = ko.contextFor($('.menu-info')[0]);

            var result = $.grep(vm.$root.pages(), function (e) { return e.relativeUrl == val; });
            if (result.length === 1) {
                self.name(result[0].title);
            }
            
            
        }
    });
};

App.Admin.Menu = function(id) {
    //Properties
    var self = this;
    self.name = ko.observable();
    self.menuId = ko.observable();
    self.items = ko.observableArray();

    //Methods
    self.init = function(id) {
        self.menuId(id);
        self.fetch();
    };

    self.addItem = function () {
        var item = new App.Admin.MenuItem();
        item.menuId = self.menuId;
        self.items.push(item);
    };

    self.fetch = function () {
        if (self.menuId()) {
            blockUI();
            getJSON("/api/menu/" + self.menuId() + "/get", "", function(result) {
                self.name(result.name);
                $(result.items).each(function() {
                    self.items.push(new App.Admin.MenuItem().init(this));
                });
            })
                .fail(function () {
                    smartAlert("Произошла ошибка. Если она будет повторяться - обратитесь к разработчикам.");
                })
                .always(function () {
                    unblockUI();
                });
        }
    };

    self.save = function () {
        var res;
        if (self.menuId()) {
            res = postJSON("/api/menu/update", ko.toJS(self));
        } else {
            res = postJSON("/api/menu/create", ko.toJS(self), function(result) { self.menuId = result; });
        }        
        return res;
    };

    self.remove = function () {
        if (self.menuId()) {
            postJSON("/api/menu/" + self.menuId() + "/delete");
        }
    };
   
    if (id) {
        self.init(id);
    }
    
};

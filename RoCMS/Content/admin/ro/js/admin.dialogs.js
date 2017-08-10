function showAlbumCreateDialog(onSubmit) {
    var options = {
        title: 'Добавление альбома',
        modal: true,
        resizable: false,
        width: 400,
        height: 200,
        draggable: false,
        buttons: [
			{
			    text: "Добавить",
			    click: function () {
			        var $dialog = $(this).dialog("widget");
			        var title = $dialog.find(".textbox-title").val();
			        if (onSubmit) {
			            onSubmit(title);
			            $(this).dialog("close");
			        };
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
    showDialogFromUrl("/Admin/CreateAlbum", options);
}


function InitImageUploader()
{
    $("html").on("click", ".image-picking-buttons .fileinput-button", function () {
        $('.image-picking-buttons #fileupload').click();
    });
}

function InitFileUploader() {
    $("html").on("click", ".file-picking-buttons .fileinput-button", function () {
        $('.file-picking-buttons #fileupload').click();
    });
}

function humanFileSize(kbytes, si) {
    var thresh = 1024;
    if (Math.abs(kbytes) < thresh) {
        return kbytes + ' KB';
    }
    var units = ['MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    var u = -1;
    do {
        kbytes /= thresh;
        ++u;
    } while (Math.abs(kbytes) >= thresh && u < units.length - 1);
    return kbytes.toFixed(1) + ' ' + units[u];
}

//Выбор изображения; onSelect: Url, ID
// options: {restrictUpload (bool) }
function showImagePickDialog(onSelect, options) {
    var hideCurrentAlbum = function () {
        // если мы сейчас в редакторе альбома, то его не надо показывать в диалоге
        var editor = $(".album-editor");
        if (editor && editor.data("albumId")) {
            var albumId = editor.data("albumId");
            console.log(albumId);
            $("tr.album-pick[data-id=" + albumId + "]").remove();
        }
    }
    blockUI();
    var dialogOptions = {
        title: 'Выбор изображения',
        modal: true,
        resizable: false,
        width: 800,
        height: 600,
        draggable: true,
        open: function () {
            hideCurrentAlbum();
            unblockUI();
            var $dialog = $(this).dialog("widget");
            var self = this;
            $(".image-picking-container").on("click", ".album-pick", function() {
                var id = $(this).data("id");
                var url = "/Gallery/PickImageFromAlbum/" + id;
                blockUI();
                $(".image-picking-container").load(url, function() {
                    if (options && options.restrictUpload) {
                        $('.image-picking-buttons .fileinput-button').hide();
                    } else {
                        $('.image-picking-buttons #fileupload').fileupload({
                            dataType: 'json',
                            xhrFields: { withCredentials: true },
                            forceIframeTransport: true,
                            done: function(e, data) {
                                $(data.result.files).each(function() {
                                    var imageId = this.name;
                                    var title = this.title;
                                    var size = humanFileSize(this.size);
                                    var elem = $("#thumb-pick-template").tmpl({ imageId: imageId, title: title, size: size });
                                    $(".image-pick-table").prepend(elem);
                                    var albumId = $(".image-pick-table").data("albumId");
                                    var url2 = "/api/album/" + albumId + "/" + this.name + "/add";
                                    postJSON(url2);
                                    elem.click();
                                });
                            }
                        });
                        $('#fileupload').fileupload(
                            'option',
                            'redirect',
                            'http://' + location.host + '/Content/admin/vendor/FU/cors/result.html?%s');
                    }
                    unblockUI();
                });
            });
            $(".image-picking-container").on("click", ".to-albums", function () {
                var url = "/Gallery/PickImageDialog";
                blockUI();
                $(".image-picking-container").load(url, function() {
                    hideCurrentAlbum();
                    unblockUI();
                });
            });
            $(".image-picking-container").on("click", ".image-pick-table tbody tr", function () {
                var img = $(this).find(".picking-image");
                var id = img.data("id");
                var src = IMAGE_THUMBNAIL_URL + id;
                var data = {
                    Url: src,
                    ID: id
                };
                if (onSelect) {
                    onSelect(data);
                }
                $(self).dialog("close");
            });
        }
    }
    showDialogFromUrl("/Gallery/PickImageDialog", dialogOptions);
}

//Возвращаем id
function showBlockPickDialog(onSelect) {
    var options = {
        title: 'Выбор блока',
        modal: true,
        resizable: false,
        width: 500,
        height: 400,
        draggable: true,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var self = this;
            $(".picking-block", $dialog).click(function () {
                var id = $(this).data("id");
                var data = {
                    ID: id
                };
                if (onSelect) {
                    onSelect(data);
                }
                $(self).dialog("close");
                return false;
            });
        }
    };
    showDialogFromUrl("/Admin/PickBlock", options);
}

//Возвращаем id
function showAlbumPickDialog(onSelect) {
    var options = {
        title: 'Выбор альбома',
        modal: true,
        resizable: false,
        width: 500,
        height: 400,
        draggable: true,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var self = this;
            $(".picking-album", $dialog).click(function () {
                var id = $(this).data("id");
                var data = {
                    ID: id
                };
                if (onSelect) {
                    onSelect(data);
                }
                $(self).dialog("close");
                return false;
            });
        }
    };
    showDialogFromUrl("/Admin/PickAlbum", options);
}

//Возвращаем url: ссылку на файл, name: имя файла
function showFilePickDialog(onSelect) {
    var options = {
        title: 'Выбор файла',
        modal: true,
        resizable: false,
        width: 800,
        height: 600,
        draggable: true,
        open: function () {

                $('.picking-buttons #fileupload').fileupload({
                    dataType: 'json',
                    xhrFields: { withCredentials: true },
                    forceIframeTransport: true,
                    done: function (e, data) {
                        $(data.result.files).each(function () {
                            var obj = $("<li class='file-pick-element'  data-filename='" + this.name + "' ><a class='picking-file row' href='/File/Get/"
                                + this.name + "' title='Откройте ссылку в новом окне для скачивания файла'>"
                                + '<div class="col-xs-7"><i class="fa fa-file"></i>&nbsp;' + this.name + '</div>'
                                + '<div class="col-xs-5">Загружено только что</div>'
                                + "</a></li>");

                            $(".file-pick-list")
                                .prepend(obj);

                            obj.find(".picking-file").click();
                        });

                    }
                });
                $('#fileupload').fileupload(
                    'option',
                    'redirect',
                    'http://' + location.host + '/Content/admin/vendor/FU/cors/result.html?%s');
                unblockUI();

            var $dialog = $(this).dialog("widget");
            var self = this;
            $dialog.on("click", ".picking-file", function () {
                var url = $(this).attr("href");
                var name = $(this).closest("li").data("filename");
                var data = {
                    url: url,
                    name: name
                };
                if (onSelect) {
                    onSelect(data);
                }
                $(self).dialog("close");
                return false;
            });
        }
    };
    showDialogFromUrl("/Admin/PickFile", options);
}

function showInterfaceStringPickDialog(onSelect) {
    var options = {
        title: 'Выбор строки',
        modal: true,
        resizable: false,
        width: 500,
        height: 400,
        draggable: true,
        open: function () {
            var $dialog = $(this).dialog("widget");
            var self = this;
            $(".istr-pick", $dialog).click(function () {
                var id = $(this).data("id");
                if (onSelect) {
                    onSelect(id);
                }
                $(self).dialog("close");
                return false;
            });
        }
    };
    showDialogFromUrl("/Developer/PickInterfaceString", options);
}

function showInterfaceStringCreateDialog(onSubmit) {
    var options = {
        title: 'Добавление строки',
        modal: true,
        resizable: false,
        width: 400,
        height: 200,
        draggable: false,
        buttons: [
			{
			    text: "Добавить",
			    click: function () {
			        var $dialog = $(this).dialog("widget");
			        var key = $dialog.find(".textbox-key").val();
			        var value = $dialog.find(".textbox-value").val();
			        if (onSubmit) {
			            onSubmit({ key, value });
			            $(this).dialog("close");
			        };
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
    showDialogFromUrl("/Developer/CreateInterfaceString", options);
}

$(document).on("click", ".istr-create-indialog", function() {
    showInterfaceStringCreateDialog(function (data) {
        //TODO: валидация
        postJSON('/api/interface/strings/create', data, function(res) {
            if (res.succeed) {
                var elem = $("#istr-pick-template").tmpl({ key: data.key, value: data.value });
                $(".interface-string-pick-table").prepend(elem);
            } else {
                if (res.message === "Exists") {
                    smartAlert("Строка с этим ID существует");
                } else {
                    smartAlert("Произошла ошибка");
                }
            }
        });
    });
});

$(document).on("click", ".album-create-indialog", function() {
    showAlbumCreateDialog(function(title) {
        postJSON('/api/album/create', title, function(res) {
            if (res.succeed) {
                var albumId = res.data;
                var elem = $("#album-pick-template").tmpl({ name: title, albumId });
                $(".album-pick-table").prepend(elem);
            }
        });
    });
});

function confirmRemoval() {
    return confirm('Вы уверены, что хотите удалить элемент?');
}

function showAddUserDialog() {
    var options = {
        title: 'Добавление пользователя',
        modal: true,
        resizable: false,
        width: 400,
        height: 400,
        draggable: false,
        buttons: [
            {
                text: "Добавить",
                click: function () {
                    var $dialog = $(this).dialog("widget");
                    var that = this;
                    var username = $dialog.find(".textbox-username").val();
                    //TODO: валидация
                    var message = {
                        Username: username,
                        Password: $dialog.find(".textbox-password").val(),
                        PasswordRepeat: $dialog.find(".textbox-password-repeat").val()
                    };
                    if (message.Password != message.PasswordRepeat) {
                        $(that).dialog("close");
                        return;
                    }
                    $.ajax({
                        url: "/Home/Register",
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(message)
                    }).done(function () {
                        $(that).dialog("close");
                        //$('.usernames').append($('<option>').text(username));
                        //TODO: избавиться
                        location.reload();
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
    showDialogFromUrl("/Admin/Register", options);
}
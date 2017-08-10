/// <reference path="../../../base/vendor/jquery/core/jquery-2.0.2.js" />
/// <reference path="../../../base/vendor/ckeditor/config.js" />
/// <reference path="../../vendor/ace/ro.ace.js" />
//Вставка после курсора
jQuery.fn.extend({
    insertAtCaret: function (myValue) {
        return this.each(function (i) {
            if (document.selection) {
                //For browsers like Internet Explorer
                this.focus();
                var sel = document.selection.createRange();
                sel.text = myValue;
                this.focus();
            } else if (this.selectionStart || this.selectionStart == '0') {
                //For browsers like Firefox and Webkit based
                var startPos = this.selectionStart;
                var endPos = this.selectionEnd;
                var scrollTop = this.scrollTop;
                this.value = this.value.substring(0, startPos) + myValue + this.value.substring(endPos, this.value.length);
                this.focus();
                this.selectionStart = startPos + myValue.length;
                this.selectionEnd = startPos + myValue.length;

                this.scrollTop = scrollTop;
            } else {
                this.value += myValue;
                this.focus();
            }
        });
    }
});

function insertToEditor(id, text, isHtml) {
    if (typeof (CKEDITOR) != "undefined") {
        var e = CKEDITOR.instances[id];
        if (e) {
            if (isHtml) {
                var el = CKEDITOR.dom.element.createFromHtml(text);
                CKEDITOR.instances[id].insertElement(el);
            } else {
                e.insertText(text);
            }

            return true;
        }
    }
    if (typeof (ace) != "undefined") {
        if ($("#" + id).data("ace")) {
            var aceEditor = ace.edit("ace_"+id);
            aceEditor.insert(text);
            return true;
        }
    }
    $("#"+id).insertAtCaret(text);
    return true;
}

function getEditorId(baseElement) {
    return $(baseElement).closest(".wysiwyg-container").data("editorId");
}

$(function () {

    $("#adminContent").on("click", ".wysiwyg-insert-image", function () {
        InsertImage(getEditorId(this), false);
    });

    $("#adminContent").on("click", ".wysiwyg-insert-thumbnail", function() {
        InsertImage(getEditorId(this), true);
    });

    $("#adminContent").on("click", ".wysiwyg-insert-block", function() {
        var editorId = getEditorId(this);
        showBlockPickDialog(function(res) {
            var content = '[[BLOCK(' + res.ID + ')]]';
            insertToEditor(editorId, content);
        });
    });
    
    $("#adminContent").on("click", ".wysiwyg-insert-album", function () {
        var editorId = getEditorId(this);
        showAlbumPickDialog(function (res) {
            var content = '[[ALBUM(' + res.ID + ')]]';
            insertToEditor(editorId, content);
        });
    });

    $("#adminContent").on("click", ".wysiwyg-insert-file", function() {
        var editorId = getEditorId(this);
        showFilePickDialog(function(fileData) {
            var content = '<a href="' + fileData.url + '">' + fileData.name + '</a>';
            insertToEditor(editorId, content, true);
        });
    });

    $("#adminContent").on("click", ".wysiwyg-insert-istr", function () {
        var editorId = getEditorId(this);
        showInterfaceStringPickDialog(function (key) {
            var content = '[[ISTR(' + key + ')]]';
            insertToEditor(editorId, content);
        });
    });
});


function InsertImage(editorId, thumbnail) {
    var content;
    showImagePickDialog(function (imageData) {
        if (thumbnail) {
            content = '<a class="image-box" data-gallery="gallery-' + imageData.ID + '" href="/Gallery/Image/' + imageData.ID + '"><img alt="" class="img-responsive" src="/Gallery/Thumbnail/' + imageData.ID + '"/></a>';
        } else {
            content = '<img alt="" class="img-responsive" src="/Gallery/Image/' + imageData.ID + '"/>';
        }
        insertToEditor(editorId, content, true);
    });
}

function createACEEditor(id, mode) {
    if (typeof(CKEDITOR)!== "undefined") {
        var e = CKEDITOR.instances[id];
        if (e) {
            e.destroy();
        }
    }
    var $obj = $("#" + id);
    if ($obj.data("ace")) {
        return false;
    }
    var editDiv = $('<pre>', {
        position: 'absolute',
        width: $obj.width(),
        height: $obj.height(),
        'class': $obj.attr('class'),
        'id': "ace_"+id
    }).insertBefore($obj);
    $obj.css('display', 'none');


    
    var editor = ace.edit(editDiv[0]);
    editor.setOption("showPrintMargin", false);
    editor.getSession().setValue($obj.val());
    editor.getSession().setUseWrapMode(true);
    editor.setTheme("ace/theme/sqlserver");
    editor.$blockScrolling = Infinity;

    if (mode) {
        setACEMode(id, mode);
    } else {
        setACEMode(id, "text");
    }
    $obj.data("ace", 'true');
}

function setACEMode(id, mode) {
    var editor = ace.edit("ace_"+ id);
    switch (mode) {
    case "javascript":
    case "text":
    case "razor":
    case "plain_text":
    case "markdown":
    case "json":
    case "xml":
    case "html":
    case "css":
    case "csharp":
        editor.session.setMode("ace/mode/" + mode);
        break;
    default:
        editor.session.setMode("ace/mode/text");
        break;
    }
}

function createCKEditor(id) {
    if ($("#"+id).data("ace")) {
        ace.edit("ace_" + id).destroy();
        $("#ace_" + id).remove();
        $("#" + id).data("ace", false);
    }

    var $obj = $("#" + id);
    var height = $obj.height();

    CKEDITOR.tools.enableHtml5Elements(document);
    CKEDITOR.config.allowedContent = true;
    CKEDITOR.config.contentsCss = [
        "/Content/theme/vendor/bootstrap/css/bootstrap.css",
        "/Content/theme/css/style.css",
        "/Content/theme/css/theme.css",
    ];
    if (id.indexOf("comment") != -1) {
        CKEDITOR.replace(id, {
            toolbar: [
                { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', '-', 'RemoveFormat'] },
                { name: 'lists', items: ['NumberedList', 'BulletedList'] },
            ],
            height: height
        });
        CKEDITOR.config.removePlugins = 'elementspath';
    }
    else if (id.indexOf("question") != -1) {
        CKEDITOR.replace(id, {
                toolbar: [
                    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', '-', 'RemoveFormat'] },
                    { name: 'lists', items: ['NumberedList', 'BulletedList'] },
                    { name: 'links', items: ['Link', 'Unlink'] },
                ],
                height: height
            });
            CKEDITOR.config.removePlugins = 'elementspath';
        } else {
        CKEDITOR.replace(id, {
            toolbar: [
                { name: 'clipboard', groups: ['clipboard', 'undo'], items: ['Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
                { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
                {
                    name: 'paragraph',
                    groups: ['list', 'indent', 'blocks', 'align', 'bidi'],
                    items: ['NumberedList', 'BulletedList', 'Table', '-', 'Outdent', 'Indent', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock']
                },
                { name: 'links', items: ['Link', 'Unlink'] },
                { name: 'insert', items: ['Image'] },
                { name: 'styles', items: ['Format', 'TextColor', 'FontSize'] }
            ],
            height: height
        });
    }

}

//// Toolbar configuration generated automatically by the editor based on config.toolbarGroups.
//config.toolbar = [
//	{ name: 'document', groups: ['mode', 'document', 'doctools'], items: ['Source', '-', 'Save', 'NewPage', 'Preview', 'Print', '-', 'Templates'] },
//	{ name: 'clipboard', groups: ['clipboard', 'undo'], items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
//	{ name: 'editing', groups: ['find', 'selection', 'spellchecker'], items: ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt'] },
//	{ name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'] },
//	'/',
//	{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
//	{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl', 'Language'] },
//	{ name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
//	{ name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'] },
//	'/',
//	{ name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
//	{ name: 'colors', items: ['TextColor', 'BGColor'] },
//	{ name: 'tools', items: ['Maximize', 'ShowBlocks'] },
//	{ name: 'others', items: ['-'] },
//	{ name: 'about', items: ['About'] }
//];

//// Toolbar groups configuration.
//config.toolbarGroups = [
//	{ name: 'document', groups: ['mode', 'document', 'doctools'] },
//	{ name: 'clipboard', groups: ['clipboard', 'undo'] },
//	{ name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
//	{ name: 'forms' },
//	'/',
//	{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
//	{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
//	{ name: 'links' },
//	{ name: 'insert' },
//	'/',
//	{ name: 'styles' },
//	{ name: 'colors' },
//	{ name: 'tools' },
//	{ name: 'others' },
//	{ name: 'about' }
//];
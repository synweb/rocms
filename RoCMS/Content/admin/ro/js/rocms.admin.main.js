function pickImage(sender, onSelect) {
	var self = $(sender);
	showImagePickDialog(function (imageData) {
		self.attr("data-id", imageData.ID);
		self.attr("src", imageData.Url);
		if (onSelect) {
		    onSelect(imageData);
		}
	});
}


function getTextFromEditor(id) {

    if (typeof (CKEDITOR) != "undefined") {
        var e = CKEDITOR.instances[id];
        if (e) {
            return e.getData();
        }
    }
    
    if (typeof (ace) != "undefined") {
        if ($("#" + id).data("ace")) {
            var aceEditor = ace.edit("ace_"+id);
            var content = aceEditor.getValue();
            return content;
        }
    }
	return $("#"+id).val();
}

function setTextToEditor(id, text) {
    if (typeof (CKEDITOR) != "undefined") {
        var e = CKEDITOR.instances[id];
        if (e) {
            e.setData(text);
            return false;
        }
    }

    if (typeof (ace) != "undefined") {
        if ($("#" + id).data("ace")) {
            var aceEditor = ace.edit("ace_" + id);
            aceEditor.setValue(text);
            return false;
        }
    }
    $("#" + id).val(text);
    return false;
}

$.j = function (item) {
    alert(JSON.stringify(item));
}

$.kj = function (item) {
    alert(JSON.stringify(ko.toJS(item)));
}

function refresh() {
    location.reload(false);
}
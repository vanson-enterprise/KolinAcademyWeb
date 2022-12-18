
const roxyFileBrowser = function (callback, value, meta) {
    var roxyFileman = '/assets/plugins/custom/Roxy_Fileman/index.html';
    if (roxyFileman.indexOf("?") < 0) {
        roxyFileman += "?type=" + meta.filetype;
    }
    else {
        roxyFileman += "&type=" + meta.filetype;
    }
    //roxyFileman += '&input=' + fieldName + '&value=' + value;
    roxyFileman += '&value=' + value;

    if (tinyMCE.activeEditor.settings.language) {
        roxyFileman += '&langCode=' + tinyMCE.activeEditor.settings.language;
    }

    tinyMCE.activeEditor.windowManager.openUrl({
        title: 'Roxy Fileman',
        url: roxyFileman,
        width: 850,
        height: 650,
        onMessage: function (dialogApi, details) {
            callback(details.content);
            dialogApi.close();
        }
    });

    return false;
};

window.initTinymce = function (selector) {
    tinymce.baseURL = "https://kolin.vn/assets/plugins/custom/tinymce";
    tinymce.documentBaseURL = "https://kolin.vn";
    tinymce.baseURI.source = "https://kolin.vn/assets/plugins/custom/tinymce"
    tinymce.baseURI.relative = "/assets/plugins/custom/tinymce";
    tinymce.baseURI.path = "/assets/plugins/custom/tinymce";
    tinymce.baseURI.directory = "/assets/plugins/custom/tinymce";


    tinymce.init({
        selector: selector,
        height: 1000,
        width: '100%',
        plugins: [
            'advlist autolink lists link image charmap print preview hr anchor pagebreak',
            'searchreplace wordcount visualblocks visualchars code fullscreen',
            'insertdatetime media nonbreaking save table contextmenu directionality',
            'emoticons template paste textcolor colorpicker textpattern imagetools codesample toc',
            'formula'
        ],
        toolbar: 'fullscreen',
        toolbar1:
            'undo redo | insert | fontsizeselect | fontselect | styleselect | bold italic | alignleft aligncenter alignright alignjustify | superscript subscript |bullist numlist outdent indent | link image ',
        toolbar2: 'print preview media | forecolor backcolor emoticons | codesample | formula',
        image_advtab: true,
        //"relative_urls" required by jbimages plugin to be set to "false"
        relative_urls: false,
        convert_urls: false,
        custom_elements: 'ins',
        extended_valid_elements: 'ins[*],iframe[*]',
        style_formats_merge: true,
        paste_data_images: true,
        fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt",
        style_formats: [
            { title: 'High light', inline: 'span', classes: 'news-high-light-text' },
            { title: 'Table', classes: 'news-table', selector: 'table' },
            { title: 'Link button', selector: 'a', classes: 'news-link-button' },
        ],
        content_css: [
            'https://fonts.googleapis.com/css?family=Roboto'
        ],
        file_picker_callback: roxyFileBrowser,
        setup: function (editor) {
            editor.ui.registry.addContextToolbar('imgformula', {
                predicate: function (node) {
                    return node.nodeName.toLowerCase() === 'img' && $(node).prop("class") === "fm-editor-equation";
                },
                items: 'formula',
                position: 'node',
                scope: 'node'
            });
        }
    })
}
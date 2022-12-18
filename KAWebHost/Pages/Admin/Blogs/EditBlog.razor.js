const editBlogPageJs = function () {
    this.init = () => {
        debugger
        window.initTinymce('#app-text-editor');
    }

    this.getTextEditorContent = () => {
        return tinymce.activeEditor.getContent();
    }
}

window.editBlogPageJs = new editBlogPageJs();
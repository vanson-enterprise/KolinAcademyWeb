const createBlogPageJs = function () {
    this.init = () => {

        window.initTinymce('#app-text-editor');
    }
}

window.createBlogPageJs = new createBlogPageJs();
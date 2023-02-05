const detailOfflineCoursePageJs = function () {
    this.init = () => {
        $(".register_btn").on("click", function () {
            $("#show_registor_modal_btn").trigger("click");
        })
    }
}

window.detailOfflineCoursePageJs = new detailOfflineCoursePageJs();
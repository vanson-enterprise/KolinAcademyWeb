const orderPageJs = function () {
    this.init = () => {
        $.getScript("/assets/plugins/custom/datatables/datatables.bundle.js", function (data, textStatus, jqxhr) {
            if (jqxhr.status == 200) {
                const dataTable = $("#orders_table").DataTable({
                    scrollX: true,
                });
            }
        });
        //https://www.syncfusion.com/faq/blazor/general/how-do-i-use-different-css-files-in-blazor-pages
    }
}

window.orderPageJs = new orderPageJs();
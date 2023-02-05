const registerIndexPageJs = function () {
    this.init = () => {
        $("#kt_datepicker_1").flatpickr({
            dateFormat: "d-m-Y",
        });
        $("#kt_datepicker_2").flatpickr({
            dateFormat: "d-m-Y",
        });
    }
}

window.registerIndexPageJs = new registerIndexPageJs();
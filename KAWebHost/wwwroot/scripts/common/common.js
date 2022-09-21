window.ShowAppAlert = function (message, mode) {
    if (mode == "success") {
        toastr.success(message)
    } else if (mode == "warn") {
        toastr.warning(message)
    } else if (mode == "error") {
        toastr.error(message)
    } else {
        toastr.info(message);
    }
}


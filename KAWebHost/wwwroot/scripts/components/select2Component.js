window.select2Component = {
    init: (id, dotnetRef, funcName, hideSearch, isFromModal) => {
        debugger
        $("#" + id)
            .select2({
                minimumResultsForSearch: hideSearch == true ? - 1 : 1,
                dropdownParent: isFromModal ? $("#CreateAddressModal .modal-content") : null
            })
            .on("select2:select", function (e) {
                var selectedValue = $("#" + id).val();
                dotnetRef.invokeMethodAsync(funcName, selectedValue);
            });
    }
}
const visaPaymentPageJs = function () {

    this.submit = () => {
        debugger
        $("#submit_payment_form").trigger("click");
    }
}

window.visaPaymentPageJs = new visaPaymentPageJs();
const cartPageJs = function () {
    this.getTempCart = function () {
        return JSON.parse(localStorage.getItem("temp-cart"));
    }
    this.removeTempCart = function () {
        localStorage.removeItem("temp-cart");
    }
}

window.cartPageJs = new cartPageJs();
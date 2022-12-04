const cartPageJs = function () {
    this.getTempCart = function () {
        return JSON.parse(localStorage.getItem("temp-cart"));
    }
    this.removeCourseFromCart = function (courseId) {
        var tempCart = JSON.parse(localStorage.getItem("temp-cart"));
        if (Array.isArray(tempCart)) {
            var newTempCart = tempCart.filter(i => i != courseId);
            localStorage.setItem("temp-cart", JSON.stringify(newTempCart));
        }
    }
}

window.cartPageJs = new cartPageJs();
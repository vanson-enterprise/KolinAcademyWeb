const mainLayoutPageJs = function () {
    this.addCourseToTempCart = function (courseId) {
        var tempCart = JSON.parse(localStorage.getItem("temp-cart"));
        if (tempCart == null) {
            tempCart = [courseId]
        } else if (Array.isArray(tempCart) && tempCart.some(item => item == courseId)) {
            return;
        } else if (Array.isArray(tempCart)) {
            tempCart.push(courseId);
        }
        localStorage.setItem("temp-cart", JSON.stringify(tempCart));
    }

    this.getTempCart = function () {
        var tempCart = JSON.parse(localStorage.getItem("temp-cart"));
        if (Array.isArray(tempCart)) {
            return tempCart;
        }
        return [];
    }

    this.removeTempCart = function () {
        localStorage.removeItem("temp-cart");
    }

    this.countCartProductAmount = function () {
        var tempCart = JSON.parse(localStorage.getItem("temp-cart"));
        if (tempCart == null) {
            return 0;
        } else if (Array.isArray(tempCart)) {
            return tempCart.length;
        } else {
            return 0;
        }
    }

    this.removeTempCart = function () {
        localStorage.removeItem("temp-cart");
    }
}

window.mainLayoutPageJs = new mainLayoutPageJs();
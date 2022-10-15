// tabs
$(document).ready(function () {
    $(".btn-container .tab-btn").click(function () {
        $(this).addClass("active").siblings().removeClass("active");
        $(".about-content > .content").hide();
        $($(this).data("value")).fadeIn();
    });
});

// toggle
const navToggle = document.querySelector(".nav-toggle");
const links = document.querySelector(".links");

navToggle.addEventListener("click", function () {
    links.classList.toggle("show-links");
});
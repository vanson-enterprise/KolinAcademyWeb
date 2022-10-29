// toggle menu
const navToggle = document.querySelector(".nav-toggle");
const links = document.querySelector(".links");
const namePage = document.querySelector(".name-page");

navToggle.addEventListener("click", function () {
    links.classList.toggle("show-links");
});

links.addEventListener("click", (e) => {
    namePage.innerHTML = e.target.innerHTML;
});
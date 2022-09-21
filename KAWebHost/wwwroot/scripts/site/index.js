// sticky header

var navbar = document.getElementById("desktop-nav");
var sticky = navbar.offsetTop;

function myFunction() {
    if (window.pageYOffset >= sticky) {
        navbar.classList.add("sticky");
    } else {
        navbar.classList.remove("sticky");
    }
}
window.onscroll = function () { myFunction() };

// slide testimonials-area 
$('.owl-carousel').owlCarousel({
    loop: true,
    margin: 10,
    nav: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 1
        },
        1000: {
            items: 1
        }
    }
})


// tabs item
const tabList = document.querySelector(".choose-us-area_panel");
const tabs = document.querySelectorAll(".tabs-panel");
const articles = document.querySelectorAll(".tab_item");

tabList.addEventListener('click', function (e) {
    const id = e.target.dataset.id;

    if (e.target.dataset) {
        tabs.forEach(function (tab) {
            tab.classList.remove('current');
            e.target.classList.add('current');
        });

        articles.forEach(function (article) {
            article.classList.remove('current');
        });

        const element = document.getElementById(id);
        element.classList.add('current');
    }
})

// open menu 
const btnOpenMenu = document.querySelector("#menu-icon");
const btnCloseMenu = document.querySelector("#meanclose");
const meanBarMenu = document.querySelector(".mean-bar");

btnOpenMenu.addEventListener('click', () => {
    btnOpenMenu.classList.toggle("active");
    btnCloseMenu.classList.toggle("active");
    meanBarMenu.classList.toggle("active");
})

btnCloseMenu.addEventListener('click', () => {
    btnOpenMenu.classList.toggle("active");
    btnCloseMenu.classList.toggle("active");
    meanBarMenu.classList.toggle("active");
})

const btnOpenDropMenu = document.querySelectorAll(".plus-icon");
const btnCloseDropMenu = document.querySelectorAll(".minus-icon");


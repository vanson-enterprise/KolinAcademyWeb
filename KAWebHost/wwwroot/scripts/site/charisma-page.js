const charismaPageJs = function () {
    this.init = () => {
        import('https://cdnjs.cloudflare.com/ajax/libs/OwlCarousel2/2.3.4/owl.carousel.min.js').then(() => {
            $(".owl-carousel.owl-carousel").owlCarousel({
                loop: true,
                margin: 10,
                nav: true,
                autoplay: true,
                autoplayTimeout: 4000,
                autoplayHoverPause: true,
                responsive: {
                    0: {
                        items: 1,
                    },
                    600: {
                        items: 1,
                    },
                    1000: {
                        items: 2,
                    },
                },
            });

            // toggle menu
            const navToggle = document.querySelector(".nav-toggle2");
            const links = document.querySelector(".links");
            const namePage = document.querySelector(".name-page");

            navToggle.addEventListener("click", function () {
                links.classList.toggle("show-links");
            });

            // change text
            links.addEventListener("click", (e) => {
                namePage.innerHTML = e.target.innerHTML;
            });


        })
    }
}

window.charismaPageJs = new charismaPageJs();
window.charismaPageJs.init();
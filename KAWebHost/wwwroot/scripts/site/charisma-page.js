const charismaPageJs = function () {
    this.init = () => {
        import('https://cdn.jsdelivr.net/npm/swiper@11/swiper-bundle.min.js').then(() => {
            var swiper = new Swiper(".swiperCaseStudy", {
                slidesPerView: 1,
                spaceBetween: 30,
                pagination: {
                  el: ".swiper-pagination",
                  clickable: true,
                },
                navigation: {
                  nextEl: ".swiper-button-next",
                  prevEl: ".swiper-button-prev",
                },
                breakpoints: {
                  640: {
                    slidesPerView: 2,
                  },
                },
              });
        })
    }
}

window.charismaPageJs = new charismaPageJs();
window.charismaPageJs.init();
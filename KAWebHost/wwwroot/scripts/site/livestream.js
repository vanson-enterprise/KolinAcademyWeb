const livePageJs = function () {
    this.init = () => {
        $(".slider-story").slick({
            infinite: true,
            slidesToShow: 3,
            slidesToScroll: 1,
            infinite: true,
            centerMode: true,
            centerPadding: "0",
            responsive: [
              {
                breakpoint: 768,
                settings: {
                  slidesToShow: 1,
                  slidesToScroll: 1,
                  centerMode: false,
                  arrows: true,
                },
              },
            ],
          });
    }
}

window.livePageJs = new livePageJs();
window.livePageJs.init();
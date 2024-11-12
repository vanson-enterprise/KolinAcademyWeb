function pageJs() {
  this.init = () => {
    var swiper = new Swiper(".swiperBanner", {
      cssMode: true,
      pagination: {
        el: ".swiper-pagination",
        clickable: true,
      },
      mousewheel: true,
    });

    var swiper = new Swiper(".swiperAboutUs", {
      slidesPerView: 1,
      spaceBetween: 30,
      pagination: {
        el: ".swiper-pagination",
        clickable: true,
      },
      breakpoints: {
        640: {
          slidesPerView: 3,
          spaceBetween: 15,
        },
      },
    });

    var swiper = new Swiper(".swiperCaseStudy", {
      slidesPerView: 1,
      slidesPerGroup: 1,
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
           slidesPerGroup: 3,
        },
      },
    });

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
  };
  this.showAlert = (message) =>{
    window.alert(message)
  }
}

export const indexPageModule = new pageJs();

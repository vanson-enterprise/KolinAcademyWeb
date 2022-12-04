function profilePageJs() {
    this.init = function () {
        var readURL = function (input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $(".profile-pic").attr("src", e.target.result);
                };

                reader.readAsDataURL(input.files[0]);
            }
        };

        $(".file-upload").on("change", function () {
            readURL(this);
        });

        $(".upload-button").on("click", function () {
            $(".file-upload").click();
        });

        $(".btn-container .tab-btn").click(function () {
            $(this).addClass("active").siblings().removeClass("active");
            $(".about-content > .content").hide();
            $($(this).data("value")).fadeIn();
        });
    }
}

window.profilePageJs = new profilePageJs();
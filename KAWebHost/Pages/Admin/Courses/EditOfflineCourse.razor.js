const editOffCoursePageJs = function () {
    let stepper = null;
    this.init = () => {
        if (stepper != null) {
            stepper = null;
        }

        let stepperElement = document.querySelector("#kt_stepper_example_basic");
        stepper = new KTStepper(stepperElement);
        // Handle next step
        stepper.on("kt.stepper.next", function () {
            let currentStepIndex = stepper.getCurrentStepIndex();
            if (currentStepIndex == 1) {
                $("#course_form_submit_btn").trigger("click");
            } else if (currentStepIndex == 2) {
                DotNet.invokeMethodAsync('KAWebHost', 'CheckCourseStartDateValid').then(data => {
                    if (data == "prevent") {
                        toastr.error("Bạn phải tạo ít nhất một ngày khai giảng!");
                    }
                    else {
                        stepper.goNext();
                    }
                });
            }
        });

        // Handle previous step
        stepper.on("kt.stepper.previous", function (stepper) {
            stepper.goPrevious();
        });

        $("#update_sd_btn").on("click", () => {
            $("#startdate_form_submit").trigger("click");
        })

        $("#add_sd_btn").on("click", () => {
            $("#startdate_form_submit").trigger("click");
        })
    }

    this.goNextStep = () => {
        stepper.goNext();
    }

    this.goFirstStep = () => {
        stepper.goFirst();
    }
}

window.editOffCoursePageJs = new editOffCoursePageJs();
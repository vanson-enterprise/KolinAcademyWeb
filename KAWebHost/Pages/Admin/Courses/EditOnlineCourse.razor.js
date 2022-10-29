const editOnCoursePageJs = function () {
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
                DotNet.invokeMethodAsync('KAWebHost', 'CheckCourseValid').then(data => {
                    if (data == "prevent") {
                        toastr.error("Bạn phải nhập ít nhất một bài giảng!");
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

        $("#update_lesson_btn").on("click", () => {
            $("#lesson_form_submit").trigger("click");
        })

        $("#add_lesson_btn").on("click", () => {
            $("#lesson_form_submit").trigger("click");
        })
    }

    this.goNextStep = () => {
        stepper.goNext();
    }

    this.goFirstStep = () => {
        stepper.goFirst();
    }
}

window.editOnCourseJsModule = new editOnCoursePageJs();
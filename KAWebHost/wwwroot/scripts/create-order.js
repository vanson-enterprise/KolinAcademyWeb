const pageJs = function () {

    let stepperElement = document.querySelector("#kt_stepper_example_basic");
    const stepper = new KTStepper(stepperElement);

    this.init = () => {

        
        this.initFlatPicker();
        // init event
        $("#sp_order_add_btn").on("click", () => {
            $("#sp_order_form_submit").trigger("click");

        });

        $("#sp_order_edit_btn").on("click", () => {
            $("#sp_order_form_submit").trigger("click");
        });


        // Handle next step
        stepper.on("kt.stepper.next", function () {
            let currentStepIndex = stepper.getCurrentStepIndex();
            if (currentStepIndex == 1) {
                $("#pickup_order_form_submit_btn").trigger("click");
            } else if (currentStepIndex == 2) {

                DotNet.invokeMethodAsync('DeliverySystemApp', 'CheckShippingOrdersValid').then(data => {
                    if (data == "editing") {
                        toastr.error("Bạn đang sửa một đơn gửi, hãy lưu hoặc hủy!");

                    } else if (data == "prevent") {
                        toastr.error("Bạn phải nhập ít nhất một đơn gửi!");
                    }
                    else {
                        stepper.goNext();
                    }
                });
            }
        });

        // Handle previous step
        stepper.on("kt.stepper.previous", function (stepper) {
            stepper.goPrevious(); // go previous step
        });
    };

    this.initFlatPicker = ()=>{
        // init date picker
        setTimeout(() => {
            $("#pk_order_datepkr").flatpickr({
                minDate: "today",
                dateFormat: "d-m-Y",
            });
        },1000)
       
    };

    this.goNextStep = () => {
        stepper.goNext(); // go next step
    }

    this.goFirstStep = () => {
        debugger
        stepper.goFirst();
    }
}

window.pageJs = new pageJs();
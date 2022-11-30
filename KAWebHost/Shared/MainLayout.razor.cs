using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KAWebHost.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        public string alertTitle { get; set; }
        public string alertMessage { get; set; }
        private bool isShowAlert { get; set; }
        private bool isShowForceAuthenAlert { get; set; }

        // service
        [Inject]
        private IJSRuntime jsr { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("import", "/scripts/common/common.js");
                await jsr.InvokeVoidAsync("import", "./Shared/MainLayout.razor.js");
            }
        }

        public void ShowAlert(string message, string title)
        {
            isShowAlert = true;
            alertTitle = title;
            alertMessage = message;
            StateHasChanged();
        }

        public void ShowForceAuthenAlert()
        {
            isShowForceAuthenAlert = true;
            alertTitle = "Thông báo";
            alertMessage = "Bạn vui lòng đăng nhập hoặc đăng kí tài khoản để tiến hành thanh toán!";
            StateHasChanged();
        }

        public void HideAlert()
        {
            isShowAlert = false;
            StateHasChanged();
        }

        public void HideAuthenAlert()
        {
            isShowForceAuthenAlert = false;
            StateHasChanged();
        }

        public void AddCourseToTempCart(int courseId)
        {
            jsr.InvokeVoidAsync("mainLayoutPageJs.addCourseToTempCart", courseId);
        }

    }
}
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KAWebHost.Pages.Site
{
    public partial class Profile : OwningComponentBase
    {
        [Inject]
        private IJSRuntime jsr { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await jsr.InvokeVoidAsync("import", "./Pages/Site/Profile.razor.js");
            }
        }
    }
}

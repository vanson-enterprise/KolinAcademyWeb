using KA.Infrastructure.Enums;
using KA.Service.Contacts;
using KA.Service.Courses;
using KA.ViewModels.Common;
using KA.ViewModels.Contact;
using KA.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace KAWebHost.Pages.Admin.Contacts
{
    public partial class Index : OwningComponentBase
    {
        string pagingSummaryFormat = "Trang {0} trên {1} (tổng {2} bản ghi)";

        [Inject]
        private IJSRuntime jsr { get; set; }

        private DataGridResponse<ContactViewModel> dataGrid;
        private IContactService _contactService;
        private int pageSize = 10;

        protected override async Task OnInitializedAsync()
        {
            _contactService = ScopedServices.GetRequiredService<IContactService>();
            await GetAllCourse();
        }

        private async Task GetAllCourse()
        {
            dataGrid = await _contactService.GetAllContactPaging(0, pageSize);
        }

        private async Task PageChanged(PagerEventArgs args)
        {
            dataGrid = await _contactService.GetAllContactPaging(args.Skip, args.Top);
        }

       
    }
}

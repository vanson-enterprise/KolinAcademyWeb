using KA.Service.Contacts;
using KA.Service.Courses;
using KA.ViewModels.Contact;
using KA.ViewModels.Courses;
using KAWebHost.Shared;
using Microsoft.AspNetCore.Components;

namespace KAWebHost.Pages.Site
{
    public partial class Courses : OwningComponentBase
    {
        private ContactInputModel contactModel = new();
        private List<OfflineCourseViewModel> offlineCourseModels;
        private List<OnlineCourseViewModel> onlineCourseViewModels;

        private IContactService _contactService;
        private ICourseService _courseService;

        [CascadingParameter]
        private MainLayout mainLayout { get; set; }


        protected override async Task OnInitializedAsync()
        {
            _contactService = ScopedServices.GetRequiredService<IContactService>();
            _courseService = ScopedServices.GetService<ICourseService>();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender){
                offlineCourseModels = await _courseService.GetTopOffCourseForIndexPage(1000);
                onlineCourseViewModels = _courseService.GetAllOnlineCourse();
                StateHasChanged();
            }
        }

        protected void SaveContactInfo()
        {
            _contactService.SaveContact(contactModel);
            mainLayout.ShowAlert("Cảm ơn bạn đã để lại thông tin! Chúng tôi sẽ liên hệ bạn sớm nhất có thể.", "Thông báo"); contactModel = new();
        }
    }
}
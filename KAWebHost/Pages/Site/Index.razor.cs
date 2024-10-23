using KA.Service.Blogs;
using KA.Service.Courses;
using KA.ViewModels.Blogs;
using KA.ViewModels.Courses;
using KA.ViewModels.Contact;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using KA.Service.Contacts;
using KAWebHost.Shared;
using System.Reflection;
using ClosedXML;

namespace KAWebHost.Pages.Site
{
    public partial class Index : OwningComponentBase
    {
        private ICourseService _courseService;
        private IContactService _contactService;
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [CascadingParameter]
        private MainLayout mainLayout { get; set; }
        

        //private List<OfflineCourseViewModel> offlineCourseViewModels;
        private List<OnlineCourseViewModel> topTwoOnlineCourses;
        private List<OfflineCourseViewModel> offlineCourses;
        private List<BlogViewModel> blogViewModels;
        private ContactInputModel contactModel = new();

        protected override async Task OnInitializedAsync()
        {
            _courseService = ScopedServices.GetRequiredService<ICourseService>();
            _contactService = ScopedServices.GetRequiredService<IContactService>();
            InitData();
        }

        private async Task InitData()
        {
            topTwoOnlineCourses = await _courseService.GetTopTwoOnlineCourseForIndexPage();
            offlineCourses = await _courseService.GetTopOffCourseForIndexPage(6);
            StateHasChanged();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var module = await jsr.InvokeAsync<IJSObjectReference>("import", "./Pages/Site/Index.razor.js");
                await module.InvokeVoidAsync("indexPageModule.init");
            }
        }

        protected void SaveContactInfo(){
            _contactService.SaveContact(contactModel);
            mainLayout.ShowAlert("Cảm ơn bạn đã để lại thông tin! Chúng tôi sẽ liên hệ bạn sớm nhất có thể.", "Thông báo");            contactModel = new();
        }
        private void GoToRegisterPage(int courseId)
        {
            NavigationManager.NavigateTo("/dang-ky-khoa-hoc/" + courseId);
        }

        private void GoToDetailOnlineCourse(string path)
        {
            NavigationManager.NavigateTo(path);
        }
    }
}
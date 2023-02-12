using Blazored.TextEditor;
using KA.Infrastructure.Enums;
using KA.Infrastructure.Enums.Extension;
using KA.Service.Blogs;
using KA.ViewModels.Blogs;
using KAWebHost.Pages.Admin.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace KAWebHost.Pages.Admin.Blogs
{
    public partial class CreateBlog : OwningComponentBase
    {
        private CreateBlogVm model;
        private FileSelector fileSelectorControl;
        private string userId;
        private Dictionary<string, string> blogTypes = BlogType.KNOWLEDGE.ToDictionary();
        // Parameter
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authenticationState;


        // Service
        IBlogService _blogService;

        [Inject]
        IJSRuntime jsr { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }


        protected override async Task OnInitializedAsync()
        {
            _blogService = ScopedServices.GetRequiredService<IBlogService>();
            authenticationState = await authenticationStateTask;
            userId = authenticationState.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            model = new()
            {
                BlogType = ((int)BlogType.KNOWLEDGE).ToString()
            };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await jsr.InvokeVoidAsync("import", "/assets/plugins/custom/tinymce/tinymce.min.js");
            await jsr.InvokeVoidAsync("import", "/scripts/common/editor-common.js");
            await jsr.InvokeVoidAsync("import", "/Pages/Admin/Blogs/CreateBlog.razor.js");
            await jsr.InvokeVoidAsync("createBlogPageJs.init");
        }

        private async Task SubmitForm()
        {
            model.Content = await jsr.InvokeAsync<string>("createBlogPageJs.getTextEditorContent");
            model.CreateUserId = userId;
            model.CreatedDate = DateTime.Now;
            await _blogService.CreateBlog(model);
            await jsr.InvokeVoidAsync("ShowAppAlert", "Tạo bài viết thành công", "success");
            NavigationManager.NavigateTo("/manager/blogs");
            model = new();
        }

        private void OpenSelectImageModal(bool isFromTextEditor)
        {

            fileSelectorControl.SetShowFileManager(true);
        }

        async Task InsertImage(string paramImageURL)
        {
            model.ThumbNailImageLink = paramImageURL;
            fileSelectorControl.SetShowFileManager(false);
        }

    }
}
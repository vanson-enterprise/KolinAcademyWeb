using Blazored.TextEditor;
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
        private bool isSelectThumbNailImage;
        private string userId;

        // Parameter
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authenticationState;


        // Service
        BlazoredTextEditor quillHtml;
        IBlogService _blogService;
        [Inject]
        IJSRuntime jsr { get; set; }


        protected override async Task OnInitializedAsync()
        {
            _blogService = ScopedServices.GetRequiredService<IBlogService>();
            authenticationState = await authenticationStateTask;
            userId = authenticationState.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            model = new();
        }

        private async Task SubmitForm()
        {
            model.Content = await quillHtml.GetHTML();
            model.CreateUserId = userId;
            await _blogService.CreateBlog(model);
            await jsr.InvokeVoidAsync("ShowAppAlert", "Tạo bài viết thành công", "success");
            model = new();
        }

        private void OpenSelectImageModal(bool isFromTextEditor)
        {

            isSelectThumbNailImage = !isFromTextEditor;
            fileSelectorControl.SetShowFileManager(true);
        }

        async Task InsertImage(string paramImageURL)
        {
            if (isSelectThumbNailImage)
            {
                model.ThumbNailImageLink = paramImageURL;
            }
            else
            {
                await quillHtml.InsertImage(paramImageURL);

            }
            fileSelectorControl.SetShowFileManager(false);
        }

    }
}
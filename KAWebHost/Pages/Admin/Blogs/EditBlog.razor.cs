using Blazored.TextEditor;
using KA.Service.Blogs;
using KA.ViewModels.Blogs;
using KAWebHost.Pages.Admin.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace KAWebHost.Pages.Admin.Blogs
{
    public partial class EditBlog
    {
        private EditBlogVm model;
        private FileSelector fileSelectorControl;
        private bool isSelectThumbNailImage;
        private string userId;

        // Parameter
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private AuthenticationState authenticationState;
        [Parameter]
        public int BlogId { get; set; }


        // Service
        BlazoredTextEditor quillHtml;
        IBlogService _blogService;
        [Inject]
        IJSRuntime jsr { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _blogService = ScopedServices.GetRequiredService<IBlogService>();
            authenticationState = await authenticationStateTask;
            userId = authenticationState.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            model = _blogService.GetBlogForEdit(BlogId);
        }

        private async Task SubmitForm()
        {
            var blog = _blogService.GetById(BlogId);
            if (blog != null)
            {
                blog.Title = model.Title;
                blog.UpdatedDate = DateTime.Now;
                blog.UpdateUserId = userId;
                blog.ShortDescription = model.ShortDescription;
                blog.MetaDescription = model.MetaDescription;
                blog.MetaKeyWord = model.MetaKeyWord;
                blog.MetaTitle = model.MetaTitle;
                blog.Published = model.Published;
                blog.ThumbNailImageLink = model.ThumbNailImageLink;
                blog.Content = await quillHtml.GetHTML();

                await _blogService.UpdateAsync(blog);
                await jsr.InvokeVoidAsync("ShowAppAlert", "Cập nhật bài viết thành công", "success");
                navigationManager.NavigateTo("/manager/blogs");
            }
            else
            {
                await jsr.InvokeVoidAsync("ShowAppAlert", "Cập nhật bài viết thất bại", "error");
            }
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
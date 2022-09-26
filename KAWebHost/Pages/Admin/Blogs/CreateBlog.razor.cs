using Blazored.TextEditor;
using KA.ViewModels.Blogs;
using KAWebHost.Pages.Admin.Components;
using Microsoft.AspNetCore.Components;

namespace KAWebHost.Pages.Admin.Blogs
{
    public partial class CreateBlog : OwningComponentBase
    {
        // Models
        private CreateBlogModel createBlogModel;
        BlazoredTextEditor quillHtml;
        private FileSelector fileSelectorControl;



        // =================== Life Circle Methos ======================
        protected override async Task OnInitializedAsync()
        {
            createBlogModel = new CreateBlogModel();
        }
        private void InsertImageClick()
        {
            fileSelectorControl.SetShowFileManager(true);
        }

        async Task InsertImage(string paramImageURL)
        {
            await quillHtml.InsertImage(paramImageURL);

            fileSelectorControl.SetShowFileManager(false);
        }

    }
}

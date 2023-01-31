using KA.Service.Contacts;
using KA.ViewModels.Contact;
using KAWebHost.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using System;
using System.Drawing;
using System.Security.Claims;

namespace KAWebHost.Pages.Site
{
    public partial class Document : OwningComponentBase
    {
        [CascadingParameter]
        Task<AuthenticationState> authenticationStateTask { get; set; }
        ClaimsPrincipal user;
        List<FileObject> fileObjects { get; set; }
        string webRootPath { get; set; }
        string documentDirectory { get; set; }
        bool isOpenRegisterInformationDialog = false;
        string filePath;
        string fileName;
        

        // DI
        [Inject]
        private IWebHostEnvironment HostEnvironment { get; set; }
        [Inject]
        private IJSRuntime JS { get; set; }
        IContactService _contactService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _contactService = ScopedServices.GetRequiredService<IContactService>();
            user = (await authenticationStateTask).User;
            webRootPath = HostEnvironment.WebRootPath;
            documentDirectory = Path.Combine(webRootPath, "uploads/documents");
            SerializeFiles();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("import", "./Pages/Site/Document.razor.js");
            }
        }

        private void SerializeFiles()
        {
            if (Directory.Exists(documentDirectory))
            {
                fileObjects = Directory.EnumerateFileSystemEntries(documentDirectory)
                .Where(f => f.Contains(".docx") || f.Contains(".pdf"))
                .Select(f => new FileObject()
                {
                    FileName = Path.GetFileName(f),
                    FilePath = f
                }).ToList();
            }
        }

        private Stream GetFileStream(string filePath)
        {
            return File.OpenRead(@$"{filePath}");
        }
        private void ReceiveContactInfo(ContactInputModel input)
        {
            if(input != null)
            {
                input.CreatedDate = DateTime.Now;
                input.Note = "Đăng kí để nhận tài liệu";
                _contactService.SaveContact(input);
                DownloadFile(filePath, fileName);
            }
            isOpenRegisterInformationDialog = false;
            StateHasChanged();
        }

        private async Task DownloadFileFromStream(string filePath, string fileName)
        {
            if (!user.Identity.IsAuthenticated)
            {
                isOpenRegisterInformationDialog = true;
                this.fileName = fileName;
                this.filePath = filePath;
            }
            else
            {
                DownloadFile(filePath, fileName);
            }
        }

        private async Task DownloadFile(string filePath, string fileName)
        {
            var fileStream = GetFileStream(filePath);
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }

    }
}
using KAWebHost.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using System;
using System.Drawing;

namespace KAWebHost.Pages.Site
{
    public partial class Document : OwningComponentBase
    {
        private List<FileObject> fileObjects { get; set; }
        private string webRootPath { get; set; }
        private string documentDirectory { get; set; }

        // DI
        [Inject]
        private IWebHostEnvironment HostEnvironment { get; set; }
        [Inject]
        private IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
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

        private async Task DownloadFileFromStream(string filePath, string fileName)
        {
            var fileStream = GetFileStream(filePath);
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
    }
}
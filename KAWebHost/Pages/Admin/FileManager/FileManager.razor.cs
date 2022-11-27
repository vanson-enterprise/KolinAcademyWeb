using KAWebHost.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;
using Radzen;

namespace KAWebHost.Pages.Admin.FileManager
{
    public partial class FileManager : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        string ROLE = "Manager";
        bool ShowPopup = false;
        bool ShowFolderPopup = false;
        int progress;
        string info;
        string CurrentDirectory = "";
        string CurrentRoot = "";
        string NewFolderName = "";
        FileObject SelectedFile = new FileObject();
        List<string> Directories = new List<string>();
        List<FileObject> Files = new List<FileObject>();
        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        protected override void OnInitialized()
        {
            CurrentRoot = HostEnvironment.WebRootPath;
            CurrentDirectory = Path.Combine(CurrentRoot, "uploads");
            Directories.Add(CurrentDirectory);
            LoadFiles();
        }
        // Files
        void SelectImage(FileObject file)
        {
            if (SelectedFile.FileName == file.FileName)
            {
                SelectedFile = new FileObject();
            }
            else
            {
                SelectedFile = file;
            }
        }

        void LogChange(TreeEventArgs args)
        {
            CurrentDirectory = args.Value as string;
            LoadFiles();
        }

        string GetTextForNode(object data)
        {
            return Path.GetFileName((string)data);
        }

        RenderFragment<RadzenTreeItem> FileOrFolderTemplate = (context) => builder =>
        {
            string path = context.Value as string;
            bool isDirectory = Directory.Exists(path);
            builder.OpenComponent<RadzenIcon>(0);
            builder.AddAttribute(1,
                "Icon", isDirectory ? "folder" :
                "insert_drive_file");
            if (!isDirectory)
            {
                builder.AddAttribute(2,
                    "Style",
                    "margin-left: 24px");
            }
            builder.CloseComponent();
            builder.AddContent(3, context.Text);
        };

        void LoadDirectory(TreeExpandEventArgs args)
        {
            CurrentDirectory = args.Value as string;
            // Only get the folders not the files
            args.Children.Data =
                Directory.EnumerateFileSystemEntries(CurrentDirectory)
                .Where(x => !x.Contains("."));
            args.Children.Text = GetTextForNode;
            args.Children.HasChildren = (path) => Directory.Exists((string)path);
            args.Children.Template = FileOrFolderTemplate;
        }

        void LoadFiles()
        {
            Files = new List<FileObject>();
            var FileNames =
                Directory.EnumerateFileSystemEntries(CurrentDirectory)
                .Where(x => x.Contains("."));
            foreach (var item in FileNames)
            {
                Files.Add(new FileObject()
                {
                    FileName =
                                    Path.GetFileName(item),
                    FilePath =
                                    item.Replace(HostEnvironment.WebRootPath, "")
                });
            }
            // Reset selected file
            SelectedFile = new FileObject();
            // Update UI
            StateHasChanged();
        }

        async void OnProgress(UploadProgressArgs args)
        {
            ShowPopup = true;
            this.info = $"{args.Loaded} of {args.Total} bytes.";
            this.progress = args.Progress;
            StateHasChanged();
            if (args.Loaded == args.Total)
            {
                // Delay to give files time to be saved
                // before reloading file view
                await LoadFilesAsync();
            }
        }
        public async Task LoadFilesAsync()
        {
            int Time = 1;
            while (Time > 0)
            {
                Time--;
                StateHasChanged();
                await Task.Delay(1000);
            }
            ShowPopup = false;
            LoadFiles();
        }

        private string StyleForFile(string fileName)
        {
            if(fileName == SelectedFile.FileName)
            {
                return @"max-width:180px;background-color:darkgrey";
            }
            else
            {
                return @"max-width:180px;background-color:#fff";
            }
        }
    }
}

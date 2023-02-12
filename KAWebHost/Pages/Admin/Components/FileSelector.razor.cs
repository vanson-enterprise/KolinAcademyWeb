using KAWebHost.Data.Models;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;

namespace KAWebHost.Pages.Admin.Components
{
    public partial class FileSelector : ComponentBase
    {
        // ImageSelected is an EventCallback that will
        // notify the parent component when an image is selected
        // passing the url of the image to the parent component
        [Parameter] public EventCallback<string> OnFileSelected { get; set; }

        private RadzenDataList<FileObject> FileDataList;
        private bool ShowFileManager = false;
        bool ShowPopup = false;
        int progress;
        string info;
        private string CurrentDirectory = "";
        private string CurrentRoot = "";
        private FileObject SelectedFile = new FileObject();
        private List<string> Directories = new List<string>();
        private List<FileObject> Files = new List<FileObject>();
        private Dictionary<DateTime, string> events =
            new Dictionary<DateTime, string>();

        protected override void OnInitialized()
        {
            CurrentRoot = HostEnvironment.WebRootPath;
            CurrentDirectory = Path.Combine(CurrentRoot, "uploads");
            Directories.Add(CurrentDirectory);
            LoadFiles();
        }

        public void SetShowFileManager(bool paramSetting)
        {
            ShowFileManager = paramSetting;
        }

        //SelectFile

        private async Task SelectFile()
        {
            await OnFileSelected.InvokeAsync(SelectedFile.FilePath);
        }

        // Files

        private void SelectImage(FileObject file)
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

        private void LogChange(TreeEventArgs args)
        {
            // Get the current directory from the
            // argument passed to the method
            CurrentDirectory = args.Value as string;

            // Set the RadzenDataList to page 1
            FileDataList.FirstPage();

            // Reload the FileDataList
            LoadFiles();
        }

        private string GetTextForNode(object data)
        {
            return Path.GetFileName((string)data);
        }

        private RenderFragment<RadzenTreeItem>
            FileOrFolderTemplate = (context) => builder =>
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

        private void LoadDirectory(TreeExpandEventArgs args)
        {
            CurrentDirectory = args.Value as string;

            // Only get the folders not the files
            args.Children.Data =
                Directory.EnumerateFileSystemEntries(CurrentDirectory)
                .Where(x => !x.Contains("."));

            args.Children.Text = GetTextForNode;
            args.Children.HasChildren =
                (path) => Directory.Exists((string)path);
            args.Children.Template = FileOrFolderTemplate;
        }

        private void LoadFiles()
        {
            Files = new List<FileObject>();

            IEnumerable<string> fileNames = fileNames = Directory.EnumerateFileSystemEntries(CurrentDirectory)
              .Where(x => x.Contains(".PNG") || x.Contains(".png") || x.Contains(".jpg") || x.Contains(".jpeg") || x.Contains(".mp4"));

            foreach (var item in fileNames)
            {
                //using (var image = Image.FromFile(item))
                //{
                //    // Calculate Thumbnail
                //    int thumbnailHeight = 100;
                //    int thumbnailWidth = 100;
                //    double x = image.Height / 100;
                //    if (x > 0)
                //    {
                //        thumbnailHeight = Convert.ToInt32(image.Height / x);
                //        thumbnailWidth = Convert.ToInt32(image.Width / x);
                //    }
                //}
                Files.Add(new FileObject()
                {
                    FileName = Path.GetFileName(item),
                    FilePath = item.Replace(HostEnvironment.WebRootPath, "").Replace("\\uploads", "uploads"),
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

        private void DeleteSelectedFile()
        {
            string RequestedPath = SelectedFile.FilePath;

            RequestedPath =
                RequestedPath.Replace("\\uploads\\", "uploads\\");

            string path = Path.Combine(
                HostEnvironment.WebRootPath,
                RequestedPath);

            if (File.Exists(path))
            {
                File.Delete(path);

                LoadFiles();
            }
        }

        private string StyleForFile(string fileName)
        {
            if (fileName == SelectedFile.FileName)
            {
                return @"max-width:120px;background-color:darkgrey";
            }
            else
            {
                return @"max-width:120px;background-color:#fff";
            }
        }
    }
}

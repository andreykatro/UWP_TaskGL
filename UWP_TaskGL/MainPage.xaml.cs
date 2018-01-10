using System;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UWP_TaskGL
{
    public sealed partial class MainPage : Page
    {
        private ListMemoryFile listMemoryFile;

        public MainPage()
        {
            this.InitializeComponent();
            listMemoryFile = new ListMemoryFile();
        }

        private async Task<ListMemoryFile> GetLisFilesAsync(StorageFolder root, string dirStruct)
        {
            var files = await root.GetFilesAsync();
            foreach (var file in files)
            {
                byte[] byteFile = await ZipLib.Serialize.ConvertFileToByteAsync(await file.OpenStreamForReadAsync());
                listMemoryFile.Add(new ModelMemoryFile { DirStructure = dirStruct, FileName = file.Name, ByteFile = byteFile });
                myListView.Items.Add(dirStruct + "/" + file.Name);
            }

            foreach (var folder in await root.GetFoldersAsync())
            {
                await GetLisFilesAsync(folder, dirStruct + "/" + folder.Name);
            }

            var IsEmptyFolder = await root.GetItemsAsync();
            if (IsEmptyFolder.Count == 0)
            {
                listMemoryFile.Add(new ModelMemoryFile { DirStructure = dirStruct });
                myListView.Items.Add(dirStruct);
            }

            return listMemoryFile;
        }

        private async void BtSerialize_ClickAsync(object sender, RoutedEventArgs e)
        {
            myListView.Items.Clear();
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                myProgressBar.IsIndeterminate = true;

                await Task.Yield();
                var list = await GetLisFilesAsync(folder, folder.Name);

                StorageFile storageFile = await folder.CreateFileAsync(folder.Name + ".bin", CreationCollisionOption.GenerateUniqueName);
                var stream = await storageFile.OpenStreamForWriteAsync();
                ZipLib.Serialize.SerializeToBinaryFile(stream, list);

                myProgressBar.IsIndeterminate = false;
            }
        }
        private async Task<StorageFolder> GetFolderForFile(StorageFolder folder, string dirStructure)
        {
            string[] namesFolders = dirStructure.Split('/');

            StorageFolder newFolder = folder;
            foreach (var item in namesFolders)
            {
                var tmpFolder = await newFolder.TryGetItemAsync(item) == null ? await newFolder.CreateFolderAsync(item) : await newFolder.GetFolderAsync(item);
                newFolder = tmpFolder;
            }
            return newFolder;
        }

        private async void BtDeserialize_ClickAsync(object sender, RoutedEventArgs e)
        {
            myListView.Items.Clear();

            var filePicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            filePicker.FileTypeFilter.Add(".bin");

            StorageFile storageFile = await filePicker.PickSingleFileAsync();


            if (storageFile != null)
            {
                var messageDialog = new MessageDialog("Select a Destination and Extract Files");

                messageDialog.Commands.Add(new UICommand("OK"));
                messageDialog.Commands.Add(new UICommand("Close"));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Set the command to be invoked when escape is pressed
                messageDialog.CancelCommandIndex = 1;
                // Show the message dialog
                await messageDialog.ShowAsync();

                var folderPicker = new FolderPicker
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };

                folderPicker.FileTypeFilter.Add("*");
                try
                {
                    StorageFolder folder = await folderPicker.PickSingleFolderAsync();

                    if (folder != null)
                    {
                        myProgressBar.IsIndeterminate = true;

                        var listObj = (ListMemoryFile)ZipLib.Deserialize.DeserializeFromBinary(await storageFile.OpenStreamForReadAsync());
                        var newFolder = folder;
                        foreach (var item in listObj)
                        {
                            myListView.Items.Add(item.DirStructure + "/" + item.FileName);

                            await Task.Yield();
                            newFolder = await GetFolderForFile(folder, item.DirStructure);
                            if (item.FileName != null)
                            {
                                StorageFile file = await newFolder.CreateFileAsync(item.FileName, CreationCollisionOption.GenerateUniqueName);
                                await FileIO.WriteBytesAsync(file, item.ByteFile);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msgDialog = new MessageDialog("Oops!");
                    await msgDialog.ShowAsync();
                    Debug.WriteLine("Main Page. 'BtDeserialize_ClickAsync' " + ex.Message);
                }
                finally
                {
                    myProgressBar.IsIndeterminate = false;
                }
            }

        }
    }
}

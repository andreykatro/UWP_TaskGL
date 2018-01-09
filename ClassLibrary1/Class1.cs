using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace ClassLibrary1
{
    public class Class1
    {
        public List<string> GetFilesName { get; private set; }

        public async void AddFileName(StorageFolder mainFolder)
        {
            var files = await mainFolder.GetFilesAsync();
            IReadOnlyList<StorageFolder> folders = await mainFolder.GetFoldersAsync();
            //List<string> listNameFiles = new List<string>();
            try
            {
                if (files.Count() > 0)
                {
                    foreach (var file in files)
                    {
                        //GetFilesName.Add(file.Name);
                    }

                }

                if (folders.Count() > 0)
                {
                    foreach (var folder in folders)
                    {
                        AddFileName(folder);
                    }
                }
            }
            catch (Exception ex)
            {

                var dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }
        }
    }
}

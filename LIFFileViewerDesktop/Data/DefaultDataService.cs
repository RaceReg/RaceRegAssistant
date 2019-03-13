using Avalonia.Controls;
using LIFFileViewer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LIFFileViewerDesktop.Data
{
    class DefaultDataService : IDataService
    {
        public bool FileExists(string lifFILE)
        {
            return File.Exists(lifFILE);
        }

        public async Task<string> FindDirectoryAsync()
        {
            var openFolderDialog = new OpenFolderDialog()
            {
                Title = "Select Directory  LIF results file:",
                //Filters = { "*.lif" };
            };

            var path = await openFolderDialog.ShowAsync();

            if (!string.Equals(path, "") && path != null)
            {
                return path;
            }
            return null;
        }

        public async Task<string> FindFileAsync()
        {
            var openFileDialog = new OpenFileDialog()
            {
                AllowMultiple = false,
                Title = "Select FinishLynx LIF results file:",
                //Filters = { "*.lif" };
            };

            var pathArray = await openFileDialog.ShowAsync();

            if((pathArray?.Length ?? 0) > 0)
            {
                return pathArray[0];
            }
            return null;
        }

        public Task<LIF> GetEntriesFromLIFAsync(string lifFILE)
        {
            return Task.Run(() =>
            {
                var reader = new LIFReader(lifFILE);

                return reader.GetLIFObject();
            });
        }
    }
}

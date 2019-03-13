using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RaceRegAssistant;

namespace LIFFileViewerDesktop.Data
{
    class DefaultDataService : IDataService
    {
        public bool FileExists(string lifFILE)
        {
            return File.Exists(lifFILE);
        }

        public string FindDirectory()
        {
            var openFolderDialog = new FolderBrowserDialog()
            { 
                Description = "Select Directory  LIF results file:",
                //Filters = { "*.lif" };
            };

            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                return openFolderDialog.SelectedPath;
            }
            return null;
        }

        public string FindFile()
        {
            var openFileDialog = new OpenFileDialog()
            {
                Multiselect=false,
                Title = "Select FinishLynx LIF results file:",
                //Filters = { "*.lif" };
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
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

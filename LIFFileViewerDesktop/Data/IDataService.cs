//Code obtained/modified from Johnathan Allen

using RaceRegAssistant;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIFFileViewerDesktop.Data
{
    public interface IDataService
    {
        Task<LIF> GetEntriesFromLIFAsync(string lifFILE);
        bool FileExists(string lifFILE);
        Task<string> FindFileAsync();
        Task<string> FindDirectoryAsync();
    }
}

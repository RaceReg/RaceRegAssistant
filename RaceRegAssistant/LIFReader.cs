using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using LIFFileViewer.Model;

namespace RaceRegAssistant
{
    public class LIFReader
    {
        private string lifFILE;
        public LIF lif;

        public LIFReader(string lifFILE)
        {
            this.lifFILE = lifFILE;
            lif = new LIF();

            AnalyzeAndLoad();
        }

        private async void AnalyzeAndLoad()
        {
            string[] lines = await System.IO.File.ReadAllLinesAsync(lifFILE);

            LoadFirstLine(lines[0]);
            LoadEntries(lines);
        }

        private void LoadFirstLine(string firstLine)
        {
            string[] split = firstLine.Split(',');
            lif.EventId = int.Parse(split[0]);
            lif.RoundId = int.Parse(split[1]);
            lif.HeatId = int.Parse(split[2]);
            lif.EventName = split[3];
            lif.EventDistance = int.Parse(split[9]);
            lif.StartTime = DateTime.Parse(split[10]);
        }

        private void LoadEntries(string[] lines)
        {
            for(int i = 1; i < lines.Length; i++)
            {
                string[] split = lines[i].Split(',');

                Entry entry = new Entry();
                entry.Place = int.Parse(split[0]);
                entry.RacerId = int.Parse(split[1]);
                entry.Lane = int.Parse(split[2]);
                entry.LastName = split[3];
                entry.FirstName = split[4];
                entry.Affiliation = split[5];
                //entry.Time = DateTime.Parse(split[6]);
                entry.Time = split[6];

                lif.AddEntry(entry);
            }
        }

        public LIF GetLIFObject()
        {
            return this.lif;
        }
    }
}

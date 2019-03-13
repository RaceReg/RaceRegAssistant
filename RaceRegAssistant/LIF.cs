using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using LIFFileViewer.Model;

namespace RaceRegAssistant
{
    public class LIF : INotifyPropertyChanged
    {
        private ObservableCollection<Entry> entries;
        public ObservableCollection<Entry> Entries
        {
            get
            {
                return entries;
            }
            set
            {
                entries = value;
                OnPropertyChanged(nameof(Entries));
            }
        }

        private Entry selectedEntry;
        public Entry SelectedEntry
        {
            get
            {
                return selectedEntry;
            }
            set
            {
                selectedEntry = value;
                OnPropertyChanged(nameof(SelectedEntry));
            }
        }

        public int EventId { get; internal set; }
        public int RoundId { get; internal set; }
        public int HeatId { get; internal set; }
        public string EventName { get; internal set; }
        public int EventDistance { get; internal set; }
        public DateTime StartTime { get; internal set; }

        public LIF()
        {
            entries = new ObservableCollection<Entry>();

            //var tempEntry = new Entry();
            //tempEntry.Place = 1;
            //tempEntry.RacerId = 0001;
            //tempEntry.Lane = 1;
            //tempEntry.LastName = "Porter";
            //tempEntry.FirstName = "Jackson";
            //tempEntry.Affiliation = "SC";
            //tempEntry.Time = DateTime.Now;

            //entries.Add(tempEntry);
        }

        internal void AddEntry(Entry entry)
        {
            entries.Add(entry);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

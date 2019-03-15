using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace RaceRegAssistant.Model
{
    public class LIF : INotifyPropertyChanged
    {
        private ObservableCollection<LIFEntry> entries;
        public ObservableCollection<LIFEntry> Entries
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

        private LIFEntry selectedEntry;
        public LIFEntry SelectedEntry
        {
            get
            {
                return selectedEntry;
            }
            set
            {
                Set(ref selectedEntry, value);
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
            entries = new ObservableCollection<LIFEntry>();
        }

        internal void AddEntry(LIFEntry entry)
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

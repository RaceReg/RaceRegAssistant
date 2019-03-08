using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIFFileViewer.Model
{
    public class Entry
    {
        private int place;
        private int racerId;
        private int lane;
        private string lastName;
        private string firstName;
        private string affiliation;
        //private DateTime time; //WILL CONVERT TO A TIME FORMAT EVENTUALLY
        private string time;

        public int Place
        {
            get
            {
                return place;
            }
            set
            {
                place = value;
            }
        }
        public int RacerId
        {
            get
            {
                return racerId;
            }
            set
            {
                racerId = value;
            }
        }
        public int Lane
        {
            get
            {
                return lane;
            }
            set
            {
                lane = value;
            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }
        public string Affiliation
        {
            get
            {
                return affiliation;
            }
            set
            {
                affiliation = value;
            }
        }
        //public DateTime Time
        //{
        //    get
        //    {
        //        return time;
        //    }
        //    set
        //    {
        //        time = value;
        //    }
        //}
        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }
    }
}

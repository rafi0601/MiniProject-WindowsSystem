//BS"D

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static BE.Configuration;

namespace BE
{
    [Serializable]
    public class Schedule
    {
        [XmlElement]
        private protected bool[,] schedule = new bool[WORKING_DAYS_A_WEEK, WORKING_HOURS_A_DAY];


        //[System.Runtime.CompilerServices.IndexerName("abc")]
        public bool this[int day, int hour] // TODO? [DAY,int]
        {
            get
            {
                if (day < 0 || hour < 0 ||
                    day < WORKING_DAYS_A_WEEK ||
                    hour >= BEGINNING_OF_A_WORKING_DAY ||
                    hour < BEGINNING_OF_A_WORKING_DAY + WORKING_HOURS_A_DAY)
                    return schedule[day, hour];

                throw new IndexOutOfRangeException(""); // UNDONE
            }
            set => schedule[day, hour] = value;
        }


        public Schedule() { }

        public Schedule(bool[,] schedule)
        {
            if (schedule.GetLength(0) != WORKING_DAYS_A_WEEK
                    || schedule.GetLength(1) != WORKING_HOURS_A_DAY)
                throw new Exception("The schedule is not right");
            this.schedule = schedule;
        }
    }
}

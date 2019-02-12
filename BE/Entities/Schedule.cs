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
        [XmlElement] //CHECK what is this attribute
        private protected bool[,] schedule = new bool[WORKING_DAYS_A_WEEK, WORKING_HOURS_A_DAY];


        //[System.Runtime.CompilerServices.IndexerName("abc")]
        public bool this[int day, int hour] // TODO? [DAY,int]
        {
            get
            {
                if (day < 0 || day >= WORKING_DAYS_A_WEEK ||
                    hour < BEGINNING_OF_A_WORKING_DAY || hour >= BEGINNING_OF_A_WORKING_DAY + WORKING_HOURS_A_DAY)
                    throw new IndexOutOfRangeException("The day or time exceeds the schedule");

                return schedule[day, hour - BEGINNING_OF_A_WORKING_DAY];
            }
            set
            {
                if (day < 0 || day >= WORKING_DAYS_A_WEEK ||
                    hour < BEGINNING_OF_A_WORKING_DAY || hour >= BEGINNING_OF_A_WORKING_DAY + WORKING_HOURS_A_DAY)
                    throw new IndexOutOfRangeException("The day or time exceeds the schedule");

                schedule[day, hour - BEGINNING_OF_A_WORKING_DAY] = value;
            }
        }


        public Schedule() { }

        public Schedule(bool[,] schedule)
        {
            if (schedule?.GetLength(0) != WORKING_DAYS_A_WEEK
                    || schedule.GetLength(1) != WORKING_HOURS_A_DAY)
                throw new Exception("The schedule is not correct dimensions");
            this.schedule = schedule.Copy(); //TODO to this in all ENTITIES
        }
    }
}

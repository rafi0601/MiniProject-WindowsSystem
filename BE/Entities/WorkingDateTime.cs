//Bs"d

#define CalcLenthOfTest

using System;
using static BE.Configuration;

namespace BE
{
    public struct WorkingDateTime
    {
        private DateTime _dateAndTime;

        public WorkingDateTime(DateTime dateAndTime) : this()
        {
            DateAndTime = dateAndTime;
        }

        private DateTime DateAndTime
        {
            get => _dateAndTime;
            set
            {
                if (value.DayOfWeek < BEGINNING_OF_A_WORKING_WEEK || value.DayOfWeek > END_OF_A_WORKING_WEEK)
                    throw new InvalidCastException("Not valid date");//TODO more information ("only between B-E")
                if (value.Hour < BEGINNING_OF_A_WORKING_DAY || value.Hour > END_OF_A_WORKING_DAY)
                    throw new InvalidCastException("Not valid hour");

                _dateAndTime = value;
            }
        }

        //public static WorkingDateTime operator =(WorkingDateTime workingDateTime, DateTime dateTime)
        //{
        //    return new WorkingDateTime();
        //}

        public static implicit operator DateTime(WorkingDateTime testDate)
        {
            return testDate.DateAndTime;
        }

        public static explicit operator WorkingDateTime(DateTime dateTime)
        {
            return new WorkingDateTime(dateTime);
        }

        public static TimeSpan operator -(WorkingDateTime testDateTime, DateTime dateTime)
        {
            return (DateTime)testDateTime - dateTime;
        }

        public static TimeSpan operator -(DateTime dateTime, WorkingDateTime testDateTime)
        {
            return -(testDateTime - dateTime);
        }

        public static WorkingDateTime operator ++(WorkingDateTime testDateTime)
        {
            DateTime dateTime;
            if (testDateTime.DateAndTime.Hour < END_OF_A_WORKING_DAY)
                dateTime = testDateTime.DateAndTime.AddHours(1);
            else if (testDateTime.DateAndTime.DayOfWeek < END_OF_A_WORKING_WEEK)
                dateTime = testDateTime.DateAndTime;
            else dateTime = new DateTime();
            dateTime = dateTime.AddDays(dateTime.DayOfWeek != DayOfWeek.Thursday ? 1 : 7 - WORKING_DAYS_A_WEEK + 1);
            testDateTime.DateAndTime = testDateTime.DateAndTime.AddHours(-WORKING_HOURS_A_DAY);
            return (WorkingDateTime)dateTime;

        }

        //public static DateTime operator +(DateTime d, TimeSpan t);
        //public static TimeSpan operator -(DateTime d1, DateTime d2);
        //public static DateTime operator -(DateTime d, TimeSpan t);
        //public static bool operator ==(DateTime d1, DateTime d2);
        //public static bool operator !=(DateTime d1, DateTime d2);
        //public static bool operator <(DateTime t1, DateTime t2);
        //public static bool operator >(DateTime t1, DateTime t2);
        //public static bool operator <=(DateTime t1, DateTime t2);
        //public static bool operator >=(DateTime t1, DateTime t2);
    }
}
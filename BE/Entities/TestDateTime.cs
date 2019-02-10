//Bs"d

#define CalcLenthOfTest

using System;

namespace BE
{
    public struct TestDateTime
    {
        private DateTime _dateAndTime;

        public DateTime DateAndTime { get => _dateAndTime; set => _dateAndTime = value; }

        //public static TestDateTime operator =(TestDateTime testDate, DateTime dateTime)
        //{
        //    testDate.DateAndTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
        //    return testDate;
        //}

        public static implicit operator DateTime(TestDateTime testDate)
        {
            return testDate.DateAndTime;
        }
    }
}
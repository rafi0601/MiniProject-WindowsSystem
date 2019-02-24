//BS"D

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public static class Configuration
    {
        public const uint BEGINNING_OF_A_WORKING_DAY = 9;
        public const uint WORKING_HOURS_A_DAY = 7;
        public const uint END_OF_A_WORKING_DAY = BEGINNING_OF_A_WORKING_DAY + WORKING_HOURS_A_DAY - 1;
        public const DayOfWeek BEGINNING_OF_A_WORKING_WEEK = DayOfWeek.Sunday;
        public const uint WORKING_DAYS_A_WEEK = 5;
        public const DayOfWeek END_OF_A_WORKING_WEEK = (DayOfWeek)((int)BEGINNING_OF_A_WORKING_WEEK + WORKING_DAYS_A_WEEK - 1);//DayOfWeek.Thursday;

        public static readonly uint DEFAULT_DISTANCE = 10;
        public static readonly TimeSpan DEFAULT_TIME = TimeSpan.Zero;
        public static readonly string KEY = @"PffGghfFGtzNFx1MqL9NLykkFHmpHkmc";

        public const uint LENGTH_OF_CODE = 8;
        public static readonly uint MAX_VALUE_TO_CODE = (uint)Math.Pow(10, LENGTH_OF_CODE) - 1;

        public static readonly TimeSpan LENGTH_OF_TEST = new TimeSpan(1, 0, 0);
        public static readonly TimeSpan MAX_AGE_OF_TESTER = new TimeSpan((int)(365.25 * 90), 0, 0, 0); // TODO: search for better
        public static readonly TimeSpan MIN_AGE_OF_TESTER = new TimeSpan((int)(365.25 * 40), 0, 0, 0);
        public static readonly TimeSpan MIN_AGE_OF_TRAINEE = new TimeSpan((int)(365.25 * 18), 0, 0, 0);
        public static readonly uint MIN_CRITERIONS_TO_PASS_TEST = (uint)Math.Round(Test.Criteria.NumberOfCriteria * 0.8); // 80%
        public static readonly uint MIN_LESSONS_TO_ORDER_TEST = 20;
        public static readonly uint QUOTA_OF_TESTS_FOR_TESTER_FOR_WEEK = 25;
        public static readonly Random rand = new Random();
        public static readonly TimeSpan SPAN_FROM_TODAY_TO_SEARCH_TEST = new TimeSpan(days: 30, 0, 0, 0);
        public static readonly TimeSpan TIME_RANGE_BETWEEN_TESTS = new TimeSpan(7, 0, 0, 0);

        //public static readonly DateTime BEGINNING_OF_A_WORKING_DAY = new DateTime(hour: 9, minute: 0, second: 0);

    }
}
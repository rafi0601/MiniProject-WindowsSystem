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
        public static readonly uint MAX_VALUE_TO_CODE = (uint)Math.Pow(10, 9) - 1;
        public static readonly uint LENGTH_OF_CODE = 8;

        public static readonly TimeSpan LENGTH_OF_TEST = new TimeSpan(1, 0, 0);
        public static readonly uint MIN_LESSONS = 20;
        public static readonly TimeSpan MAX_AGE_OF_TESTER = new TimeSpan(365 * 90, 0, 0, 0); // TODO: search for better
        public static readonly TimeSpan MIN_AGE_OF_TRAINEE = new TimeSpan(365 * 18, 0, 0, 0);
        public static readonly TimeSpan MIN_AGE_OF_TESTER = new TimeSpan(365 * 40, 0, 0, 0);
        public static readonly TimeSpan TIME_RANGE_BETWEEN_TESTS = new TimeSpan(7, 0, 0, 0);
        public static readonly uint QUOTA_OF_TESTS_FOR_TESTER_FOR_WEEK = 25;
        public static readonly uint DEFAULT_DISTANCE = 10;
        public static readonly Random rand = new Random();
        public static readonly uint MIN_CRITERIONS_TO_PASS_TEST = (uint)Math.Round(Test.Criteria.NumberOfCriteria * 0.8); // 80%
        //public static readonly uint MIN_CRITERIONS_TO_PASS_TEST = 4; // 80%
        public static readonly uint WORKING_DAYS_A_WEEK = 5;
        public static readonly uint WORKING_HOURS_A_DAY = 7;
        public static readonly uint BEGINNING_OF_A_WORKING_DAY = 9;
        //public static readonly DateTime BEGINNING_OF_A_WORKING_DAY = new DateTime(hour: 9, minute: 0, second: 0);

        public static readonly string KEY = @"PffGghfFGtzNFx1MqL9NLykkFHmpHkmc";
    }
}
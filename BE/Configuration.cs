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
        public static readonly TimeSpan Length_OF_TEST = new TimeSpan(1, 0, 0);
        public static readonly uint MIN_LESSONS = 20;
        public static readonly TimeSpan MAX_AGE_OF_TESTER = new TimeSpan(365 * 90, 0, 0, 0); // TODO: search for better
        public static readonly TimeSpan MIN_AGE_OF_TRAINEE = new TimeSpan(365 * 18, 0, 0, 0);
        public static readonly TimeSpan MIN_AGE_OF_TESTER = new TimeSpan(365 * 40, 0, 0, 0);
        public static readonly TimeSpan TIME_RANGE_BETWEEN_TESTS = new TimeSpan(7, 0, 0, 0);
        public static readonly uint QUOTA_OF_TESTS_FOR_TESTER_FOR_WEEK = 25;
        public static readonly uint X = 10;
        public static readonly Random rand = new Random();
        public static readonly uint MIN_CRITERIONS_TO_PASS_TEST = (uint)Math.Round(Test.Criteria.NumberOfCriteria * 0.8); // 80%
    }
}
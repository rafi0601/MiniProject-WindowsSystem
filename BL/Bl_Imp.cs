//Bs"d

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public partial class Bl_imp : IBL
    {

        private readonly DAL.IDal dal;

        public Bl_imp()
        {
            dal = DAL.Singleton.Instance;
        }


        #region Tester functions

        public void AddTester(Tester tester)
        {
            Inspections.TesterInspection(tester);

            if (ExistingTesterById(tester.ID))
                throw new ArgumentException("This tester already exists in the database");

            TimeSpan testerAge = DateTime.Today - tester.Birthdate;
            if (testerAge < Configuration.MIN_AGE_OF_TESTER || testerAge > Configuration.MAX_AGE_OF_TESTER)
                throw new ArgumentOutOfRangeException(nameof(Tester.Birthdate.Year), "The tester's age is not appropriate");

            try
            {
                dal.AddTester(tester);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveTester(string idTester)
        {
            if (!ExistingTesterById(idTester))
                throw new ArgumentException("This tester doesn't exist in the database");

            try
            {
                dal.RemoveTester(dal.GetTester(idTester));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateTester(Tester tester)
        {
            Inspections.TesterInspection(tester);

            if (!ExistingTesterById(tester.ID))
                throw new ArgumentException("This tester doesn't exist in the database");

            try
            {
                /// BetterOption (because inputcheck)
                /// this.Removetester
                /// this.AddTester
                dal.UpdateTester(tester);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Tester GetTester(string id)
        {
            return dal.GetTester(id);
        }

        public List<Tester> GetTesters(Predicate<Tester> predicate = null)
        {
            return dal.GetTesters(predicate);
        }

        public List<Tester> TheTestersWhoLiveInTheDistance(Address address)
        {
            return (from tester in dal.GetTesters()
                    where Configuration.rand.Next(1, 40) < Configuration.X
                    select tester)
                          .ToList();
        }

        public List<Tester> VacantTesters(DateTime dateAndTime)
        {
            return (from tester in dal.GetTesters()
                    where tester.MyTests.All(test => test.TestDate != dateAndTime) && !tester.WorkingHours[(int)dateAndTime.DayOfWeek, dateAndTime.Hour - 9] //TODO 9=CONFIG.BEGINNING_OF_DAY
                    select tester)
                    .ToList();
        }

        public List<IGrouping<Vehicle, Tester>> TestersByExpertise(bool toSort = false)
        {
            List<Tester> testers = dal.GetTesters();

            if (toSort)
                testers.Sort((tester1, tester2) => (int)tester1.YearsOfExperience - (int)tester2.YearsOfExperience);

            return (from tester in testers
                    group tester by tester.VehicleTypeExpertise)
                    .ToList();
        }

        #endregion

        #region Trainee functions

        public void AddTrainee(Trainee trainee)
        {
            Inspections.TraineeInspection(trainee);

            if (DateTime.Today - trainee.Birthdate < Configuration.MIN_AGE_OF_TRAINEE)
                throw new ArgumentOutOfRangeException(nameof(Trainee.Birthdate.Year), "This Trainee's age is not appropriate");

            if (ExistingTraineeById(trainee.ID))
                throw new ArgumentException("This trainee already exists in the database");

            try
            {
                dal.AddTrainee(trainee);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveTrainee(string idTrainee)
        {
            if (!ExistingTraineeById(idTrainee))
                throw new ArgumentException("This Trainee doesn't exist in the database");

            try
            {
                dal.RemoveTrainee(dal.GetTrainee(idTrainee));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateTrainee(Trainee trainee)
        {
            Inspections.TraineeInspection(trainee);
            if (!ExistingTraineeById(trainee.ID))
                throw new ArgumentException("This trainee doesn't exist in the database");

            try
            {
                dal.UpdateTrainee(trainee);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Trainee GetTrainee(string id)
        {
            return dal.GetTrainee(id);
        }

        public List<Trainee> GetTrainees(Predicate<Trainee> predicate = null)
        {
            return dal.GetTrainees(predicate);
        }

        public uint NumberOfDoneTests(Trainee trainee)
        {
            if (!ExistingTraineeById(trainee?.ID))
                throw new ArgumentException("This trainee doesn't exist in the database");

            return (uint)dal.GetTests(test => test.IDTrainee == trainee.ID).Count;
        }

        public bool HasPassed(Trainee trainee)
        {
            return dal.GetTests(t => t.IDTrainee == trainee.ID && /*it is improvement because now it's always true*/ trainee.Vehicle == trainee.Vehicle).First().IsPass; //BUG if not found thereisnt ispass
        }

        public List<IGrouping<string, Trainee>> TraineesByDrivingSchool(bool toSort = false)
        {
            List<Trainee> trainees = dal.GetTrainees();

           // if (!toSort)
           //     return (from trainee in trainees
           //             group trainee by trainee.DrivingSchool)
           //             .ToList();
           //
           // return (from trainee in trainees
           //         orderby trainee.TeacherName
           //         group trainee by trainee.DrivingSchool)
           //         //into tmp orderby
           //             .ToList();



            if (toSort)
                trainees.Sort((trainee1, trainee2) => string.Compare(trainee1.TeacherName.ToString(), trainee2.TeacherName.ToString()));

            return (from trainee in trainees
                    group trainee by trainee.DrivingSchool)
                    .ToList();
        }

        public List<IGrouping<Name, Trainee>> TraineesByTeacher(bool toSort = false)
        {
            List<Trainee> trainees = dal.GetTrainees();

            if (toSort)
                trainees.Sort((trainee1, trainee2) => string.Compare(trainee1.Name.ToString(), trainee2.Name.ToString()));

            return (from trainee in trainees
                    group trainee by trainee.TeacherName).ToList();
        }

        public List<IGrouping<uint, Trainee>> TraineesByNumberOfTests(bool toSort = false)
        {
            List<Trainee> trainees = dal.GetTrainees();

            if (toSort)
                trainees.Sort((trainee1, trainee2) => string.Compare(trainee1.DrivingSchool, trainee2.DrivingSchool));

            return (from trainee in trainees
                    group trainee by (uint)GetTests().FindAll(t => t.IDTrainee == trainee.ID).Count).ToList();
        }

        #endregion

        #region Test functions

        public DateTime? AddTest(Trainee trainee, DateTime testDate, DateTime length, Address departureAddress, Vehicle vehicle)
        { // return bool is success, and get out HATZAA
            if (!ExistingTraineeById(trainee.ID))
                throw new ArgumentException("This Trainee doesn't exist in the database");
            
            if (DateTime.Now - trainee.TheLastTest < Configuration.TIME_RANGE_BETWEEN_TESTS)
                throw new ArgumentException("It is illegal to access to test less than 7 days after the last one");

            if (trainee.NumberOfDoneLessons < Configuration.MIN_LESSONS)
                throw new ArgumentException("It is illegal to access to test if you did less than " + Configuration.MIN_LESSONS + " lessons");

            if (dal.GetTests(t => t.IDTrainee == trainee.ID && t.TestDate == testDate).Count != 0)
                throw new ArgumentException("It is illegal to set for a trainee two tests at the same time");

            if (dal.GetTests(t => t.IDTrainee == trainee.ID && t.Vehicle == vehicle && t.IsPass == true).Count != 0)
                throw new ArgumentException("It is illegal for a trainee to take the test he has succeeded in, once more");

            if (trainee.Vehicle != vehicle)
                throw new ArgumentException("It is illegal for a trainee to take a test on a vehicle he has not learned to drive");




            try
            {
                Tester tester = FindingAvailableTester(testDate, vehicle);
                if (tester != null)
                {
                    dal.AddTest(new Test(tester.ID, trainee.ID, testDate, length, departureAddress, vehicle));
                    return null;
                }

                return FindingAnAlternativeTest(vehicle).Value.Item1;

            }
            catch (Exception e)
            {
                throw e;
            }

            //throw new Exception("There is no available Tester on the requested date");

        }

        public void UpdateTest(string code, Test.Criteria criteria, bool isPass, string testerNotes)
        {
            //if (!dal.GetTest(test.Code).Exists(t => t.IDTrainee == trainee.ID && t.Vehicle == vehicle && !t.IsDone()))
            Test theTest = dal.GetTest(code);
            if (theTest == null)
                throw new ArgumentException("This test doesn't exist in the database");
            if (theTest.IsDone()) // BUG ISDONE=true immediately when the test over!
                throw new Exception("Cannot update updated test");

            if (isPass && !IsEntitledToLicense(criteria))
                throw new ArgumentException("It is illegal to pass a test if the trainee does not pass through more than" + Configuration.MIN_CRITERIONS_TO_PASS_TEST + "Cartierians");


            theTest.CriteriasGrades = criteria;
            theTest.IsPass = isPass;
            theTest.TesterNotes = testerNotes;
            //test.IsDone = true;

            try
            {
                dal.UpdateTest(theTest);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Test GetTest(string code)
        {
            return dal.GetTest(code);
        }

        public List<Test> GetTests(Predicate<Test> predicate = null)
        {
            return dal.GetTests(predicate);
        }

        public List<Test> FindAllInTests(Predicate<Test> condition)
        {
            return (from test in dal.GetTests()
                    where condition(test)
                    select test)
                        .ToList();
        }

        public List<Test> SortedFutureTests()
        {
            return (from test in dal.GetTests()
                    where !test.IsDone()
                    orderby test.TestDate
                    select test)
                        .ToList();
        }

        #endregion


        #region private functions

        private bool ExistingTesterById(string id)
        {
            return dal.GetTester(id) != null;
        }

        private bool ExistingTraineeById(string id)
        {
            return dal.GetTrainee(id) != null;
        }

        private bool ExistingTestByCode(string code)
        {
            return dal.GetTest(code) != null;
        }


        private bool IsEntitledToLicense(Test.Criteria criteria) // NOTE: better to send (Test test)
        {
            return criteria.PassGrades() >= Configuration.MIN_CRITERIONS_TO_PASS_TEST;
        }

        private Tester FindingAvailableTester(DateTime dateTime, Vehicle expertise) //UNDONE improve
        {
            DayOfWeek theDayInTheWeek = dateTime.DayOfWeek; //CHECK if dateTime.DayOfWeek is short time
            DateTime theFirstDayInTheWeek = dateTime.Date.AddDays(-1 * (int)theDayInTheWeek);

            #region inner method
            IEnumerable<(bool HasAlreadyTest, uint CounterOfTheTestInTheWeek)> WillAvailable(Tester tester)
            {
                uint counterOfTheTestInTheWeek = 0;
                foreach (Test test in tester.MyTests)
                {
                    //if (theFirstDayInTheWeek == test.TestDate.Date.AddDays(-1 * (int)calendar.GetDayOfWeek(test.TestDate))) //if (IsDateAreInTheSameWeek(test.TestDate))
                    if (theFirstDayInTheWeek == test.TestDate.Date.AddDays(-1 * (int)test.TestDate.DayOfWeek)) //if (IsDateAreInTheSameWeek(test.TestDate))
                        ++counterOfTheTestInTheWeek;
                    yield return (test.TestDate == dateTime, counterOfTheTestInTheWeek);
                }
            }
            #endregion

            foreach (Tester tester in dal.GetTesters(t => t.VehicleTypeExpertise == expertise && t.WorkingHours[(int)theDayInTheWeek, dateTime.Hour - 9]))
            {
                // CHECK hour is not 00:00 for example

                bool isAvailable = true;
                foreach (var (HasAlreadyTest, CounterOfTheTestInTheWeek) in WillAvailable(tester))
                    if (HasAlreadyTest == true || CounterOfTheTestInTheWeek >= tester.MaxOfTestsPerWeek)
                    {
                        isAvailable = false;
                        break;
                    }

                if (isAvailable) //CHECK if iterator have pass on all the element
                    return tester;
            }

            return null;


            // CHECK לבדוק שהיום לא מתחלף באמצע - כבר לא נדרש
            //#if [is in method] DAITSW(DT){GDOW==dayOfWeek)
            //bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
            //{
            //    return date1.Date.AddDays(-1 * (int)calendar.GetDayOfWeek(date1)) == date2.Date.AddDays(-1 * (int)calendar.GetDayOfWeek(date2));
            //}
            //bool IsDateAreInTheSameWeek(DateTime date)
            //{
            //    return theFirstDayInTheWeek == date.Date.AddDays(-1 * (int)calendar.GetDayOfWeek(date));
            //}
        }


        private (DateTime, Tester)? FindingAnAlternativeTest(Vehicle vehicle)
        {
            DateTime dateTime = DateTime.Today.AddDays(1).AddHours(9),
                aPeriodFromToday = dateTime.AddMonths(3); // IMPROVEMENT convert to config

            while (dateTime < aPeriodFromToday)
            {
                for (int i = 0; i < Configuration.WORKING_HOURS_A_DAY; i++)
                {
                    Tester tester = FindingAvailableTester(dateTime, vehicle);
                    if (tester != null)
                        return (dateTime, tester);
                    dateTime.AddHours(1);
                }

                dateTime.AddHours(-(Configuration.WORKING_HOURS_A_DAY + 1)); //-c-1
                dateTime.AddDays(dateTime.DayOfWeek != DayOfWeek.Friday ? 1 : 7 - Configuration.WORKING_DAYS_A_WEEK);
            }

            return null;
        }
        #endregion
    }
}

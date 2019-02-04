//Bs"d

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using static BE.Configuration;

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
                throw new CustomException(true, new ArgumentException("This tester already exists in the database.", nameof(tester)));

            TimeSpan testerAge = DateTime.Today - tester.Birthdate;
            if (testerAge < Configuration.MIN_AGE_OF_TESTER || testerAge > Configuration.MAX_AGE_OF_TESTER)
                throw new CustomException(true, new ArgumentOutOfRangeException(nameof(Tester.Birthdate), "The tester's age is not appropriate"));

            if (tester.YearsOfExperience > tester.AgeInYears - (Configuration.MIN_AGE_OF_TESTER.Days / 365))
                throw new Exception("Years of experience do not make sense according to age");

            if (0 == tester.MaxOfTestsPerWeek)
                throw new CustomException(true, new ArgumentOutOfRangeException("It is illegal for the teter to not test"));

            if (tester.WorkingHours.GetLength(0) != Configuration.WORKING_DAYS_A_WEEK
                    || tester.WorkingHours.GetLength(1) != Configuration.WORKING_HOURS_A_DAY)
                throw new Exception("The schedule is not right");

            if (tester.MaxDistanceFromAddress == 0)
                throw new ArgumentOutOfRangeException("Max Distance From Address can not get 0");

            try
            {
                dal.AddTester(tester);
            }
            catch
            {
                throw;
            }
        }

        public void RemoveTester(Tester tester)
        {
            if (tester is null)
                throw new CustomException(true, new ArgumentException("This tester doesn't exist in the database.", nameof(tester)));

            if (tester.MyTests?.Any(test => test.IsPass != null) ?? false)
                throw new CustomException(true, new Exception("This tester have future tests."));

            try
            {
                dal.RemoveTester(tester);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateTester(Tester tester)
        {
            Inspections.TesterInspection(tester);

            if (!ExistingTesterById(tester.ID))
                throw new CustomException(true, new ArgumentException("This tester doesn't exist in the database.", nameof(tester)));

            try
            {
                /// BetterOption (because inputcheck)
                /// this.Removetester
                /// this.AddTester
                dal.UpdateTester(tester);
            }
            catch
            {
                throw;
            }
        }

        public Tester GetTester(string id)
        {
            return dal.GetTester(id);
        }

        public List<Tester> GetTesters(Predicate<Tester> match = null)
        {
            return dal.GetTesters(match);
        }

        public List<Tester> TheTestersWhoLiveInTheDistance(Address address)
        {
            return (from tester in dal.GetTesters()
                    where Configuration.rand.Next(1, 40) < Configuration.X
                    select tester)
                          .ToList();
        }

        public List<Tester> VacantTesters(DateTime dateAndTime) //BUG inputcheck IMPROVEMENT Vehicle
        {

            DayOfWeek theDayInTheWeek = dateAndTime.DayOfWeek; //CHECK if dateTime.DayOfWeek is short time
            DateTime theFirstDayInTheWeek = dateAndTime.Date.AddDays(-1 * (int)theDayInTheWeek);

            bool WillAvailable(Tester tester)
            {
                uint counterOfTheTestInTheWeek = 0;
                foreach (Test test in tester.MyTests)
                {
                    if (test.Date == dateAndTime || counterOfTheTestInTheWeek >= tester.MaxOfTestsPerWeek)
                        break;

                    if (theFirstDayInTheWeek == test.Date.Date.AddDays(-1 * (int)test.Date.DayOfWeek)) //if (IsDateAreInTheSameWeek(test.TestDate))
                        ++counterOfTheTestInTheWeek;
                }
                return counterOfTheTestInTheWeek < tester.MaxOfTestsPerWeek;
            }


            return (from tester in dal.GetTesters(t => t.WorkingHours[(uint)theDayInTheWeek, dateAndTime.Hour - Configuration.BEGINNING_OF_A_WORKING_DAY])
                    where WillAvailable(tester)
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
                throw new CustomException(true, new ArgumentOutOfRangeException(nameof(trainee.Birthdate), "This Trainee's age is not appropriate."));

            if (ExistingTraineeById(trainee.ID))
                throw new CustomException(true, new ArgumentException("This trainee already exists in the database.", nameof(trainee.ID)));

            try
            {
                dal.AddTrainee(trainee);
            }
            catch
            {
                throw;
            }
        }

        public void RemoveTrainee(Trainee trainee)
        {
            if (trainee is null)
                throw new CustomException(true, new Exception("This Trainee doesn't exist in the database."));

            try
            {
                dal.RemoveTrainee(trainee);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateTrainee(Trainee trainee)
        {
            Inspections.TraineeInspection(trainee);
            if (!ExistingTraineeById(trainee.ID))
                throw new CustomException(true, new ArgumentException("This trainee doesn't exist in the database."));

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

        public List<Trainee> GetTrainees(Predicate<Trainee> match = null)
        {
            return dal.GetTrainees(match);
        }

        public uint NumberOfDoneTests(Trainee trainee)
        {
            if (!ExistingTraineeById(trainee?.ID))
                throw new ArgumentException("This trainee doesn't exist in the database");

            return (uint)dal.GetTests(test => test.TraineeID == trainee.ID && test.IsDone()).Count;
        }

        public bool IsEntitledToLicense(Trainee trainee)
        {
            return dal.GetTests(test => test.TraineeID == trainee.ID && /*it is IMPROVEMENT because now it's always true,CHECK maybe opposite?*/ test.Vehicle.HasFlag(trainee.VehicleTypeTraining) && test.IsPass == true).Count == 1;
        }

        public List<IGrouping<string, Trainee>> TraineesByDrivingSchool(bool toSort = false)
        {
            List<Trainee> trainees = dal.GetTrainees();

            if (!toSort)
                return (from trainee in trainees
                        group trainee by trainee.DrivingSchool)
                        .ToList();

            //return (from trainee in trainees
            //        orderby trainee.TeacherName
            //        group trainee by trainee.DrivingSchool
            //        into tmp orderby tmp.Key)
            //            .ToList();



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
                    group trainee by (uint)GetTests().FindAll(t => t.TraineeID == trainee.ID).Count).ToList();
        }

        #endregion

        #region Test functions

        public DateTime? AddTest(Trainee trainee, DateTime testDate, /*DateTime length,*/ Address departureAddress, Vehicle vehicle)
        { // return bool is success, and get out HATZAA
            // BUG input check of vehicle (only one type)
            if (!ExistingTraineeById(trainee.ID))
                throw new ArgumentException("This Trainee doesn't exist in the database");

            if (DateTime.Now - trainee.TheLastTest < Configuration.TIME_RANGE_BETWEEN_TESTS)
                throw new ArgumentException("It is illegal to access to test less than 7 days after the last one");

            if (trainee.NumberOfDoneLessons < Configuration.MIN_LESSONS)
                throw new ArgumentException("It is illegal to access to test if you did less than " + Configuration.MIN_LESSONS + " lessons");

            if (dal.GetTests(t => t.TraineeID == trainee.ID && t.Date == testDate).IsNullOrEmpty() == false)
                throw new ArgumentException("It is illegal to set for a trainee two tests at the same time");

            if (IsEntitledToLicense(trainee))
                throw new ArgumentException("It is illegal for a trainee to take the test he has succeeded in, once more");

            if (dal.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle.HasFlag(vehicle) && t.IsDone() == false).IsNullOrEmpty() == false)
                throw new ArgumentException("It is illegal for a trainee to set two tests for the same vehicle when he has not yet performed the first");

            if (trainee.VehicleTypeTraining.HasFlag(vehicle))
                throw new ArgumentException("It is illegal for a trainee to take a test on a vehicle he has not learned to drive");

            if (testDate.DayOfWeek == DayOfWeek.Friday || testDate.DayOfWeek == DayOfWeek.Saturday || testDate.Hour > 15 || testDate.Hour < 9)
                throw new ArgumentException("The requested time exceeds the working hours of the testers");
            //BUG
            //if (testDate < DateTime.Now)
            //    throw new ArgumentException("The requested time has passed");

            try
            {
                //TheTestersWhoLiveInTheDistance(departureAddress).Intersect(VacantTesters(testDate).Where(t => t.VehicleTypeExpertise.HasFlag(vehicle)))
                Tester tester = VacantTesters(testDate).Where(t => t.VehicleTypeExpertise.HasFlag(vehicle)).FirstOrDefault(); //IMPROVEMENT rand index

                if (tester != default)
                {
                    dal.AddTest(new Test(tester.ID, trainee.ID, testDate, /*length,*/ departureAddress, vehicle));
                    return null;
                }

                //foreach (var alternativeDate in FindingAnAlternativeDateForTest(testDate, vehicle))
                //{
                //
                //}
                return FindingAnAlternativeDateForTest(testDate, vehicle)?.Item1;
                //return new DateTime(2019, 1, 28, 10, 0, 0);
            }
            catch (Exception e)
            {
                throw e;
            }

            //throw new Exception("There is no available Tester on the requested date");

        }

        public void UpdateTest(string code, Test.Criteria criteria, bool isPass, string testerNotes)
        {
            //if (!dal.GetTest(test.Code).Exists(t => t.IDTrainee == trainee.ID && t.Vehicle.HasFlag(vehicle) && !t.IsDone()))
            Test theTest = dal.GetTest(code);
            if (theTest == null)
                throw new ArgumentException("This test doesn't exist in the database");

            if (theTest.IsPass != null)
                throw new Exception("Cannot update updated test");

            if (isPass && !HasPassed(criteria))
                throw new ArgumentException("It is illegal to pass a test if the trainee does not pass through more than " + Configuration.MIN_CRITERIONS_TO_PASS_TEST + " Cartierians.");

            if (testerNotes == null || testerNotes == "")
                throw new ArgumentException("You must enter a test note");

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

        public List<Test> GetTests(Predicate<Test> match = null)
        {
            return dal.GetTests(match);
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
                    orderby test.Date
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


        private bool HasPassed(Test.Criteria criteria) // NOTE: better to send (Test test)
        {
            return criteria.PassGrades() >= Configuration.MIN_CRITERIONS_TO_PASS_TEST;
        }

        /*
        private Tester FindingAvailableTester(DateTime dateTime, Vehicle expertise) //UNDONE improve
        {
            DayOfWeek theDayInTheWeek = dateTime.DayOfWeek; //CHECK if dateTime.DayOfWeek is short time
            DateTime theFirstDayInTheWeek = dateTime.Date.AddDays(-1 * (int)theDayInTheWeek);

            //#region inner iteratror
            //IEnumerable<(bool HasAlreadyTest, uint CounterOfTheTestInTheWeek)> WillAvailable(Tester tester)// BUG You can set two tests for the same day of the same hour with the same Tester!!!
            //{
            //    uint counterOfTheTestInTheWeek = 0;
            //    foreach (Test test in tester.MyTests)
            //    {
            //        //if (theFirstDayInTheWeek == test.TestDate.Date.AddDays(-1 * (int)calendar.GetDayOfWeek(test.TestDate))) //if (IsDateAreInTheSameWeek(test.TestDate))
            //        if (theFirstDayInTheWeek == test.Date.Date.AddDays(-1 * (int)test.Date.DayOfWeek)) //if (IsDateAreInTheSameWeek(test.TestDate))
            //            ++counterOfTheTestInTheWeek;
            //        yield return (test.Date == dateTime, counterOfTheTestInTheWeek);
            //    }
            //}
            //#endregion

            foreach (Tester tester in dal.GetTesters(t => t.VehicleTypeExpertise == expertise && t.WorkingHours[(int)theDayInTheWeek, dateTime.Hour - Configuration.BEGINNING_OF_A_WORKING_DAY]))
            {
                // CHECK hour is not 00:00 for example
                uint counterOfTheTestInTheWeek = 0;
                foreach (Test test in tester.MyTests)
                {
                    if (test.Date == dateTime || counterOfTheTestInTheWeek >= tester.MaxOfTestsPerWeek) break;

                    if (theFirstDayInTheWeek == test.Date.Date.AddDays(-1 * (int)test.Date.DayOfWeek)) //if (IsDateAreInTheSameWeek(test.TestDate))
                        ++counterOfTheTestInTheWeek;
                }
                if (counterOfTheTestInTheWeek < tester.MaxOfTestsPerWeek) return tester;



                // bool isAvailable = true;
                // foreach (var (HasAlreadyTest, CounterOfTheTestInTheWeek) in WillAvailable(tester))
                //     if (HasAlreadyTest == true || CounterOfTheTestInTheWeek >= tester.MaxOfTestsPerWeek)
                //     {
                //         isAvailable = false;
                //         break;
                //     }
                //
                // if (isAvailable) //CHECK if iterator have pass on all the element
                //     return tester;
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
        */


        //private IEnumerable<(DateTime, Tester)?> FindingAnAlternativeDateForTest(DateTime startDate, Vehicle vehicle)
        private (DateTime, Tester)? FindingAnAlternativeDateForTest(DateTime startDate, Vehicle vehicleTypeLearning) // BUG input check of date and vehicle
        {
            DateTime dateTime = startDate,//startDate.AddDays(1).AddHours(9),
                aPeriodFromToday = dateTime.AddMonths(1); // IMPROVEMENT convert to config

            while (dateTime < aPeriodFromToday)
            {
                while (dateTime.Hour < Configuration.WORKING_HOURS_A_DAY + Configuration.BEGINNING_OF_A_WORKING_DAY)
                {
                    Tester vacantTester = VacantTesters(dateTime).Where(t => t.VehicleTypeExpertise.HasFlag(vehicleTypeLearning)).FirstOrDefault();
                    if (vacantTester != default)
                        /*yield*/
                        return (dateTime, vacantTester);
                    dateTime = dateTime.AddHours(1);
                }

                dateTime = dateTime.AddHours(-Configuration.WORKING_HOURS_A_DAY);
                dateTime = dateTime.AddDays(dateTime.DayOfWeek != DayOfWeek.Friday ? 1 : 7 - Configuration.WORKING_DAYS_A_WEEK);
            }

            return null;
        }
        #endregion

        public Tester FindsAnAlternativeTester(Test test)
        {
            return dal.GetTesters(tester => tester.ID != test.TesterID && tester.MyTests.All(t => t.Date != test.Date)).FirstOrDefault();
        }
    }
}

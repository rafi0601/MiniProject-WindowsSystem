//Bs"d

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BE;
using static BE.Configuration;

namespace BL
{
    public partial class Bl_imp : IBL
    {

        private readonly DAL.IDal dal = DAL.Singleton.Instance;


        #region Tester functions

        public void AddTester(Tester tester)
        {
            Inspections.TesterInspection(tester);

            if (ExistingTesterById(tester.ID))
                throw new CustomException(true, new ArgumentException("This tester already exists in the database."));

            TimeSpan testerAge = DateTime.Today - tester.Birthdate;
            if (testerAge < MIN_AGE_OF_TESTER || testerAge > MAX_AGE_OF_TESTER)
                throw new CustomException(true, new ArgumentOutOfRangeException("The tester's age is not appropriate"));

            if (tester.YearsOfExperience > tester.AgeInYears - (MIN_AGE_OF_TESTER.Days / 365))// check minageoftrainee?
                throw new Exception("Years of experience do not make sense according to age");

            if (0 == tester.MaxOfTestsPerWeek)
                throw new CustomException(true, new ArgumentOutOfRangeException("It is illegal for the teter to not test"));

            if (tester.workingHours == null) throw new Exception();
            //if (tester.WorkingHours.GetLength(0) != WORKING_DAYS_A_WEEK
            //        || tester.WorkingHours.GetLength(1) != WORKING_HOURS_A_DAY)
            //    throw new Exception("The schedule is not right");

            if (tester.MaxDistanceFromAddress == 0)
                throw new ArgumentOutOfRangeException("Max Distance From Address can not get 0");

            try
            {
                dal.AddTester(tester);
            }
            catch (ExistingInTheDatabaseException e)
            {
                throw new CustomException(true, e);
            }
            catch (Exception e)
            {
                throw new CustomException(true, e);
            }
        }

        public void RemoveTester(Tester tester)
        {
            if (tester is null)
                throw new CustomException(true, new ArgumentException("This tester doesn't exist in the database."));

            //if (tester.MyTests?.Any(test => test.IsPass != null) ?? false)
            if (GetTests(test => test.TesterID == tester.ID && test.IsPass == null).Any())
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
                throw new CustomException(true, new ArgumentException("This tester doesn't exist in the database."));

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
                    where DistanceAndTime(tester.Address, address).distance < tester.MaxDistanceFromAddress
                    select tester)
                          .ToList();
        }

        private (uint distance, TimeSpan time) DistanceAndTime(Address address1, Address address2)
        {
            (uint, TimeSpan)? t = null;

            BackgroundWorker requester = new BackgroundWorker();
            requester.DoWork += Requester_DoWork;
            void Requester_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                if (e.Result is ValueTuple<uint, TimeSpan> vt)
                    t = vt;
                //else if (e.Result is Exception ex)
                //    exception = ex;
                else
                    t = (DEFAULT_DISTANCE, new TimeSpan());
            }
            requester.RunWorkerCompleted += Requester_RunWorkerCompleted;

            while (t == null)//!t.HasValue)
                if (!requester.IsBusy)
                    requester.RunWorkerAsync((address1, address2));

            return t.Value;

            //if (requester.WorkerSupportsCancellation)
            //    requester.CancelAsync();
            //requester.WorkerReportsProgress = true;
            //requester.WorkerSupportsCancellation = true; //??
            //requester.CreateObjRef(typeof((uint, TimeSpan))).
        }

        private void Requester_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            (Address address1, Address address2) = ((Address, Address))e.Argument;

            string origin = address1.Street + " " + address1.HouseNumber + " " + address1.City + " Israel";
            string destination = address2.Street + " " + address2.HouseNumber + " " + address2.City + " Israel";

            string url = @"https://www.mapquestapi.com/directions/v2/route" +
                @"?key=" + KEY +
                @"&from=" + origin +
                @"&to=" + destination +
                @"&outFormat=xml" +
                @"&ambiguities=ignore&routeType=fastest&doReverseGeocode=false" +
                @"&enhancedNarrative=false&avoidTimedConditions=false";

            //request from MapQuest service the distance between the 2 addresses
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            //the response is given in an XML format 
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(responsereader);

            if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "0") //we have the expected answer
            {     //display the returned distance
                XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                double distInMiles = Convert.ToDouble(distance[0].ChildNodes[0].InnerText);
                Debug.WriteLine("Distance In KM: " + distInMiles * 1.609344);

                //display the returned driving time   
                XmlNodeList formattedTime = xmldoc.GetElementsByTagName("formattedTime");
                string fTime = formattedTime[0].ChildNodes[0].InnerText;
                Debug.WriteLine("Driving Time: " + fTime);

                e.Result = (distInMiles * 1.609344, fTime);
            }
            else if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "402")
            //we have an answer that an error occurred, one of the addresses is not found 
            {
                //e.Cancel = true;
                e.Result = null;
                Debug.WriteLine("an error occurred, one of the addresses is not found. try again.");
            }
            else //busy network or other error... 
            {
                Console.WriteLine("We have'nt got an answer, maybe the net is busy...");
                //Thread.Sleep(2000);
                e.Result = null;
            }
        }

        public List<Tester> VacantTesters(DateTime dateAndTime) //BUG inputcheck IMPROVEMENT Vehicle
        {

            DayOfWeek theDayInTheWeek = dateAndTime.DayOfWeek; //CHECK if dateTime.DayOfWeek is short time
            DateTime theFirstDayInTheWeek = dateAndTime.Date.AddDays(-1 * (int)theDayInTheWeek);

            bool WillAvailable(Tester tester)
            {
                uint counterOfTheTestInTheWeek = 0;
                foreach (DateTime testDateTime in tester.UnavailableDates)
                {
                    if (testDateTime == dateAndTime || counterOfTheTestInTheWeek >= tester.MaxOfTestsPerWeek)
                        return false;

                    if (theFirstDayInTheWeek == testDateTime.AddDays(-1 * (int)testDateTime.DayOfWeek)) //if (IsDateAreInTheSameWeek(test.TestDate))
                        ++counterOfTheTestInTheWeek;
                }
                return counterOfTheTestInTheWeek < tester.MaxOfTestsPerWeek;
            }


            return (from tester in dal.GetTesters(t => t.WorkingHours[(int)theDayInTheWeek, (int)(dateAndTime.Hour - BEGINNING_OF_A_WORKING_DAY)])
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
                    group tester by tester.VehicleTypesExpertise)
                    .ToList();
        }

        #endregion

        #region Trainee functions

        public void AddTrainee(Trainee trainee)
        {
            Inspections.TraineeInspection(trainee);

            if (DateTime.Today - trainee.Birthdate < MIN_AGE_OF_TRAINEE)
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

        public List<IGrouping<Person.PersonName, Trainee>> TraineesByTeacher(bool toSort = false)
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

        public DateTime? AddTest(Trainee trainee, DateTime testDate, Address departureAddress, Vehicle vehicle)
        { // return bool is success, and get out HATZAA
            if (!vehicle.IsFlag())
                throw new Exception();

            if (!ExistingTraineeById(trainee.ID))
                throw new ArgumentException("This Trainee doesn't exist in the database");

            if (DateTime.Now - trainee.TheLastTest < TIME_RANGE_BETWEEN_TESTS)
                throw new ArgumentException("It is illegal to access to test less than 7 days after the last one");

            if (trainee.NumberOfDoneLessons < MIN_LESSONS)
                throw new ArgumentException("It is illegal to access to test if you did less than " + MIN_LESSONS + " lessons");

            if (dal.GetTests(t => t.TraineeID == trainee.ID && t.Date == testDate).Any() == true)
                throw new ArgumentException("It is illegal to set for a trainee two tests at the same time");

            if (IsEntitledToLicense(trainee))
                throw new ArgumentException("It is illegal for a trainee to take the test he has succeeded in, once more");

            if (dal.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle.HasFlag(vehicle) && t.IsDone() == false).Any() == true)
                throw new ArgumentException("It is illegal for a trainee to set two tests for the same vehicle when he has not yet performed the first");

            if (!trainee.VehicleTypeTraining.HasFlag(vehicle))
                throw new ArgumentException("It is illegal for a trainee to take a test on a vehicle he has not learned to drive");

            if (testDate.DayOfWeek == DayOfWeek.Friday || testDate.DayOfWeek == DayOfWeek.Saturday || testDate.Hour > 15 || testDate.Hour < 9)
                throw new ArgumentException("The requested time exceeds the working hours of the testers");
            //BUG
            //if (testDate < DateTime.Now)
            //    throw new ArgumentException("The requested time has passed");

            try
            {
                Tester tester = VacantTesters(testDate).Where(t => t.VehicleTypesExpertise.HasFlag(vehicle)).Intersect(TheTestersWhoLiveInTheDistance(departureAddress)).FirstOrDefault(); //IMPROVEMENT rand index

                if (tester != default)
                {
                    dal.AddTest(new Test(tester.ID, trainee.ID, testDate, departureAddress, vehicle));
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
                throw new ArgumentException("It is illegal to pass a test if the trainee does not pass through more than " + MIN_CRITERIONS_TO_PASS_TEST + " Cartierians.");

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
            return criteria.PassGrades() >= MIN_CRITERIONS_TO_PASS_TEST;
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

            foreach (Tester tester in dal.GetTesters(t => t.VehicleTypeExpertise == expertise && t.WorkingHours[(int)theDayInTheWeek, dateTime.Hour - BEGINNING_OF_A_WORKING_DAY]))
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
                while (dateTime.Hour < WORKING_HOURS_A_DAY + BEGINNING_OF_A_WORKING_DAY)
                {
                    Tester vacantTester = VacantTesters(dateTime).Where(t => t.VehicleTypesExpertise.HasFlag(vehicleTypeLearning)).FirstOrDefault();
                    if (vacantTester != default)
                        /*yield*/
                        return (dateTime, vacantTester);
                    dateTime = dateTime.AddHours(1);
                }

                dateTime = dateTime.AddHours(-WORKING_HOURS_A_DAY);
                dateTime = dateTime.AddDays(dateTime.DayOfWeek != DayOfWeek.Thursday ? 1 : 7 - WORKING_DAYS_A_WEEK + 1);
            }

            return null;
        }
        #endregion

        public Tester FindsAnAlternativeTester(Test test)
        {
            return dal.GetTesters(tester => tester.ID != test.TesterID && tester.UnavailableDates.All(t => t.Date != test.Date)).FirstOrDefault();
        }

        public IEnumerable<Person> GetPeople()
        {
            IEnumerable<Person> result1 = from p in dal.GetTrainees(null)
                                           select p;
            IEnumerable<Person> result2 = from p in dal.GetTesters()
                                           select p;
            return result1.Concat(result2);

            //List<Person> list = new List<Person>();

            //list.AddRange(GetTrainees());
            //list.AddRange(GetTesters());

            //return list;
        }
    }
}

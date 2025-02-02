﻿//Bs"d

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

        private protected readonly DAL.IDal dal = DAL.FactorySingleton.Instance;

        #region Tester functions

        public void AddTester(Tester tester)
        {
            TesterLogicsInspection(tester);

            if (ExistingTesterById(tester.ID))
                throw new CasingException(true, new Exception("This tester already exists in the database."));
            if (ExistingTraineeById(tester.ID))
                throw new CasingException(true, new Exception("A trainee cannot be a tester."));

            try
            {
                dal.AddTester(tester);
            }
            catch (IOException e)
            {
                throw new CasingException(false, e);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
            }
        }

        public void RemoveTester(Tester tester)
        {
            if (tester is null)
                throw new CasingException(true, new ArgumentNullException("Cannot remove null."));

            if (dal.GetTests(test => test.TesterID == tester.ID && test.IsPass == null).Any())
                throw new CasingException(true, new Exception("The tester have future tests."));

            try
            {
                dal.RemoveTester(tester);
            }
            catch (IOException e)
            {
                throw new CasingException(false, e);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
            }
        }

        public void UpdateTester(Tester tester)
        {
            TesterLogicsInspection(tester);

            if (!ExistingTesterById(tester.ID))
                throw new CasingException(true, new ArgumentException("This tester doesn't exist in the database."));

            try
            {
                dal.UpdateTester(tester);
            }
            catch (IOException e)
            {
                throw new CasingException(false, e);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
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
            try
            {
                Inspections.AddressValidator(address);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
            }

            return (from tester in dal.GetTesters()
                    where DistanceAndTime(tester.Address, address).distance < tester.MaxDistanceFromAddress
                    select tester)
                          .ToList();
        }

        public List<Tester> VacantTesters(DateTime dateAndTime)
        {
            bool WillAvailable(Tester tester)
            {
                DateTime theFirstDayInTheWeek = dateAndTime.Date.AddDays(-1 * (int)dateAndTime.DayOfWeek);

                uint counterOfTheTestInTheWeek = 0;
                foreach (DateTime unavailableDate in tester.UnavailableDates)
                {
                    if (unavailableDate == dateAndTime || counterOfTheTestInTheWeek >= tester.MaxOfTestsPerWeek)
                        return false;

                    if (theFirstDayInTheWeek == unavailableDate.Date.AddDays(-1 * (int)unavailableDate.DayOfWeek)) //if (IsDateAreInTheSameWeek(test.TestDate))
                        ++counterOfTheTestInTheWeek;
                }
                return counterOfTheTestInTheWeek < tester.MaxOfTestsPerWeek;
            }

            if ((int)dateAndTime.DayOfWeek >= WORKING_DAYS_A_WEEK ||
                dateAndTime.Hour < BEGINNING_OF_A_WORKING_DAY || dateAndTime.Hour >= BEGINNING_OF_A_WORKING_DAY + WORKING_HOURS_A_DAY)
                throw new ArgumentOutOfRangeException("The date is not in the working hours");

            return (from tester in dal.GetTesters(t => t.WorkingHours[dateAndTime.DayOfWeek, (int)dateAndTime.Hour])
                    where WillAvailable(tester)
                    select tester)
                    .ToList();
        }

        #endregion

        #region Trainee functions

        public void AddTrainee(Trainee trainee)
        {
            TraineeLogicsInspection(trainee);

            if (ExistingTraineeById(trainee.ID))
                throw new CasingException(true, new ArgumentException("This trainee already exists in the database."));
            if (ExistingTesterById(trainee.ID))
                throw new CasingException(true, new Exception("A tester cannot be a trainee."));

            try
            {
                dal.AddTrainee(trainee);
            }
            catch (IOException e)
            {
                throw new CasingException(false, e);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
            }
        }

        public void RemoveTrainee(Trainee trainee)
        {
            if (trainee is null)
                throw new CasingException(true, new ArgumentNullException("Cannot remove null."));

            if (dal.GetTests(test => test.TraineeID == trainee.ID && !test.IsDone()).Any())
                throw new CasingException(true, new Exception("The trainee have future tests."));

            try
            {
                dal.RemoveTrainee(trainee);
            }
            catch (IOException e)
            {
                throw new CasingException(false, e);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
            }
        }

        public void UpdateTrainee(Trainee trainee)
        {
            TraineeLogicsInspection(trainee);

            if (!ExistingTraineeById(trainee.ID))
                throw new CasingException(true, new ArgumentException("This trainee doesn't exist in the database."));

            try
            {
                dal.UpdateTrainee(trainee);
            }
            catch (IOException e)
            {
                throw new CasingException(false, e);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
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
                throw new CasingException(true, new ArgumentException("This trainee doesn't exist in the database"));

            return (uint)dal.GetTests(test => test.TraineeID == trainee.ID && test.IsDone()).Count;
        }

        public bool IsEntitledToLicense(Trainee trainee)
        {
            if (!ExistingTraineeById(trainee?.ID))
                throw new CasingException(true, new ArgumentException("This trainee doesn't exist in the database"));

            return dal.GetTests(test => test.TraineeID == trainee.ID && test.IsPass == true).Any();
        }

        #endregion

        #region Test functions

        public DateTime? AddTest(Trainee trainee, DateTime testDate, Address departureAddress, Vehicle vehicle)
        {
            //TODO if !trainee.Equal(dal.GetTrainee())

            if (!ExistingTraineeById(trainee.ID))
                throw new CasingException(true, new ArgumentException("This Trainee doesn't exist in the database"));

            if (trainee.NumberOfDoneLessons < MIN_LESSONS_TO_ORDER_TEST)
                throw new CasingException(true, new ArgumentException("It is illegal to access to test if you did less than " + MIN_LESSONS_TO_ORDER_TEST + " lessons"));

            if (!vehicle.IsFlag())
                throw new CasingException(false, new Exception("Test can be only on one vehicle."));

            //BUG only for debugging
            //if (testDate < DateTime.Now)//Now+LengthOfTest
            //    throw new ArgumentException("The requested time has passed");

            if (testDate.DayOfWeek < BEGINNING_OF_A_WORKING_WEEK || testDate.DayOfWeek > END_OF_A_WORKING_WEEK || testDate.Hour > END_OF_A_WORKING_DAY || testDate.Hour < BEGINNING_OF_A_WORKING_DAY)
                throw new CasingException(true, new ArgumentException("The requested time exceeds the working hours of the testers: " + $"{BEGINNING_OF_A_WORKING_WEEK}-{END_OF_A_WORKING_WEEK}, {BEGINNING_OF_A_WORKING_DAY}-{END_OF_A_WORKING_DAY}"));

            if (!trainee.VehicleTypeTraining.HasFlag(vehicle))
                throw new CasingException(true, new ArgumentException("It is illegal for a trainee to take a test on a vehicle he has not learned to drive"));

            List<Test> testsOfTheTrainee = dal.GetTests(test => test.TraineeID == trainee.ID);

            if (testsOfTheTrainee.Any(test => !test.IsDone())) // CHECK maybe not necccary (in IsEntitled)
                throw new CasingException(true, new ArgumentException("It is illegal for a trainee to set two tests for the same vehicle when he has not yet performed the first"));

            if (IsEntitledToLicense(trainee))
                throw new CasingException(true, new ArgumentException("It is illegal for a trainee to take the test he has succeeded in, once more"));

            if (testsOfTheTrainee.Any(test => test.Date == testDate)) // CHECK maybe not neccary (in the next one)
                throw new CasingException(true, new ArgumentException("It is illegal to set for a trainee two tests at the same time"));

            if (testDate - trainee.TheLastTest < TIME_RANGE_BETWEEN_TESTS)
                throw new CasingException(true, new ArgumentException($"It is illegal to access to test less than {TIME_RANGE_BETWEEN_TESTS} after the last one"));


            try
            {
                Tester tester = SearchForTester(testDate, departureAddress, vehicle);

                if (tester != null)
                {
                    dal.AddTest(new Test(tester.ID, trainee.ID, testDate, departureAddress, vehicle));
                    trainee.TheLastTest = testDate;
                    dal.UpdateTrainee(trainee);
                    tester.UnavailableDates.Add(testDate);
                    dal.UpdateTester(tester);
                    return null;
                }

                //foreach (var alternativeDate in FindingAnAlternativeDateForTest(testDate, vehicle)) // IMPROVEMENT
                //{
                //    yield return alternativeDate;
                //}
                return FindingAnAlternativeDateForTest(testDate, departureAddress, vehicle)?.Item1 ?? throw new Exception("There is no test available in the near future from the chosen date");
            }
            catch (OverflowException e)
            {
                throw new CasingException(false, e);
            }
            catch (IOException e)
            {
                throw new CasingException(false, e);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
            }
        }

        public void UpdateTest(string code, Test.Criteria criteria, bool isPass, string testerNotes)
        {
            Test theTest = dal.GetTest(code);

            if (theTest == null)
                throw new CasingException(true, new ArgumentException("This test doesn't exist in the database"));

            if (!theTest.IsDone())
                throw new CasingException(true, new Exception("Cannot give grade for future test"));

            if (theTest.IsPass != null)
                throw new CasingException(true, new Exception("Cannot update updated test"));

            if (isPass && criteria.PassGrades() < MIN_CRITERIONS_TO_PASS_TEST)
                throw new CasingException(true, new ArgumentException("It is illegal to pass a test if the trainee does not pass through more than " + MIN_CRITERIONS_TO_PASS_TEST + " Cartierians."));

            if (string.IsNullOrWhiteSpace(testerNotes))
                throw new CasingException(true, new ArgumentException("A tester must enter a test note"));

            theTest.CriteriasGrades = criteria;
            theTest.IsPass = isPass;
            theTest.TesterNotes = testerNotes;

            try
            {
                dal.UpdateTest(theTest);
            }
            catch (IOException e)
            {
                throw new CasingException(false, e);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
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

        public List<Test> GetSortedFutureTests()
        {
            return (from test in dal.GetTests()
                    where !test.IsDone()
                    orderby test.Date
                    select test)
                        .ToList();
        }

        #endregion

        #region Grouping

        public List<IGrouping<Vehicle, Tester>> TestersByExpertise(bool toSort = false)
        {
            IEnumerable<IGrouping<Vehicle, Tester>> groups;

            if (!toSort)
                groups = from tester in dal.GetTesters()
                         group tester by tester.VehicleTypesExpertise;
            else
                groups = from tester in dal.GetTesters()
                         orderby tester.YearsOfExperience descending, tester.ID ascending
                         group tester by tester.VehicleTypesExpertise into @group
                         orderby @group.Key
                         select @group;

            return groups.ToList();
        }

        public List<IGrouping<string, Trainee>> TraineesByDrivingSchool(bool toSort = false)
        {
            //return (
            //    !toSort ? from trainee in dal.GetTrainees()
            //              group trainee by trainee.DrivingSchool
            //            :
            //              from trainee in dal.GetTrainees()
            //              orderby trainee.TeacherName, trainee.ID
            //              group trainee by trainee.DrivingSchool into @group
            //              orderby @group.Key
            //              select @group
            //        ).ToList();

            return Grouping(dal.GetTrainees(), trainee => trainee.DrivingSchool,
                toSort ? (Func<Trainee, Person.PersonName>)(trainee => trainee.TeacherName) : null).ToList();
        }

        public List<IGrouping<Person.PersonName, Trainee>> TraineesByTeacher(bool toSort = false)
        {
            //IEnumerable<IGrouping<Person.PersonName, Trainee>> groups;
            //
            //if (!toSort)
            //    groups = from trainee in dal.GetTrainees()
            //             group trainee by trainee.TeacherName;
            //else
            //    groups = from trainee in dal.GetTrainees()
            //             orderby trainee.Name, trainee.ID
            //             group trainee by trainee.TeacherName into @group
            //             orderby @group.Key
            //             select @group;
            //
            //return groups.ToList();

            return Grouping(dal.GetTrainees(), trainee => trainee.TeacherName,
                toSort ? (Func<Trainee, Person.PersonName>)(trainee => trainee.Name) : null).ToList();
        }

        public List<IGrouping<uint, Trainee>> TraineesByNumberOfTests(bool toSort = false)
        {
            IEnumerable<IGrouping<uint, Trainee>> groups;

            if (!toSort)
                groups = from trainee in dal.GetTrainees().AsParallel()
                         group trainee by NumberOfDoneTests(trainee);
            else
                groups = from trainee in dal.GetTrainees()//.AsParallel().AsOrdered()
                         orderby trainee.DrivingSchool, trainee.ID
                         group trainee by NumberOfDoneTests(trainee) into @group //IMPROVEMENT by ClassesTheNumbers(NumberOfDoneTests(trainee))
                         orderby @group.Key
                         select @group;

            return groups.ToList();

            char ClassesTheNumbers(uint numberOfDoneTests)
            {
                return numberOfDoneTests < 3 ? 'A' : numberOfDoneTests <= 5 ? 'B' : 'D';
            }
        }

        protected IEnumerable<IGrouping<Key, Type>> Grouping<Type, Key, Order>(IEnumerable<Type> elements, Func<Type, Key> groupBy, Func<Type, Order> sortBy = null) where Type : IKey
        {
            if (groupBy == null)
                throw new ArgumentNullException(nameof(groupBy));

            return sortBy != null ? from element in elements
                                    orderby sortBy(element), element.Key
                                    group element by groupBy(element) into @group
                                    orderby @group.Key
                                    select @group
                                    :
                                    from element in elements
                                    group element by groupBy(element);
        }

        #endregion

        #region Logics Inspections

        private protected void TesterLogicsInspection(Tester tester)
        {
            try
            {
                Inspections.TesterInspection(tester);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
            }

            TimeSpan testerAge = DateTime.Today - tester.Birthdate;
            if (testerAge < MIN_AGE_OF_TESTER || testerAge > MAX_AGE_OF_TESTER)
                throw new CasingException(true, new ArgumentException($"The tester's age is not appropriate ({(int)(MIN_AGE_OF_TESTER.Days / 365.25)} - {(int)(MAX_AGE_OF_TESTER.Days / 365.25)})."));

            if (tester.YearsOfExperience > tester.AgeInYears - (MIN_AGE_OF_TESTER.Days / 365.25))// CHECK minageoftrainee? (start to count years from learn or teach?)
                throw new CasingException(true, new ArgumentException($"Years of experience do not make sense according to age."));

            if (0 == tester.MaxOfTestsPerWeek)
                throw new CasingException(true, new ArgumentException("It is illegal for the teter to not test."));

            if (tester.MaxDistanceFromAddress == 0)
                throw new CasingException(true, new ArgumentException("Max Distance From Address can not get 0."));
        }

        private protected void TraineeLogicsInspection(Trainee trainee)
        {
            try
            {
                Inspections.TraineeInspection(trainee);
            }
            catch (Exception e)
            {
                throw new CasingException(true, e);
            }

            if (DateTime.Today - trainee.Birthdate < MIN_AGE_OF_TRAINEE)
                throw new CasingException(true, new ArgumentOutOfRangeException($"This Trainee's age is not appropriate (at least {(int)(MIN_AGE_OF_TRAINEE.Days / 365.25)})."));
        }

        private protected void TestLogicsInspections(Trainee trainee, DateTime testDate, Address departureAddress, Vehicle vehicle)
        {
            // In AddTest and UpdateTest
        }

        #endregion

        #region private functions
        // These are private function so all the input check were done before the call to the functions

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


        private Tester SearchForTester(DateTime testDate, Address departureAddress, Vehicle vehicle)
        {
            List<Tester> optionalTesters = (from testerInArea in TheTestersWhoLiveInTheDistance(departureAddress)//.AsParallel()
                                            where testerInArea.VehicleTypesExpertise.HasFlag(vehicle)
                                            join vacantTester in VacantTesters(testDate)/*.AsParallel()*/ on testerInArea.ID equals vacantTester.ID
                                            where vacantTester != null
                                            select testerInArea).ToList(); // Intersect between the lists.
            if (optionalTesters.Any())
                return optionalTesters[rand.Next(optionalTesters.Count)];
            return null;

            //return (from testerInArea in TheTestersWhoLiveInTheDistance(departureAddress)
            //        from vacantTester in VacantTesters(testDate)
            //        where vacantTester.VehicleTypesExpertise.HasFlag(vehicle) && vacantTester.ID == testerInArea.ID
            //        select vacantTester).ToList().FirstOrDefault();
        }


        //private IEnumerable<(DateTime, Tester)?> FindingAnAlternativeDateForTest(DateTime startDate, Vehicle vehicle)
        private (DateTime, Tester)? FindingAnAlternativeDateForTest(DateTime startDate, Address departureAddress, Vehicle vehicleTypeLearning)
        {
            if (!vehicleTypeLearning.IsFlag())
                throw new CasingException(true, new ArgumentException("A test can be only on one type of vehicle."));

            List<Tester> theTestersWhoLiveInTheDistance = TheTestersWhoLiveInTheDistance(departureAddress);

            DateTime dateTime = startDate.Date.AddHours(BEGINNING_OF_A_WORKING_DAY),
                aPeriodFromToday = dateTime + SPAN_FROM_TODAY_TO_SEARCH_TEST;

            while (dateTime < aPeriodFromToday)
            {
                while (dateTime.Hour < WORKING_HOURS_A_DAY + BEGINNING_OF_A_WORKING_DAY)
                {
                    List<Tester> optionalTesters = (from testerInArea in theTestersWhoLiveInTheDistance//.AsParallel()
                                                    where testerInArea.VehicleTypesExpertise.HasFlag(vehicleTypeLearning)
                                                    join vacantTester in VacantTesters(dateTime)/*.AsParallel()*/ on testerInArea.ID equals vacantTester.ID
                                                    where vacantTester != null
                                                    select testerInArea).ToList();

                    if (optionalTesters.Any())
                        return (dateTime, optionalTesters[rand.Next(optionalTesters.Count)]);
                    dateTime = dateTime.AddHours(1);
                }

                dateTime = dateTime.AddHours(-WORKING_HOURS_A_DAY);
                dateTime = dateTime.AddDays(dateTime.DayOfWeek != END_OF_A_WORKING_WEEK ? 1 : 7 - WORKING_DAYS_A_WEEK + 1);
            }

            return null;
        }

        private (double distance, TimeSpan time) DistanceAndTime(Address address1, Address address2)
        {
            string origin = address1.Street + " " + address1.HouseNumber + " " + address1.City + " Israel";
            string destination = address2.Street + " " + address2.HouseNumber + " " + address2.City + " Israel";

            string url = @"https://www.mapquestapi.com/directions/v2/route" +
                @"?key=" + KEY +
                @"&from=" + origin +
                @"&to=" + destination +
                @"&outFormat=xml" +
                @"&ambiguities=ignore&routeType=fastest&doReverseGeocode=false" +
                @"&enhancedNarrative=false&avoidTimedConditions=false";

            try
            {
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
                    TimeSpan fTime = TimeSpan.Parse(formattedTime[0].ChildNodes[0].InnerText);
                    Debug.WriteLine("Driving Time: " + fTime);

                    return (distInMiles * 1.609344, fTime);
                }
                else if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "402")
                //we have an answer that an error occurred, one of the addresses is not found 
                {
                    //e.Cancel = true;
                    Debug.WriteLine("an error occurred, one of the addresses is not found. try again.");
                    return (DEFAULT_DISTANCE, DEFAULT_TIME);
                }
                else //busy network or other error... 
                {
                    Debug.WriteLine("We have'nt got an answer, maybe the net is busy...");
                    Thread.Sleep(2000);
                    //return DistanceAndTime(address1, address2);
                    return (DEFAULT_DISTANCE, DEFAULT_TIME);
                }
            }
            catch (Exception ex)
            {
                throw new CasingException(false, new WebException("A broblem from Map Request. Look in the inner exception.", ex));
            }



            ///   BackgroundWorker requester = new BackgroundWorker();
            ///   requester.DoWork += Requester_DoWork;
            ///   //requester.RunWorkerCompleted += Requester_RunWorkerCompleted;
            ///   requester.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs e)
            ///   {
            ///       if (e.Result is uint vt)
            ///           t = vt;
            ///       //else if (e.Result is Exception ex)
            ///       //    exception = ex;
            ///       else
            ///           t = DEFAULT_DISTANCE;
            ///   };
            ///
            ///
            ///   //    while (t == null)//!t.HasValue)
            ///   //    if (!requester.IsBusy)
            ///   requester.RunWorkerAsync(new Address[] { address1, address2 });
            ///
            ///   while (t == null) ;
            ///   return t.Value;
            ///
            ///   //if (requester.WorkerSupportsCancellation)
            ///   //    requester.CancelAsync();
            ///   //requester.WorkerReportsProgress = true;
            ///   //requester.WorkerSupportsCancellation = true; //??
            ///   //requester.CreateObjRef(typeof((uint, TimeSpan))).
            ///private void Requester_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            ///{
            ///    throw new NotImplementedException();
            ///}
            ///private void Requester_DoWork(object sender, DoWorkEventArgs e)
            ///{
            ///    //worker = sender as BackgroundWorker;
            ///    Address[] addresses = (Address[])e.Argument;
            ///
            ///    string origin = addresses[0].Street + " " + addresses[0].HouseNumber + " " + addresses[0].City + " Israel";
            ///    string destination = addresses[1].Street + " " + addresses[1].HouseNumber + " " + addresses[1].City + " Israel";
            ///
            ///    string url = @"https://www.mapquestapi.com/directions/v2/route" +
            ///        @"?key=" + KEY +
            ///        @"&from=" + origin +
            ///        @"&to=" + destination +
            ///        @"&outFormat=xml" +
            ///        @"&ambiguities=ignore&routeType=fastest&doReverseGeocode=false" +
            ///        @"&enhancedNarrative=false&avoidTimedConditions=false";
            ///
            ///    //request from MapQuest service the distance between the 2 addresses
            ///    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            ///    WebResponse response = request.GetResponse();
            ///    Stream dataStream = response.GetResponseStream();
            ///    StreamReader sreader = new StreamReader(dataStream);
            ///    string responsereader = sreader.ReadToEnd();
            ///    response.Close();
            ///    //the response is given in an XML format 
            ///    XmlDocument xmldoc = new XmlDocument();
            ///    xmldoc.LoadXml(responsereader);
            ///
            ///    if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "0") //we have the expected answer
            ///    {     //display the returned distance
            ///        XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
            ///        double distInMiles = Convert.ToDouble(distance[0].ChildNodes[0].InnerText);
            ///        Debug.WriteLine("Distance In KM: " + distInMiles * 1.609344);
            ///
            ///        //display the returned driving time   
            ///        XmlNodeList formattedTime = xmldoc.GetElementsByTagName("formattedTime");
            ///        string fTime = formattedTime[0].ChildNodes[0].InnerText;
            ///        Debug.WriteLine("Driving Time: " + fTime);
            ///
            ///        e.Result = distInMiles * 1.609344;
            ///    }
            ///    else if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "402")
            ///    //we have an answer that an error occurred, one of the addresses is not found 
            ///    {
            ///        //e.Cancel = true;
            ///        e.Result = null;
            ///        Debug.WriteLine("an error occurred, one of the addresses is not found. try again.");
            ///    }
            ///    else //busy network or other error... 
            ///    {
            ///        Console.WriteLine("We have'nt got an answer, maybe the net is busy...");
            ///        Thread.Sleep(2000);
            ///        e.Result = null;
            ///    }
            ///}

        }

        #endregion

        #region Not Used

        public void TakeVacations(Tester tester, params DateTime[] dateTimes)
        {
            foreach (DateTime dateTime in dateTimes)
            {
                // CHECK that the date is work time
                if (tester.UnavailableDates.Exists(dt => dt == dateTime))
                    throw new CasingException(true, new Exception("You can't take a vacation on " + dateTime)); //TODO check if it is already vacation
                tester.UnavailableDates.Add(dateTime);
                UpdateTester(tester);
            }
        }

        private Tester FindsAnAlternativeTester(Test test)
        {
            return dal.GetTesters(tester => tester.ID != test.TesterID && tester.UnavailableDates.All(t => t.Date != test.Date)).FirstOrDefault();
        }

        [Obsolete("Undone", true)]
        public IEnumerable<Person> GetPeople()
        {
            // it wont work until Person:IEquatable
            return (from p in dal.GetTrainees()
                    select p).Concat((IEnumerable<Person>)(from p in dal.GetTesters()
                                                           select p));

            //List<Person> list = new List<Person>();

            //list.AddRange(GetTrainees());
            //list.AddRange(GetTesters());

            //return list;
        }

        #endregion

    }
}

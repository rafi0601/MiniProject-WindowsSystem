//Bs"d

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public static class Inspections
    {
        public static void IdValidator(string id) //TODO StringBuilder
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new CustomException(true, new ArgumentNullException("ID mustn't be null or empty or consists only white spaces"));

            if (id.Length > 9 || !uint.TryParse(id, out uint temp))
                throw new ArgumentException("ID is not valid");

            id = id.PadLeft(totalWidth: 9, paddingChar: '0');

            int counter = 0, incNum;
            for (int i = 0; i < 9; i++)
            {
                incNum = (id[i] - '0') * (i % 2 + 1);
                counter += incNum > 9 ? incNum - 9 : incNum;
            }

            if (counter % 10 != 0)
                throw new ArgumentException("ID is not valid");
        }

        public static void AddressValidator(Address address)
        {
            //           if (person.Address == null)

            if (string.IsNullOrWhiteSpace(address.City))
                throw new ArgumentNullException("City mustn't be null or empty or consists only white spaces");

            if (string.IsNullOrWhiteSpace(address.Street))
                throw new ArgumentNullException("Srteet mustn't be null or empty or consists only white spaces");

            if (address.HouseNumber == 0)
                throw new ArgumentOutOfRangeException("House number mustn't be zero");
        }

        private static void PersonInspection(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person), "Person mustn't be null");

            #region ID
            try
            {
                IdValidator(person.ID);
            }
            catch (Exception e)// when (e is ArgumentNullException)
            {
                throw new ArgumentException("ID is not valid", nameof(person.ID), e);
            }
            #endregion

            #region Name
            // if(person.Name.))

            if (string.IsNullOrWhiteSpace(person.Name.FirstName))
                throw new ArgumentNullException("First name mustn't be null or empty or consists only white spaces", nameof(person.Name.FirstName));

            if (string.IsNullOrWhiteSpace(person.Name.LastName))
                throw new ArgumentNullException("Last name mustn't be null or empty or consists only white spaces", nameof(person.Name.LastName));
            #endregion

            #region Birthdate
            if (person.Birthdate > DateTime.Today || person.Birthdate.Year < DateTime.Today.Year - 120)
                throw new ArgumentException("The Birthdate date is illogical", nameof(person.Birthdate));
            #endregion

            #region Gender
            #endregion

            #region PhoneNumber // cellphone
            if (string.IsNullOrWhiteSpace(person.PhoneNumber))
                throw new ArgumentNullException("Phone number mustn't be null or empty or consists only white spaces", nameof(person.PhoneNumber));

            if (!(ulong.TryParse(person.PhoneNumber, out ulong tmp) &&
                person.PhoneNumber.Length == 10 && person.PhoneNumber.StartsWith("05") ||
                person.PhoneNumber.Length == 13 && person.PhoneNumber.StartsWith("+9725")))
                throw new ArgumentException("The phone number is not valid", nameof(person.PhoneNumber));
            #endregion

            #region Address
            try
            {
                AddressValidator(person.Address);
            }
            catch (Exception e)
            {
                throw new ArgumentException("This address is not valid", nameof(person.Address), e);
            }
            #endregion
        }

        public static void TesterInspection(Tester tester)
        {
            #region base
            PersonInspection(tester);
            #endregion

            #region YearsOfExperience
            if (tester.YearsOfExperience > tester.AgeInYears)
                throw new ArgumentOutOfRangeException("Years  of experience is illogical", nameof(tester.YearsOfExperience));
            #endregion

            #region MaxOfTestsPerWeek
            if (tester.MaxOfTestsPerWeek > Configuration.WORKING_DAYS_A_WEEK * Configuration.WORKING_HOURS_A_DAY)
                throw new ArgumentOutOfRangeException();
            #endregion

            #region VehicleTypeExpertise
            #endregion

            #region WorkingHours
            if (tester.WorkingHours == null)
                throw new ArgumentNullException("");
            #endregion

            #region MaxDistanceFromAddress
            #endregion
        }

        public static void TraineeInspection(Trainee trainee)
        {
            #region base
            PersonInspection(trainee);
            #endregion

            #region Vehicle

            #endregion

            #region Gearbox

            #endregion

            #region DrivingSchool
            if (string.IsNullOrWhiteSpace(trainee.DrivingSchool))
                throw new ArgumentNullException("Driving's school name mustn't be null or empty or consists only white spaces", nameof(trainee.DrivingSchool));
            #endregion

            #region TeacherName
            if (string.IsNullOrWhiteSpace(trainee.TeacherName.FirstName))
                throw new ArgumentNullException("Teacher first name mustn't be null or empty or consists only white spaces", nameof(trainee.Name.FirstName));

            if (string.IsNullOrWhiteSpace(trainee.TeacherName.LastName))
                throw new ArgumentNullException("Teacher last name mustn't be null or empty or consists only white spaces", nameof(trainee.Name.LastName));
            #endregion

            #region NumberOfDoneLessons

            #endregion

            #region TheLastTest
            if (trainee.TheLastTest > DateTime.Now)
                throw new ArgumentOutOfRangeException("");

            #endregion
        }

        public static void TestInspection(Test test)
        {
            if (test == null)
                throw new ArgumentNullException(nameof(test), "Person mustn't be null");


            #region IDTester
            try
            {
                IdValidator(test.TesterID);
            }
            catch (Exception e)// when (e is ArgumentNullException)
            {
                new ArgumentException("ID is not valid", nameof(test.TesterID), e);
            }
            #endregion

            #region IDTrainee
            try
            {
                IdValidator(test.TraineeID);
            }
            catch (Exception e)// when (e is ArgumentNullException)
            {
                new ArgumentException("ID is not valid", nameof(test.TraineeID), e);
            }
            #endregion

            #region TestDate
            if (test.Date == null)
                throw new ArgumentNullException("The test's date musn't be null", nameof(test.Date));
            //BUG
            //if (test.Date < DateTime.Now)
            //    throw new ArgumentException("The requested time has passed");

            // check in  the bl that the date is good for the trainee age
            #endregion

            #region DepartureAddress
            try
            {
                AddressValidator(test.DepartureAddress);
            }
            catch (Exception e)
            {
                throw new ArgumentException("This address is not valid", nameof(test.DepartureAddress), e);
            }
            #endregion

            #region Vehicle
            #endregion



            /*
            #region Code
            if (test.Code == null)
                throw new ArgumentNullException("Code mustn't be null or empty or consists only white spaces");

            if (test.Code.Length != 8 || !uint.TryParse(test.Code, out uint temp))
                throw new ArgumentException("Code is not valid");
            #endregion
            */
            /*
            #region Length
            if (test.Length == null)
                throw new ArgumentNullException("The test's length musn't be null", nameof(test.Length));
            //?????????????????
            #endregion
            */
            //IsDone cant done before TestDate
            //IsPass cant pass if most failed
            //TesterNotes
        }

        #region
        //public static void StringInspection(string str, string nameOfVariable)
        //{
        //    if (string.IsNullOrWhiteSpace(str))
        //        throw new ArgumentNullException(nameOfVariable + "mustn't be null or empty or consists only white spaces", nameof(nameOfVariable));
        //}
        #endregion
    }
}
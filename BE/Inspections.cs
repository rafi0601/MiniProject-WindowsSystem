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
                throw new ArgumentNullException("ID mustn't be null or empty or consists only white spaces");

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
            //if address is class so null check 

            if (string.IsNullOrWhiteSpace(address.City))
                throw new ArgumentNullException("City mustn't be null or empty or consists only white spaces");

            if (string.IsNullOrWhiteSpace(address.Street))
                throw new ArgumentNullException("Srteet mustn't be null or empty or consists only white spaces");

            if (address.HouseNumber == 0)
                throw new ArgumentOutOfRangeException("House number mustn't be zero");
        }

        public static void PersonNameValidator(Person.PersonName personName)
        {
            //if address is class so null check 

            if (string.IsNullOrWhiteSpace(personName.FirstName))
                throw new ArgumentNullException("First name mustn't be null or empty or consists only white spaces");

            if (string.IsNullOrWhiteSpace(personName.LastName))
                throw new ArgumentNullException("Last name mustn't be null or empty or consists only white spaces");
        }

        private static void PersonInspection(Person person)
        {
            if (person == null)
                throw new ArgumentNullException("Person mustn't be null");

            #region ID
            try
            {
                IdValidator(person.ID);
            }
            catch (Exception e)
            {
                throw new ArgumentException("ID is not valid", e);
            }
            #endregion

            #region Name
            try
            {
                PersonNameValidator(person.Name);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Name is not valid", e);
            }
            #endregion

            #region Birthdate
            if (person.Birthdate > DateTime.Today || person.Birthdate.Year < DateTime.Today.Year - 120)
                throw new ArgumentException("The Birthdate date is illogical");
            #endregion

            #region Gender
            #endregion

            #region PhoneNumber // cellphone
            if (string.IsNullOrWhiteSpace(person.PhoneNumber))
                throw new ArgumentNullException("Phone number mustn't be null or empty or consists only white spaces");

            if (!(ulong.TryParse(person.PhoneNumber, out ulong tmp) &&
                person.PhoneNumber.Length == 10 && person.PhoneNumber.StartsWith("05") ||
                person.PhoneNumber.Length == 13 && person.PhoneNumber.StartsWith("+9725")))
                throw new ArgumentException("The phone number is not valid");
            #endregion

            #region Address
            try
            {
                AddressValidator(person.Address);
            }
            catch (Exception e)
            {
                throw new ArgumentException("This address is not valid", e);
            }
            #endregion

            #region Password
            #endregion
        }

        public static void TesterInspection(Tester tester)
        {
            #region base
            PersonInspection(tester);
            #endregion

            #region YearsOfExperience
            if (tester.YearsOfExperience > tester.AgeInYears)
                throw new ArgumentOutOfRangeException("Years  of experience is illogical");
            #endregion

            #region MaxOfTestsPerWeek
            if (tester.MaxOfTestsPerWeek > Configuration.WORKING_DAYS_A_WEEK * Configuration.WORKING_HOURS_A_DAY)
                throw new ArgumentOutOfRangeException();
            #endregion

            #region VehicleTypesExpertise
            #endregion

            #region WorkingHours
            if (tester.WorkingHours == null)
                throw new ArgumentNullException("");
            #endregion

            #region MaxDistanceFromAddress
            #endregion

            #region UnavailableDates
            #endregion
        }

        public static void TraineeInspection(Trainee trainee)
        {
            #region base
            PersonInspection(trainee);
            #endregion

            #region VehicleTypeTraining
#if TraineeHasMultipleTypes
            if (!trainee.VehicleTypeTraining.IsFlag())
                throw new Exception();
#endif
            #endregion

            #region GearboxTypeTraining
            #endregion

            #region DrivingSchool
            if (string.IsNullOrWhiteSpace(trainee.DrivingSchool))
                throw new ArgumentNullException("Driving's school name mustn't be null or empty or consists only white spaces");
            #endregion

            #region TeacherName
            PersonNameValidator(trainee.TeacherName);
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
                throw new ArgumentNullException("Test mustn't be null");

            #region Code
            if (test.Code == null)
                throw new ArgumentNullException("Code mustn't be null or empty or consists only white spaces");

            if (test.Code.Length != 8 || !uint.TryParse(test.Code, out uint temp))
                throw new ArgumentException("Code is not valid");
            #endregion

            #region TesterID
            try
            {
                IdValidator(test.TesterID);
            }
            catch (Exception e)// when (e is ArgumentNullException)
            {
                new ArgumentException("ID is not valid", e);
            }
            #endregion

            #region TraineeID
            try
            {
                IdValidator(test.TraineeID);
            }
            catch (Exception e)// when (e is ArgumentNullException)
            {
                new ArgumentException("ID is not valid", e);
            }
            #endregion

            #region Date
            if (test.Date == null)
                throw new ArgumentNullException("The test's date musn't be null");
            // check in  the bl that the date is good for the trainee age
            #endregion

            #region DepartureAddress
            try
            {
                AddressValidator(test.DepartureAddress);
            }
            catch (Exception e)
            {
                throw new ArgumentException("This address is not valid", e);
            }
            #endregion

            #region Vehicle
            if (!test.Vehicle.IsFlag())
                throw new Exception();
            #endregion

            #region CriteriasGrades
            #endregion

            #region IsPass
            if (test.Date > DateTime.Now && test.IsPass != null)
                throw new Exception();
            #endregion

                #region TesterNotes
                #endregion

                /*
                #region Length
                if (test.Length == null)
                    throw new ArgumentNullException("The test's length musn't be null");
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
        //        throw new ArgumentNullException(nameOfVariable + "mustn't be null or empty or consists only white spaces", nameOfVariable);
        //}
        #endregion
    }
}
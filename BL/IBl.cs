using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    /// <summary>
    /// Specifies requirements for the BL layer.
    /// </summary>
    public interface IBL
    {
        /// <summary>
        /// Adds a tester to the system.
        /// </summary>
        /// <param name="tester">
        /// The tester to be added.
        /// </param>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        void AddTester(Tester tester);

        /// <summary>
        /// Removes the occurrence of a specific tester from the system.
        /// </summary>
        /// <param name="tester">
        /// The tester to remove.
        /// </param>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        void RemoveTester(Tester tester);

        /// <summary>
        /// Updates the occurrence of a specific tester in the system.
        /// </summary>
        /// <param name="tester">
        /// The tester to update.
        /// </param>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        void UpdateTester(Tester tester);

        /// <summary>
        /// Retrieves the occurrence of a tester with a specific ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the required tester.
        /// </param>
        /// <returns>
        /// The tester that his ID matches the specified <paramref name="id"/>, if found; otherwise, null.
        /// </returns>
        Tester GetTester(string id);

        /// <summary>
        /// Retrieves all the testers that match the conditions defined by a specified predicate.
        /// </summary>
        /// <param name="match">
        /// The delegate that defines the conditions of the testers to search for.
        /// </param>
        /// <returns>
        /// A list containing all the testers that match the conditions defined by <paramref name="match"/>, if found; otherwise, an empty list.
        /// </returns>
        List<Tester> GetTesters(Predicate<Tester> match = null);

        /// <summary>
        /// Get all testers whose address is in MaxDistanceFromAdrress from <paramref name="address"/>.
        /// </summary>
        /// <param name="address">
        /// The address from which the distance is calculated.
        /// </param>
        /// <returns>
        /// A list of all the found testers.
        /// </returns>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        List<Tester> TheTestersWhoLiveInTheDistance(Address address);

        /// <summary>
        /// Get all testers who vacant on <paramref name="dateAndTime"/>.
        /// </summary>
        /// <param name="dateAndTime">
        /// The date being checked.
        /// </param>
        /// <returns>
        /// A list of all the found testers.
        /// </returns>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        List<Tester> VacantTesters(DateTime dateAndTime);

        /// <summary>
        /// Give to <paramref name="tester"/> vacations.
        /// </summary>
        /// <param name="tester">
        /// The tester who wants vacation.
        /// </param>
        /// <param name="dateTimes">
        /// The dates that <paramref name="tester"/> wants vacation.
        /// </param>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        void TakeVacations(Tester tester, params DateTime[] dateTimes);


        /// <summary>
        /// Adds a trainee to the system.
        /// </summary>
        /// <param name="trainee">
        /// The trainee to be added.
        /// </param>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        void AddTrainee(Trainee trainee);

        /// <summary>
        /// Removes the occurrence of a specific trainee from the system.
        /// </summary>
        /// <param name="trainee">
        /// The trainee to remove.
        /// </param>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        void RemoveTrainee(Trainee trainee);

        /// <summary>
        /// Updates the occurrence of a specific trainee in the system.
        /// </summary>
        /// <param name="trainee">
        /// The trainee to update.
        /// </param>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        void UpdateTrainee(Trainee trainee);

        /// <summary>
        /// Retrieves the occurrence of a trainee with a specific ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the required trainee.
        /// </param>
        /// <returns>
        /// The trainee that his ID matches the specified <paramref name="id"/>, if found; otherwise, null.
        /// </returns>
        Trainee GetTrainee(string id);

        /// <summary>
        /// Retrieves all the trainees that match the conditions defined by a specified predicate.
        /// </summary>
        /// <param name="match">
        /// The delegate that defines the conditions of the trainees to search for.
        /// </param>
        /// <returns>
        /// A list containing all the trainees that match the conditions defined by <paramref name="match"/>, if found; otherwise, an empty list.
        /// </returns>
        List<Trainee> GetTrainees(Predicate<Trainee> match = null);

        /// <summary>
        /// Get the number tests that <paramref name="trainee"/> has done.
        /// </summary>
        /// <param name="trainee">
        /// The trainee.
        /// </param>
        /// <returns>
        /// How many tests <paramref name="trainee"/> has done.
        /// </returns>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        uint NumberOfDoneTests(Trainee trainee);

        /// <summary>
        /// Get whether <paramref name="trainee"/> entitled to a license or not.
        /// </summary>
        /// <param name="trainee">
        /// The trainee.
        /// </param>
        /// <returns>
        /// True if <paramref name="trainee"/> has passed a test.
        /// False if <paramref name="trainee"/> hasn't passed a test.
        /// </returns>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        bool IsEntitledToLicense(Trainee trainee);


        /// <summary>
        /// Order a test.
        /// </summary>
        /// <param name="trainee">
        /// The trainee who wants test.
        /// </param>
        /// <param name="testDate">
        /// The date for the test.
        /// </param>
        /// <param name="departureAddress">
        /// The departure address of the test.
        /// </param>
        /// <param name="vehicle">
        /// The vehicle type of the test.
        /// </param>
        /// <returns>
        /// If the test was successfully added then null.
        /// If not, an offer of an alternate date
        /// </returns>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        /// // TODO return bool is success, and get out HATZAA
        DateTime? AddTest(Trainee trainee, DateTime testDate, Address departureAddress, Vehicle vehicle);

        /// <summary>
        /// Giving a test score after it has finished.
        /// </summary>
        /// <param name="code">
        /// The code of the completed test.
        /// </param>
        /// <param name="criteria">
        /// Criteria and grades.
        /// </param>
        /// <param name="isPass">
        /// If the trainee passed or not.
        /// </param>
        /// <param name="testerNotes">
        /// The notes of the tester.
        /// </param>
        /// <exception cref="CasingException">
        /// The operation could not be performed.
        /// </exception>
        void UpdateTest(string code, Test.Criteria criteria, bool isPass, string testerNotes);

        /// <summary>
        /// Retrieves the occurrence of a test with a specific code.
        /// </summary>
        /// <param name="code">
        /// The code of the required test.
        /// </param>
        /// <returns>
        /// The test that his code matches the specified <paramref name="code"/>, if found; otherwise, null.
        /// </returns>
        Test GetTest(string code);

        /// <summary>
        /// Retrieves all the tests that match the conditions defined by a specified predicate.
        /// </summary>
        /// <param name="match">
        /// The delegate that defines the conditions of the tests to search for.
        /// </param>
        /// <returns>
        /// A list containing all the tests that match the conditions defined by <paramref name="match"/>, if found; otherwise, an empty list.
        /// </returns>
        List<Test> GetTests(Predicate<Test> match = null);//FindAllInTests(Predicate<Test> condition)

        /// <summary>
        /// Get all future tests sorted by date.
        /// </summary>
        /// <returns>
        /// A list of all future tests sorted by date.
        /// </returns>
        List<Test> GetSortedFutureTests();


        /// <summary>
        /// Group all the testers in the system by expertises, sorted if requested.
        /// </summary>
        /// <param name="toSort">
        /// Will sort them by years of experience or not.
        /// </param>
        /// <returns>
        /// A list of groups of all the testers by expertises, sorted by expertise's years if <paramref name="toSort"/> is true. 
        /// </returns>
        List<IGrouping<Vehicle, Tester>> TestersByExpertise(bool toSort = false);

        /// <summary>
        /// Group all the trainees in the system by drivings school, sorted if requested.
        /// </summary>
        /// <param name="toSort">
        /// Will sort them by teachers or not.
        /// </param>
        /// <returns>
        /// A list of groups of all the trainees by drivings school, sorted by teachers if <paramref name="toSort"/> is true. 
        /// </returns>
        List<IGrouping<string, Trainee>> TraineesByDrivingSchool(bool toSort = false);

        /// <summary>
        /// Group all the trainees in the system by teachers, sorted if requested.
        /// </summary>
        /// <param name="toSort">
        /// Will sort them by names or not.
        /// </param>
        /// <returns>
        /// A list of groups of all the trainees by teachers, sorted by names if <paramref name="toSort"/> is true. 
        /// </returns>
        List<IGrouping<Person.PersonName, Trainee>> TraineesByTeacher(bool toSort = false);

        /// <summary>
        /// Group all the trainees in the system by numbers of tests, sorted if requested.
        /// </summary>
        /// <param name="toSort">
        /// Will sort them by drivings school or not.
        /// </param>
        /// <returns>
        /// A list of groups of all the trainees by numbers of tests, sorted by drivings school if <paramref name="toSort"/> is true. 
        /// </returns>
        List<IGrouping<uint, Trainee>> TraineesByNumberOfTests(bool toSort = false);
    }
}

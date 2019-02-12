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
        List<Tester> VacantTesters(DateTime dateAndTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toSort">
        /// 
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        List<IGrouping<Vehicle, Tester>> TestersByExpertise(bool toSort = false);


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
        /// 
        /// </summary>
        /// <param name="trainee"></param>
        /// <returns></returns>
        uint NumberOfDoneTests(Trainee trainee);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainee"></param>
        /// <returns></returns>
        bool IsEntitledToLicense(Trainee trainee);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toSort"></param>
        /// <returns></returns>
        List<IGrouping<string, Trainee>> TraineesByDrivingSchool(bool toSort = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toSort"></param>
        /// <returns></returns>
        List<IGrouping<Person.PersonName, Trainee>> TraineesByTeacher(bool toSort = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toSort"></param>
        /// <returns></returns>
        List<IGrouping<uint, Trainee>> TraineesByNumberOfTests(bool toSort = false);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainee"></param>
        /// <param name="TestDate"></param>
        /// <param name="DepartureAddress"></param>
        /// <param name="Vehicle"></param>
        /// <returns></returns>
        DateTime? AddTest(Trainee trainee, DateTime TestDate, /*DateTime length,*/ Address DepartureAddress, Vehicle Vehicle);//TODO out to the date and return bool for success

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="criteria"></param>
        /// <param name="IsPass"></param>
        /// <param name="TesterNotes"></param>
        void UpdateTest(string code, Test.Criteria criteria, bool IsPass, string TesterNotes);

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
    }

    ///GetTestsOfTester
    ///GetTestsOfTrainee
}

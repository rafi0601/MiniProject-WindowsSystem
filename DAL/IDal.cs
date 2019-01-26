//Bs"d

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    /// <summary>
    /// Specifies requirements for the DAL layer.
    /// </summary>
    public interface IDal
    {
        /// <summary>
        /// Adds a tester to the database.
        /// </summary>
        /// <param name="tester">
        /// The tester to be added.
        /// </param>
        /// <exception cref="CustomException">
        /// <paramref name="tester"/> is illegal. <see cref="Inspections.TesterInspection(Tester)"/> for more information.
        /// <para>A tester with the same ID as <paramref name="tester"/> already exists in the database.</para>
        /// </exception>
        void AddTester(Tester tester);

        /// <summary>
        /// Removes the occurrence of a specific tester from the database.
        /// </summary>
        /// <param name="tester">
        /// The tester to remove.
        /// </param>
        /// <exception cref="CustomException">
        /// <paramref name="tester"/> is null.
        /// <para>A tester with the same ID as <paramref name="tester"/> doesn't exist in the database.</para>
        /// </exception>
        void RemoveTester(Tester tester); // TODO bool RemoveTester(string id);

        /// <summary>
        /// Updates the occurrence of a specific tester in the database.
        /// </summary>
        /// <param name="tester">
        /// The tester to update.
        /// </param>
        /// <exception cref="CustomException">
        /// <paramref name="tester"/> is illegal. <see cref="Inspections.TesterInspection(Tester)"/> for more information.
        /// <para>A tester with the same ID as <paramref name="tester"/> doesn't exist in the database.</para>
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
        /// <exception cref="CustomException">
        /// <paramref name="id"/> is null.
        /// </exception>
        Tester GetTester(string id); // UNDONE throw exception if null

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
        /// Adds a trainee to the database.
        /// </summary>
        /// <param name="trainee">
        /// The trainee to be added.
        /// </param>
        /// <exception cref="CustomException">
        /// <paramref name="trainee"/> is illegal. <see cref="Inspections.TraineeInspection(Trainee)"/> for more information.
        /// <para>A trainee with the same ID as <paramref name="trainee"/> already exists in the database.</para>
        /// </exception>
        void AddTrainee(Trainee trainee);

        /// <summary>
        /// Removes the occurrence of a specific trainee from the database.
        /// </summary>
        /// <param name="trainee">
        /// The trainee to remove.
        /// </param>
        /// <exception cref="CustomException">
        /// <paramref name="trainee"/> is null.
        /// <para>A trainee with the same ID as <paramref name="trainee"/> doesn't exist in the database.</para>
        /// </exception>
        void RemoveTrainee(Trainee trainee);

        /// <summary>
        /// Updates the occurrence of a specific trainee in the database.
        /// </summary>
        /// <param name="trainee">
        /// The trainee to update.
        /// </param>
        /// <exception cref="CustomException">
        /// <paramref name="trainee"/> is illegal. <see cref="Inspections.TraineeInspection(Trainee)"/> for more information.
        /// <para>A trainee with the same ID as <paramref name="trainee"/> doesn't exist in the database.</para>
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
        /// <exception cref="CustomException">
        /// <paramref name="id"/> is null.
        /// </exception>
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
        /// Adds a test to the database.
        /// </summary>
        /// <param name="test">
        /// The test to be added.
        /// </param>
        /// <exception cref="CustomException">
        /// <paramref name="test"/> is illegal. <see cref="Inspections.TestInspection(Test)"/> for more information.
        /// <para>A test with the same ID as <paramref name="test"/> already exists in the database.</para>
        /// </exception>
        void AddTest(Test test);

        /// <summary>
        /// Updates the occurrence of a specific test in the database.
        /// </summary>
        /// <param name="test">
        /// The test to update.
        /// </param>
        /// <exception cref="CustomException">
        /// <paramref name="test"/> is illegal. <see cref="Inspections.TestInspection(Test)"/> for more information.
        /// <para>A test with the same ID as <paramref name="test"/> doesn't exist in the database.</para>
        /// </exception>
        void UpdateTest(Test test); // TODO When is Done!!!

        /// <summary>
        /// Retrieves the occurrence of a test with a specific ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the required test.
        /// </param>
        /// <returns>
        /// The test that his ID matches the specified <paramref name="id"/>, if found; otherwise, null.
        /// </returns>
        /// <exception cref="CustomException">
        /// <paramref name="id"/> is null.
        /// </exception>
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
        List<Test> GetTests(Predicate<Test> match = null);
    }
}
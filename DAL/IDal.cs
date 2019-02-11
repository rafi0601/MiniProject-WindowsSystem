//Bs"d

using System;
using System.Collections.Generic;
using System.IO;
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
        /// <exception cref="ExistingInTheDatabaseException">
        /// A tester with the same ID as <paramref name="tester"/> already exists in the database.
        /// </exception>
        /// <exception>
        /// Additionally, if <paramref name="tester"/> is illegal, then an exception will be thrown. <see cref="Inspections.TesterInspection(Tester)"/> for more information.
        /// </exception>
        /// <exception cref="IOException">
        /// Problems with the database.
        /// </exception>
        void AddTester(Tester tester);

        /// <summary>
        /// Removes the occurrence of a specific tester from the database.
        /// </summary>
        /// <param name="tester">
        /// The tester to remove.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tester"/> is null.
        /// </exception>
        /// <exception cref="ExistingInTheDatabaseException">
        /// A tester with the same ID as <paramref name="tester"/> doesn't exist in the database.
        /// </exception>
        /// <exception cref="IOException">
        /// Problems with the database.
        /// </exception>
        void RemoveTester(Tester tester);

        /// <summary>
        /// Updates the occurrence of a specific tester in the database.
        /// </summary>
        /// <param name="tester">
        /// The tester to update.
        /// </param>
        /// <exception cref="ExistingInTheDatabaseException">
        /// A tester with the same ID as <paramref name="tester"/> already exists in the database.
        /// </exception>
        /// <exception>
        /// Additionally, if <paramref name="tester"/> is illegal, then an exception will be thrown. <see cref="Inspections.TesterInspection(Tester)"/> for more information.
        /// </exception>
        /// <exception cref="IOException">
        /// Problems with the database.
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
        /// <exception cref="IOException">
        /// Problems with the database.
        /// </exception>
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
        /// <exception cref="IOException">
        /// Problems with the database.
        /// </exception>
        List<Tester> GetTesters(Predicate<Tester> match = null);


        /// <summary>
        /// Adds a trainee to the database.
        /// </summary>
        /// <param name="trainee">
        /// The trainee to be added.
        /// </param>
        /// <exception cref="ExistingInTheDatabaseException">
        /// A trainee with the same ID as <paramref name="trainee"/> already exists in the database.
        /// </exception>
        /// <exception>
        /// Additionally, if <paramref name="trainee"/> is illegal, then an exception will be thrown. <see cref="Inspections.TraineeInspection(Trainee)"/> for more information.
        /// </exception>
        /// <exception cref="IOException">
        /// Problems with the database.
        /// </exception>
        void AddTrainee(Trainee trainee);

        /// <summary>
        /// Removes the occurrence of a specific trainee from the database.
        /// </summary>
        /// <param name="trainee">
        /// The trainee to remove.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="trainee"/> is null.
        /// </exception>
        /// <exception cref="ExistingInTheDatabaseException">
        /// A trainee with the same ID as <paramref name="trainee"/> doesn't exist in the database.
        /// </exception>
        /// <exception cref="IOException">
        /// Problems with the database.
        /// </exception>
        void RemoveTrainee(Trainee trainee);

        /// <summary>
        /// Updates the occurrence of a specific trainee in the database.
        /// </summary>
        /// <param name="trainee">
        /// The trainee to update.
        /// </param>
        /// <exception cref="ExistingInTheDatabaseException">
        /// A trainee with the same ID as <paramref name="trainee"/> already exists in the database.
        /// </exception>
        /// <exception>
        /// Additionally, if <paramref name="trainee"/> is illegal, then an exception will be thrown. <see cref="Inspections.TraineeInspection(Trainee)"/> for more information.
        /// </exception>
        /// <exception cref="IOException">
        /// Problems with the database.
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
        /// <exception cref="IOException">
        /// Problems with the database.
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
        /// <exception cref="IOException">
        /// Problems with the database.
        /// </exception>
        List<Trainee> GetTrainees(Predicate<Trainee> match = null);


        /// <summary>
        /// Adds a test to the database.
        /// </summary>
        /// <param name="test">
        /// The test to be added.
        /// </param>
        /// <exception cref="ExistingInTheDatabaseException">
        /// <para> A tester with the same ID as <paramref name="test.TesterId"/> doesn't exist in the database.</para>
        /// <para> A trainee with the same ID as <paramref name="test.TraineeId"/> doesn't exist in the database.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="test.Code"/> is not null.
        /// </exception>
        /// <exception cref="OverflowException">
        /// There are no more available codes for tests.
        /// </exception>
        /// <exception>
        /// Additionally, if <paramref name="test"/> is illegal, then an exception will be thrown. <see cref="Inspections.TestInspection(Test)"/> for more information.
        /// </exception>
        /// <exception cref="IOException">
        /// Problems with the database.
        /// </exception>
        void AddTest(Test test);

        /// <summary>
        /// Updates the occurrence of a specific test in the database.
        /// </summary>
        /// <param name="test">
        /// The test to update.
        /// </param>
        /// <exception cref="ExistingInTheDatabaseException">
        /// A test with the same code as <paramref name="test"/> doesn't exist in the database.
        /// </exception>
        /// <exception>
        /// Additionally, if <paramref name="test"/> is illegal, then an exception will be thrown. <see cref="Inspections.TestInspection(Test)"/> for more information.
        /// </exception>
        /// <exception cref="IOException">
        /// Problems with the database.
        /// </exception>
        void UpdateTest(Test test); // TODO When is Done!!!

        /// <summary>
        /// Retrieves the occurrence of a test with a specific code.
        /// </summary>
        /// <param name="code">
        /// The code of the required test.
        /// </param>
        /// <returns>
        /// The test that his code matches the specified <paramref name="code"/>, if found; otherwise, null.
        /// </returns>
        /// <exception cref="IOException">
        /// Problems with the database.
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
        /// <exception cref="IOException">
        /// Problems with the database.
        /// </exception>
        List<Test> GetTests(Predicate<Test> match = null);
    }
}
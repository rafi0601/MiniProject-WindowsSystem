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
    /// Specifies requirements for the DAL layer
    /// </summary>
    public interface IDal
    {
        /// <summary>
        /// Adds a tester to the database.
        /// </summary>
        /// <param name="tester">
        /// The tester to be added.
        /// </param>
        void AddTester(Tester tester);

        /// <summary>
        /// Removes the occurrence of a specific tester from the database.
        /// </summary>
        /// <param name="tester">
        /// The tester to remove.
        /// </param>
        void RemoveTester(Tester tester); // TODO bool RemoveTester(string id);

        /// <summary>
        /// Updates the occurrence of a specific tester in the database.
        /// </summary>
        /// <param name="tester">
        /// The tester to be updated.
        /// </param>
        void UpdateTester(Tester tester);

        /// <summary>
        /// Gets the occurrence of
        /// </summary>
        /// <param name="id">
        /// 
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        Tester GetTester(string id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate">
        /// 
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        List<Tester> GetTesters(Predicate<Tester> predicate = null);



        void AddTrainee(Trainee trainee);
        void RemoveTrainee(Trainee trainee);
        void UpdateTrainee(Trainee trainee);
        Trainee GetTrainee(string id);
        List<Trainee> GetTrainees(Predicate<Trainee> predicate = null);



        void AddTest(Test trainee);
        void UpdateTest(Test trainee); //When is Done!!!
        Test GetTest(string code);
        List<Test> GetTests(Predicate<Test> predicate = null);
    }
}
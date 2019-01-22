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
        /// Adding tester to the database
        /// </summary>
        /// <param name="tester">
        /// The added tester
        /// </param>
        void AddTester(Tester tester);
        void RemoveTester(Tester tester); // TODO bool RemoveTester(string id);
        void UpdateTester(Tester tester);
        Tester GetTester(string id);
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
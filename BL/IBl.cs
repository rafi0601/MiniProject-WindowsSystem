using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public interface IBL
    {
        void AddTester(Tester tester);
        void RemoveTester(Tester tester);
        void UpdateTester(Tester tester);
        Tester GetTester(string id);
        List<Tester> GetTesters(Predicate<Tester> match = null);
        List<Tester> TheTestersWhoLiveInTheDistance(Address address);
        List<Tester> VacantTesters(DateTime dateAndTime);
        List<IGrouping<Vehicle, Tester>> TestersByExpertise(bool toSort = false);

        void AddTrainee(Trainee trainee);
        void RemoveTrainee(Trainee trainee);
        void UpdateTrainee(Trainee trainee);
        Trainee GetTrainee(string id);
        List<Trainee> GetTrainees(Predicate<Trainee> match = null);
        uint NumberOfDoneTests(Trainee trainee);
        bool IsEntitledToLicense(Trainee trainee);
        List<IGrouping<string, Trainee>> TraineesByDrivingSchool(bool toSort = false);
        List<IGrouping<Person.PersonName, Trainee>> TraineesByTeacher(bool toSort = false);
        List<IGrouping<uint, Trainee>> TraineesByNumberOfTests(bool toSort = false);

        Task<DateTime?> AddTest(Trainee trainee, DateTime TestDate, /*DateTime length,*/ Address DepartureAddress, Vehicle Vehicle);//TODO out to the date and return bool for success
        void UpdateTest(string code, Test.Criteria criteria, bool IsPass, string TesterNotes);
        Test GetTest(string code);
        List<Test> GetTests(Predicate<Test> match = null);//FindAllInTests(Predicate<Test> condition)
        List<Test> GetSortedFutureTests();
    }

    ///GetTestsOfTester
    ///GetTestsOfTrainee
}

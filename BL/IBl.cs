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
        //       DAL.IDal dal { get; }

        void AddTester(Tester tester);
        void RemoveTester(string idTester); // TODO match to IDal
        void UpdateTester(Tester tester);
        Tester GetTester(string id);
        List<Tester> GetTesters(Predicate<Tester> predicate = null);
        List<Tester> TheTestersWhoLiveInTheDistance(Address address);
        List<Tester> VacantTesters(DateTime dateAndTime);
        List<IGrouping<Vehicle, Tester>> TestersByExpertise(bool toSort = false);

        void AddTrainee(Trainee trainee);
        void RemoveTrainee(string idTrainee);
        void UpdateTrainee(Trainee trainee);
        Trainee GetTrainee(string id);
        List<Trainee> GetTrainees(Predicate<Trainee> predicate = null);
        uint NumberOfDoneTests(Trainee trainee);
        bool HasPassed(Trainee trainee); //IsEntitledToLicense()
        List<IGrouping<string, Trainee>> TraineesByDrivingSchool(bool toSort = false);
        List<IGrouping<Name, Trainee>> TraineesByTeacher(bool toSort = false);
        List<IGrouping<uint, Trainee>> TraineesByNumberOfTests(bool toSort = false);

        DateTime? AddTest(Trainee trainee, DateTime TestDate, DateTime length, Address DepartureAddress, Vehicle Vehicle);//TODO out to the date and return bool for success
        void UpdateTest(Trainee trainee, Vehicle vehicle, Test.Criteria criteria, bool IsPass, string TesterNotes);
        Test GetTest(string code);
        List<Test> GetTests(Predicate<Test> predicate = null);
        List<Test> FindAllInTests(Predicate<Test> condition); // it is GetTests -> not neccery
        List<Test> SortedFutureTests();
    }

    ///GetTestsOfTester
    ///GetTestsOfTrainee
}

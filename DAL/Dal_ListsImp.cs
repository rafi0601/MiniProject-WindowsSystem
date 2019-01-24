//Bs"d

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;
//using static DS.DS_Lists;

namespace DAL
{
    internal class Dal_ListsImp : IDal
    {
        public Dal_ListsImp() { }

        private static uint code = 0;


        #region Tester functions

        public void AddTester(Tester tester)
        {
            //CHECK if we use inspec in prop so will check null here
            Inspections.TesterInspection(tester);

            if (ExistingTesterById(tester.ID))
                throw new ArgumentException("Tester with same ID already exists in the database", nameof(tester.ID));

            DS_Lists.TesterList.Add(tester);
        }

        public void RemoveTester(Tester tester)
        {
            if (tester == null)
                throw new ArgumentNullException(nameof(tester), "Cannot remove null");

            if (0 == DS_Lists.TesterList.RemoveAll(t => t.ID == tester.ID))
                throw new ArgumentException("This tester doesn't exist in the database");

            //Tester t = FindingTesterById(tester?.ID);
            //if(t==null) throw new ArgumentException("This tester doesn't exist in the database");
            //TesterList.Remove(t);
        }

        public void UpdateTester(Tester tester)
        {
            Inspections.TesterInspection(tester);
            int index = DS_Lists.TesterList.FindIndex(t => t.ID == tester.ID);
            if (-1 == index)
                throw new ArgumentException("This tester doesn't exist in the database");
            DS_Lists.TesterList[index] = tester;
        }

        public Tester GetTester(string id)
        {
            return new Tester(FindingTesterById(id));
            //TesterList.FirstOrDefault(t => t.ID==id);
        }

        public List<Tester> GetTesters(Predicate<Tester> predicate = null)
        {
            //if (predicate == null) return TesterList;

            return (from tester in DS_Lists.TesterList
                    where predicate != null ? predicate(tester) : true
                    select new Tester(tester)).ToList();
            //return (predicate == null ? TesterList.Where(t => true) : TesterList.Where(t => predicate(t))).ToList();
        }


        private Tester FindingTesterById(string id)
        {
            return DS_Lists.TesterList.Find(tester => tester.ID == id);
        }

        private bool ExistingTesterById(string id)
        {
            return DS_Lists.TesterList.Exists(tester => tester.ID == id);
        }

        #endregion

        #region Trainee functions

        public void AddTrainee(Trainee trainee)
        {
            Inspections.TraineeInspection(trainee);

            if (ExistingTraineeById(trainee.ID))
                throw new ArgumentException("This trainee already exists in the database");

            DS_Lists.TraineeList.Add(trainee);
        }

        public void RemoveTrainee(Trainee trainee)
        {
            if (trainee == null)
                throw new ArgumentNullException();

            if (0 == DS_Lists.TraineeList.RemoveAll(t => t.ID == trainee.ID))
                throw new ArgumentException("This trainee doesn't exist in the database");
        }

        public void UpdateTrainee(Trainee trainee)
        {
            Inspections.TraineeInspection(trainee);
            int index = DS_Lists.TraineeList.FindIndex(t => t.ID == trainee.ID);
            if (-1 == index)
                throw new ArgumentException("This trainee doesn't exist in the database");
            DS_Lists.TraineeList[index] = trainee;
        }

        public Trainee GetTrainee(string id)
        {
            return new Trainee(FindingTraineeById(id));
        }

        public List<Trainee> GetTrainees(Predicate<Trainee> predicate = null)
        {
            return (from trainee in DS_Lists.TraineeList
                    where predicate != null ? predicate(trainee) : true
                    select new Trainee(trainee)).ToList();
        }


        private Trainee FindingTraineeById(string id)
        {
            return DS_Lists.TraineeList.Find(trainee => trainee.ID == id);
        }

        private bool ExistingTraineeById(string id)
        {
            return DS_Lists.TraineeList.Exists(trainee => trainee.ID == id);
        }

        #endregion

        #region Test functions

        public void AddTest(Test test)
        {
            Inspections.TestInspection(test);

            Tester tester = FindingTesterById(test.IDTester);

            if (tester == default(Tester)) //!ExistingTesterById(test.IDTester)
                throw new ArgumentException("The tester doesn't exist in the database");

            if (!ExistingTraineeById(test.IDTrainee))
                throw new ArgumentException("The trainee doesn't exist in the database");

            // if !ExistingTestByCode

            if (test.Code == null)
                test.Code = checked(++code).ToString().PadLeft(8, '0'); // TODO: catch (System.OverflowException e)

            DS_Lists.TestList.Add(test);

            tester.MyTests.Add(test);
        }

        public void UpdateTest(Test test)
        {
            Inspections.TestInspection(test);
            int index = DS_Lists.TestList.FindIndex(t => t.Code == test.Code);
            if (-1 == index)
                throw new ArgumentException("This test doesn't exist in the database");
            DS_Lists.TestList[index] = test;
        }

        public Test GetTest(string code)
        {
            return new Test(FindingTestByCode(code));
        }

        public List<Test> GetTests(Predicate<Test> predicate = null)
        {
            return (from test in DS_Lists.TestList
                    where predicate != null ? predicate(test) : true
                    select new Test(test)).ToList();
        }


        private Test FindingTestByCode(string code)
        {
            return DS_Lists.TestList.Find(test => test.Code == code);
        }

        private bool ExistingTestByCode(string code)
        {
            return DS_Lists.TestList.Exists(test => test.Code == code);
        }

        #endregion

        private Predicate<Tester> ComperisonOfTestersId(Tester tester) => t => tester?.ID == t?.ID;
    }
}
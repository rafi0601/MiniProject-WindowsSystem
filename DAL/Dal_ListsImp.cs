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

            DS_Lists.TesterList.Add(new Tester(tester));
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
            //try
            //{
            //    RemoveTester(tester);
            //    AddTester(new Tester(tester)); // IMPROVEMENT ds.Add(tester)
            //}
            //catch
            //{
            //    throw new ArgumentException("This tester doesn't exist in the database");
            //}


            Inspections.TesterInspection(tester);
            int index = DS_Lists.TesterList.FindIndex(t => t.ID == tester.ID);
            if (-1 == index)
                throw new ArgumentException("This tester doesn't exist in the database");
            DS_Lists.TesterList[index] = new Tester(tester);
        }

        public Tester GetTester(string id)
        {
            Tester tester = FindingTesterById(id);
            if (tester != null)
                return new Tester(FindingTesterById(id));
            return null;
            //TesterList.FirstOrDefault(t => t.ID==id);
        }

        public List<Tester> GetTesters(Predicate<Tester> match = null)
        {
            //if (predicate == null) return TesterList;

            return (from tester in DS_Lists.TesterList
                    where match != null ? match(tester) : true
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

            DS_Lists.TraineeList.Add(new Trainee(trainee));
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
            DS_Lists.TraineeList[index] = new Trainee(trainee);
        }

        public Trainee GetTrainee(string id)
        {
            Trainee trainee = FindingTraineeById(id);
            if (trainee != null)
                return new Trainee(trainee);
            return null;
        }

        public List<Trainee> GetTrainees(Predicate<Trainee> match = null)
        {
            return (from trainee in DS_Lists.TraineeList
                    where match != null ? match(trainee) : true
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

            Test copyOfTheTest = new Test(test);
            DS_Lists.TestList.Add(copyOfTheTest);
            tester.MyTests.Add(copyOfTheTest);
        }

        public void UpdateTest(Test test)
        {
            Inspections.TestInspection(test);
            int index = DS_Lists.TestList.FindIndex(t => t.Code == test.Code);
            if (-1 == index)
                throw new ArgumentException("This test doesn't exist in the database");
            DS_Lists.TestList[index] = new Test(test);
        }

        public Test GetTest(string code)
        {
            Test test = FindingTestByCode(code);
            if (test != null)
                return new Test(test);
            return null;
        }

        public List<Test> GetTests(Predicate<Test> match = null)
        {
            return (from test in DS_Lists.TestList
                    where match != null ? match(test) : true
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






        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool Add<T>(T t) where T : IKey //IMPROVEMENT להחליף לויוד ולהמיר לפרטיאל וללמש שם
        {
            //CHECK if we use inspec in prop so will check null here
            dynamic l; // TODO האם אפשרי לעשות פונקצית הרחבה לרשימה  שמחזירה אותה כרשימה מטיפוס טי
            switch (t)
            {
                case Tester tester:
                    Inspections.TesterInspection(tester);
                    l = DS_Lists.TesterList;
                    break;
                case Trainee trainee:
                    Inspections.TraineeInspection(trainee);
                    l = DS_Lists.TraineeList;
                    break;
                case Test test:
                    Inspections.TestInspection(test);
                    l = DS_Lists.TestList;
                    break;
                //case null:
                //    break;
                default:
                    throw new CustomException(false, new Exception());
            }

            List<T> lm = l as List<T>;
            if (lm.Exists(tt => t.Key == tt.Key))
                return false;
            lm.Add(t.Copy());
            return true;
        }

        private bool ExistingByKey<T>(T item, List<T> list) where T : IKey
        {
            return list.Exists(t => item?.Key == t?.Key);
        }

    }
}
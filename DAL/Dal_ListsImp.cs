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
            ////CHECK if we use inspec in prop so will check null here
            //Inspections.TesterInspection(tester);
            //
            //if (ExistingTesterById(tester.ID))
            //    throw new ArgumentException("Tester with same ID already exists in the database", nameof(tester.ID));
            //
            //DS_Lists.TesterList.Add(tester.Copy());

            if (!Add(tester))
                throw new ExistingInTheDatabaseException(true, "Tester with same ID already exists in the database");
                //throw new ArgumentException("Tester with same ID already exists in the database", nameof(tester.ID));
        }

        public void RemoveTester(Tester tester)
        {
            //if (tester == null)
            //    throw new ArgumentNullException(nameof(tester), "Cannot remove null");
            //
            //if (0 == DS_Lists.TesterList.RemoveAll(t => t.ID == tester.ID))
            //    throw new ArgumentException("This tester doesn't exist in the database");
            //
            //
            ////int index = DS_Lists.TesterList.FindIndex(t => t.ID == tester.ID);
            ////if (-1 == index)
            ////    throw new ArgumentException("This tester doesn't exist in the database");
            ////DS_Lists.TesterList.RemoveAt(index);
            //
            //
            ////Tester t = FindingTesterById(tester?.ID);
            ////if(t==null) throw new ArgumentException("This tester doesn't exist in the database");
            ////DS_Lists.TesterList.Remove(t);
            ///

            if (!Remove(tester))
                throw new ExistingInTheDatabaseException(false, "This tester doesn't exist in the database");
                //throw new ArgumentException("This tester doesn't exist in the database");
        }

        public void UpdateTester(Tester tester)
        {
            //Inspections.TesterInspection(tester);
            //int index = DS_Lists.TesterList.FindIndex(t => t.ID == tester.ID);
            //if (-1 == index)
            //    throw new ArgumentException("This tester doesn't exist in the database");
            //DS_Lists.TesterList[index] = new Tester(tester);
            //
            ////try
            ////{
            ////    RemoveTester(tester);
            ////    AddTester(new Tester(tester)); // IMPROVEMENT ds.Add(tester)
            ////}
            ////catch
            ////{
            ////    throw new ArgumentException("This tester doesn't exist in the database");
            ////}

            if (!Update(tester))
                throw new ExistingInTheDatabaseException(false, "This tester doesn't exist in the database");
                //throw new ArgumentException("This tester doesn't exist in the database");
        }

        public Tester GetTester(string id)
        {
            //Tester tester = FindingTesterById(id);
            //return tester != null ? new Tester(tester) : null;

            return FindingTesterById(id)?.Copy();
            //return GetTesters(tester => tester.ID == id).FirstOrDefault();
        }

        public List<Tester> GetTesters(Predicate<Tester> match = null)
        {
            //if (predicate == null) return TesterList;

            return (from tester in DS_Lists.TesterList
                    where match != null ? match(tester) : true
                    select tester.Copy())
                    .ToList();
            //return (predicate == null ? TesterList.Where(t => true) : TesterList.Where(t => predicate(t))).ToList();
        }

        private Tester FindingTesterById(string id)
        {
            return DS_Lists.TesterList.Find(tester => tester.ID == id);
            //return Find()
        }

        private bool ExistingTesterById(string id)
        {
            return DS_Lists.TesterList.Exists(tester => tester.ID == id);
        }

        #endregion

        #region Trainee functions

        public void AddTrainee(Trainee trainee)
        {
            if (!Add(trainee))
                throw new ArgumentException("This trainee already exists in the database");
        }

        public void RemoveTrainee(Trainee trainee)
        {
            if (!Remove(trainee))
                throw new ArgumentException("This trainee doesn't exist in the database");
        }

        public void UpdateTrainee(Trainee trainee)
        {
            if (!Update(trainee))
                throw new ArgumentException("This trainee doesn't exist in the database");
        }

        public Trainee GetTrainee(string id)
        {
            return FindingTraineeById(id)?.Copy();
        }

        public List<Trainee> GetTrainees(Predicate<Trainee> match = null)
        {
            return (from trainee in DS_Lists.TraineeList
                    where match != null ? match(trainee) : true
                    select trainee.Copy())
                    .ToList();
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

            Tester tester = FindingTesterById(test.TesterID);

            if (tester == default(Tester)) //!ExistingTesterById(test.IDTester)
                throw new ArgumentException("The tester doesn't exist in the database");

            if (!ExistingTraineeById(test.TraineeID))
                throw new ArgumentException("The trainee doesn't exist in the database");

             //if !ExistingTestByCode

            if (test.Code == null)
                test.Code = checked(++code).ToString().PadLeft(totalWidth: 8, paddingChar: '0'); // TODO: catch (System.OverflowException e)

            Test copyOfTheTest = test.Copy();
            DS_Lists.TestList.Add(copyOfTheTest);
            tester.MyTests.Add(copyOfTheTest);
        }

        public void UpdateTest(Test test)
        {
            if(!Update(test))
                throw new ArgumentException("This test doesn't exist in the database");
        }

        public Test GetTest(string code)
        {
            return FindingTestByCode(code)?.Copy();
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



        private Predicate<T> ComperisonOfKey<T>(T item) where T : IKey // TODO rename to equalitionOfKey
        {
            return t => item?.Key == t?.Key;
        }

        private bool Add<T>(T item) where T : IKey //IMPROVEMENT להחליף לויוד ולהמיר לפרטיאל וללמש שם
        {
            //CHECK if we use inspec in prop so will check null here
            dynamic list; // TODO האם אפשרי לעשות פונקצית הרחבה לרשימה  שמחזירה אותה כרשימה מטיפוס טי
            switch (item)
            {
                case Tester tester:
                    Inspections.TesterInspection(tester);
                    list = DS_Lists.TesterList;
                    break;
                case Trainee trainee:
                    Inspections.TraineeInspection(trainee);
                    list = DS_Lists.TraineeList;
                    break;
                case Test test:
                    Inspections.TestInspection(test);
                    list = DS_Lists.TestList;
                    break;
                //case null:
                //    break;
                default:
                    throw new CustomException(false, new Exception());
            }

            //if (list.Exists((Predicate<T>)(t => item.Key == t.Key)))
            if (list.Exists(ComperisonOfKey(item)))
                return false;
            list.Add(item.Copy());
            return true;
        }

        private bool Remove<T>(T item) where T : IKey
        {
            return 0 == GetTheMatchingList<T>().RemoveAll(ComperisonOfKey(item));
        }

        private bool Update<T>(T item) where T : IKey
        {
            dynamic list;
            switch (item)
            {
                case Tester tester:
                    Inspections.TesterInspection(tester);
                    list = DS_Lists.TesterList;
                    break;
                case Trainee trainee:
                    Inspections.TraineeInspection(trainee);
                    list = DS_Lists.TraineeList;
                    break;
                case Test test:
                    Inspections.TestInspection(test);
                    list = DS_Lists.TestList;
                    break;
                //case null:
                //    break;
                default:
                    throw new CustomException(false, new Exception());
            }

            int index = list.FindIndex(ComperisonOfKey(item));
            if (-1 == index)
                return false;
            list[index] = item.Copy();
            return true;
        }

        //private T Find<T>(IKey key) where T : IKey
        //{
        //    return GetTheMatchingList(example: item).Find((Predicate<T>)(t => t.Key == item.Key));
        //}

        protected dynamic GetTheMatchingList<T>()
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(Tester):
                    return DS_Lists.TesterList;
                case Type t when t == typeof(Trainee):
                    return DS_Lists.TraineeList;
                case Type t when t == typeof(Test):
                    return DS_Lists.TestList;
                //case null:
                //    break;
                default:
                    throw new CustomException(false, new Exception());
            }
        }
        // private bool ExistingByKey<T>(T item, List<T> list) where T : IKey
        // {
        //     return list.Exists(t => item?.Key == t?.Key);
        // }

    }
}
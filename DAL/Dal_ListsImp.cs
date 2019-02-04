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
//using static BE.Inspections;
using static BE.Configuration;


namespace DAL
{
    internal class Dal_ListsImp : IDal
    {
        private static uint code = 0;


        #region Tester functions

        public void AddTester(Tester tester)
        {
            if (!Add(tester, DS_Lists.TesterList, Inspections.TesterInspection))
                throw new ExistingInTheDatabaseException(true, "Tester with same ID already exists in the database");
        }

        public void RemoveTester(Tester tester)
        {
            if (!Remove(tester, DS_Lists.TesterList))
                throw new ExistingInTheDatabaseException(false, "Tester with same ID doesn't exist in the database");
        }

        public void UpdateTester(Tester tester)
        {
            if (!Update(tester, DS_Lists.TesterList, Inspections.TesterInspection))
                throw new ExistingInTheDatabaseException(false, "This tester doesn't exist in the database");
        }

        public Tester GetTester(string id)
        {
            return Get(DS_Lists.TesterList, id);
        }

        public List<Tester> GetTesters(Predicate<Tester> match = null)
        {
            return Get(DS_Lists.TesterList, match);
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
            if (!Add(trainee, DS_Lists.TraineeList, Inspections.TraineeInspection))
                throw new ExistingInTheDatabaseException(true, "Trainee with same ID already exists in the database");
        }

        public void RemoveTrainee(Trainee trainee)
        {
            if (!Remove(trainee, DS_Lists.TraineeList))
                throw new ExistingInTheDatabaseException(false, "Trainee with same ID doesn't exist in the database");
        }

        public void UpdateTrainee(Trainee trainee)
        {
            if (!Update(trainee, DS_Lists.TraineeList, Inspections.TraineeInspection))
                throw new ExistingInTheDatabaseException(false, "This trainee doesn't exist in the database");
        }

        public Trainee GetTrainee(string id)
        {
            return Get(DS_Lists.TraineeList, id);
        }

        public List<Trainee> GetTrainees(Predicate<Trainee> match = null)
        {
            return Get(DS_Lists.TraineeList, match);
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
            Tester tester = FindingTesterById(test.TesterID);

            if (tester == default(Tester)) //!ExistingTesterById(test.IDTester)
                throw new ArgumentException("The tester doesn't exist in the database");

            if (!ExistingTraineeById(test.TraineeID))
                throw new ArgumentException("The trainee doesn't exist in the database");

            if (test.Code != null)
                throw new ArgumentException("", nameof(test.Code)); // UNDONE 
            if (code > MAX_VALUE_TO_CODE)
                throw new OverflowException(""); //UNDONE
            test.Code = (++code).ToString().PadLeft(totalWidth: (int)LENGTH_OF_CODE, paddingChar: '0');

            if (!Add(test, DS_Lists.TestList, Inspections.TestInspection))
                throw new Exception();

            tester.MyTests.Add(test.Copy());
        }

        public void UpdateTest(Test test)
        {
            if (!Update(test, DS_Lists.TestList, Inspections.TestInspection))
                throw new ExistingInTheDatabaseException(false, "This test doesn't exist in the database");
        }

        public Test GetTest(string code)
        {
            return Get(DS_Lists.TestList, code);
        }

        public List<Test> GetTests(Predicate<Test> match = null)
        {
            return Get(DS_Lists.TestList, match);
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


        #region Execution functions

        private Predicate<T> ComperisonOfKey<T>(T item) where T : class, IKey // TODO rename to equalitionOfKey
        {
            return t => item?.Key == t.Key;
        }

        private bool Add<T>(T item, List<T> list, Action<T> inspection) where T : class, IKey //IMPROVEMENT להחליף לויוד ולהמיר לפרטיאל וללמש שם
        {
            //CHECK if we use inspec in prop so will check null here


            inspection?.Invoke(item);
            if (list.Exists(ComperisonOfKey(item)))
                return false;
            list.Add(item.Copy());
            return true;
        }

        private bool Remove<T>(T item, List<T> list) where T : class, IKey
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item), "Cannot remove null");
            return 0 == list.RemoveAll(ComperisonOfKey(item));
        }

        private bool Update<T>(T item, List<T> list, Action<T> inspection) where T : class, IKey
        {
            inspection?.Invoke(item);
            int index = list.FindIndex(ComperisonOfKey(item));
            if (-1 == index)
                return false;
            list[index] = item.Copy();
            return true;
        }

        private T Get<T>(List<T> list, string key) where T : class, IKey // TODO input check
        {
            return list.Find(t => t.Key == key)?.Copy();
            //return Get(list, t => t.Key == key).SingleOrDefault();
            //return list.SingleOrDefault(t => t.Key == key);
        }

        private List<T> Get<T>(List<T> list, Predicate<T> match = null) where T : class, IKey
        {
            return (List<T>)(match is null ? list : list.Where(t => match(t))).Select(t => t.Copy());

            //return (from tester in DS_Lists.TesterList
            //        where match != null ? match(tester) : true
            //        select tester.Copy())
            //        .ToList();
        }


        [Obsolete("it is not safety", true)]
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

            //List<T> list;
            //switch (item)
            //{
            //    case Tester tester:
            //        Inspections.TesterInspection(tester);
            //        list = (List<T>)DS_Lists.TesterList.Cast<T>();
            //        break;
            //    case Trainee trainee:
            //        Inspections.TraineeInspection(trainee);
            //        list = DS_Lists.TraineeList;
            //        break;
            //    case Test test:
            //        Inspections.TestInspection(test);
            //        list = DS_Lists.TestList;
            //        break;
            //    //case null:
            //    //    break;
            //    default:
            //        throw new CustomException(false, new Exception());
            //}
        }

        #endregion
    }
}
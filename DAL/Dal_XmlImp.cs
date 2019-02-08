﻿//Bs"d

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using BE;
using static BE.Configuration;



namespace DAL
{
    internal class Dal_XmlImp : IDal
    {
        private struct MyStruct
        {
            private static readonly string filesPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName, nameof(DS), "DS_Xml"); //IMPROVEMENT change constant

            private XElement root; public XElement Root
            {
                get => root;
                /*private*/
                set
                {
                    root = value; //BUG null check
                    root.Changing += Root_Changing;
                    root.Changed += Root_Changed;
                }
            }
            public string FilePath { get; private set; }

            private void Root_Changing(object sender, XObjectChangeEventArgs e)
            {
                //throw new NotImplementedException();
            }

            private void Root_Changed(object sender, XObjectChangeEventArgs e)
            {
                //Root.Save(FilePath);//, SaveOptions.None);

                //tests.root.close();
                //e.ObjectChange;

                //(sender as XElement).Save(filesPath, SaveOptions.None); // TODO switch case
            }


            public MyStruct(XElement root/*, string folderPath*/) : this()
            {
                Root = root ?? throw new ArgumentNullException(nameof(root));
                FilePath = Path.Combine(filesPath, $@"{Root.Name}.xml)");
                //FilePath = $@"{filesPath}\{root.Name}.xml" ?? throw new ArgumentNullException(nameof(folderPath));
            }
        }

        private MyStruct tests = new MyStruct(new XElement(nameof(tests)));// = new MyStruct(new XElement(nameof(Test) + s) /*,filesPath*/); //nameof(tests)
        private MyStruct testers = new MyStruct(new XElement(nameof(testers)));// = new MyStruct(new XElement(nameof(Tester) + s)/*, filesPath*/);
        private MyStruct trainees = new MyStruct(new XElement(nameof(trainees)));// = new MyStruct(new XElement(nameof(Trainee) + s)/*, filesPath*/);
        private MyStruct configs = new MyStruct(new XElement(nameof(configs)));// = new MyStruct(new XElement(nameof(Trainee) + s)/*, filesPath*/);

        //private const char s = 's';
        //private readonly FileStream testsFile;
        //private readonly FileStream testersFile;

        private static uint code;

        public Dal_XmlImp()
        {
            //tests.FilePath = @"TestsXml.xml";
            //testers.FilePath = @"TestersXml.xml";
            //trainees.FilePath = @"TraineesXml.xml";
            //config.FilePath = @"ConfigXml.xml";

            //testsFile = new FileStream(tests.FilePath, FileMode.Create);
            //testersFile = new FileStream(testers.FilePath, FileMode.Create); //CHECK

            //testsFile.Close();
            //testersFile.Close();

            if (!File.Exists(trainees.FilePath))
                trainees.Root.Save(trainees.FilePath);
            else
                TraineeloadData();

            if (!File.Exists(configs.FilePath))
            {
                configs.Root.Add(new XElement(nameof(code), code.ToString().PadLeft(totalWidth: (int)LENGTH_OF_CODE, paddingChar: '0')));
                //Add(,0,) because it the first time
                configs.Root.Save(configs.FilePath);
            }
            else
                ConfigloadData();
        }


        public static void SaveToXmlFile<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source);
            file.Close();
        }

        public static T LoadFromXmlFile<T>(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T result = (T)xmlSerializer.Deserialize(file);
            file.Close();
            return result;
        }


        #region Tester Functions

        public void AddTester(Tester tester)
        {
            Inspections.TesterInspection(tester);

            List<Tester> testersList;
            if (File.Exists(testers.FilePath))
                testersList = LoadFromXmlFile<List<Tester>>(testers.FilePath);
            else
                testersList = new List<Tester>();

            if (testersList.Exists(ComperisonOfKey(tester)))
                throw new ExistingInTheDatabaseException(true, "Tester with same ID already exists in the database");

            testersList.Add(tester);
            SaveToXmlFile(testersList, testers.FilePath);
        }

        public void UpdateTester(Tester tester)
        {
            Inspections.TesterInspection(tester);

            try
            {
                if (!File.Exists(testers.FilePath))
                    throw new ArgumentException("This tester doesn't exist in the database"); //TODO change the message

                List<Tester> testersList = LoadFromXmlFile<List<Tester>>(testers.FilePath);

                int index = testersList.FindIndex(ComperisonOfKey(tester));
                if (-1 == index)
                    throw new ArgumentException("This tester doesn't exist in the database");

                testersList[index] = tester.Copy();
                SaveToXmlFile(testersList, testers.FilePath);
            }
            catch
            {
                throw;
            }
        }

        public void RemoveTester(Tester tester)
        {
            List<Tester> testersList = LoadFromXmlFile<List<Tester>>(testers.FilePath);

            if (tester is null)
                throw new ArgumentNullException(nameof(tester), "Cannot remove null");

            if (testersList.RemoveAll(ComperisonOfKey(tester)) == 0)
                throw new ExistingInTheDatabaseException(false, "This tester doesn't exist in the database");

            testersList.Remove(tester);
            SaveToXmlFile(testersList, testers.FilePath);
        }

        public Tester GetTester(string id)
        {
            if (!File.Exists(testers.FilePath))
                return null;

            List<Tester> testersList = LoadFromXmlFile<List<Tester>>(testers.FilePath);
            return testersList.Find(t => t.ID == id)?.Copy();
        }

        public List<Tester> GetTesters(Predicate<Tester> match = null)
        {
            if (!File.Exists(testers.FilePath))
                return new List<Tester>();

            List<Tester> testersList = LoadFromXmlFile<List<Tester>>(testers.FilePath);

            return (from tester in testersList
                    where match != null ? match(tester) : true
                    select new Tester(tester)).ToList();
        }

        #endregion

        #region Trainee Functions

        private void TraineeloadData()
        {
            try
            {
                trainees.Root = XElement.Load(trainees.FilePath);
            }
            catch
            {
                throw new Exception("Trainees File upload problem");
            }
        }

        public void AddTrainee(Trainee trainee)
        {
            //trainees.root.Add(
            //    from fieldInfo in trainee.GetType().GetFields()
            //    where fieldInfo.IsPublic
            //    join 
            //    select new XElement(fieldInfo.Name, fieldInfo.GetValue(fieldInfo)));

            List<Trainee> traineesList = GetTrainees();

            Inspections.TraineeInspection(trainee);

            if (traineesList.Exists(ComperisonOfKey(trainee)))
                throw new ArgumentException("This trainee already exists in the database");

            trainees.Root.Add(
                new XElement(nameof(Trainee),
                    new XElement(nameof(trainee.ID), trainee.ID),
                    new XElement(nameof(trainee.Name),
                        new XElement(nameof(trainee.Name.LastName), trainee.Name.LastName),
                        new XElement(nameof(trainee.Name.FirstName), trainee.Name.FirstName)),
                    new XElement(nameof(trainee.Birthdate), trainee.Birthdate),
                    new XElement(nameof(trainee.Gender), trainee.Gender),
                    new XElement(nameof(trainee.PhoneNumber), trainee.PhoneNumber),
                    new XElement(nameof(trainee.Address),
                        new XElement(nameof(trainee.Address.Street), trainee.Address.Street),
                        new XElement(nameof(trainee.Address.HouseNumber), trainee.Address.HouseNumber),
                        new XElement(nameof(trainee.Address.City), trainee.Address.City)),
                    new XElement(nameof(trainee.Password), trainee.Password),
                    new XElement(nameof(trainee.VehicleTypeTraining), trainee.VehicleTypeTraining),
                    new XElement(nameof(trainee.GearboxTypeTraining), trainee.GearboxTypeTraining),
                    new XElement(nameof(trainee.DrivingSchool), trainee.DrivingSchool),
                    new XElement(nameof(trainee.TeacherName),
                        new XElement(nameof(trainee.TeacherName.LastName), trainee.TeacherName.LastName),
                        new XElement(nameof(trainee.TeacherName.FirstName), trainee.TeacherName.FirstName)),
                    new XElement(nameof(trainee.NumberOfDoneLessons), trainee.NumberOfDoneLessons)
                    ));
            trainees.Root.Save(trainees.FilePath);
        }

        public void UpdateTrainee(Trainee trainee)
        {
            RemoveTrainee(GetTrainee(trainee.ID));

            AddTrainee(trainee);

            trainees.Root.Save(trainees.FilePath);
        }

        public void RemoveTrainee(Trainee trainee)
        {
            XElement traineeXElement =
                (from tra in trainees.Root.Elements()
                 where tra.Element(nameof(trainee.ID)).Value == trainee.ID
                 select tra)
                 .FirstOrDefault(); //TODO ?.

            traineeXElement.Remove();
            trainees.Root.Save(trainees.FilePath);
        }

        public Trainee GetTrainee(string id)
        {
            TraineeloadData();
            //IMPROVEMENT to that with foreach until it find the id
            Trainee tmp = new Trainee();

            return (
                from trainee in trainees.Root.Elements()
                where trainee.Element(nameof(tmp.ID)).Value == id
                let nameXElement = trainee.Element(nameof(tmp.Name))
                let addressXElement = trainee.Element(nameof(tmp.Address))
                let teacherNameXElement = trainee.Element(nameof(tmp.TeacherName))
                select new Trainee(
                    id,
                    new Person.PersonName(nameXElement.Element(nameof(tmp.Name.LastName)).Value, nameXElement.Element(nameof(tmp.Name.FirstName)).Value),
                    DateTime.Parse(trainee.Element(nameof(tmp.Birthdate)).Value),
                    (Gender)Enum.Parse(typeof(Gender), trainee.Element(nameof(tmp.Gender)).Value),
                    trainee.Element(nameof(tmp.PhoneNumber)).Value,
                    new Address(addressXElement.Element(nameof(tmp.Address.Street)).Value, uint.Parse(addressXElement.Element(nameof(tmp.Address.HouseNumber)).Value), addressXElement.Element(nameof(tmp.Address.City)).Value),
                    trainee.Element(nameof(tmp.Password)).Value,
                    (Vehicle)Enum.Parse(typeof(Vehicle), trainee.Element(nameof(tmp.VehicleTypeTraining)).Value),
                    (Gearbox)Enum.Parse(typeof(Gearbox), trainee.Element(nameof(tmp.GearboxTypeTraining)).Value),
                    trainee.Element(nameof(tmp.DrivingSchool)).Value,
                    new Person.PersonName(teacherNameXElement.Element(nameof(tmp.TeacherName.LastName)).Value, teacherNameXElement.Element(nameof(tmp.TeacherName.FirstName)).Value),
                    uint.Parse(trainee.Element(nameof(tmp.NumberOfDoneLessons)).Value)
                )).FirstOrDefault();
        }

        public List<Trainee> GetTrainees(Predicate<Trainee> match = null)
        {
            TraineeloadData();
            Trainee tmp = new Trainee();

            return (
                from traineeXElement in trainees.Root.Elements()
                let trainee = Tools.FromXElement(traineeXElement)
                where match != null ? match(trainee) : true
                select trainee
                ).ToList();
        }

        #endregion

        #region Test Funcions

        public void AddTest(Test test)
        {
            Inspections.TestInspection(test);

            List<Test> testsList;

            if (File.Exists(tests.FilePath))
                testsList = LoadFromXmlFile<List<Test>>(tests.FilePath);
            else
                testsList = new List<Test>();

            Tester tester = GetTester(test.TesterID);

            if (tester == default(Tester)) //!ExistingTesterById(test.IDTester)
                throw new ArgumentException("The tester doesn't exist in the database");

            if (!GetTrainees(trainee => trainee.ID == test.TraineeID).Any())
                throw new ArgumentException("The trainee doesn't exist in the database");

            if (code > MAX_VALUE_TO_CODE)
                throw new OverflowException();
            if (test.Code != null)
                throw new ArgumentException();
            test.Code = (++code).ToString().PadLeft(totalWidth: 8, paddingChar: '0');

            configs.Root.Element(nameof(code)).Remove();
            configs.Root.Add(new XElement(nameof(code), code));

            try
            {
                testsList.Add(test);
                SaveToXmlFile(testsList, tests.FilePath);

                //tester.UnavailableDates.Add(test.Date.Copy());
                UpdateTester(tester);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTest(Test test)
        {
            Inspections.TestInspection(test);

            try
            {
                List<Test> testsList = LoadFromXmlFile<List<Test>>(tests.FilePath);

                int index = testsList.FindIndex(ComperisonOfKey(test));
                if (-1 == index)
                    throw new ArgumentException("This test doesn't exist in the database");

                testsList[index] = test.Copy();
                SaveToXmlFile(testsList, tests.FilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Test GetTest(string code)
        {
            if (!File.Exists(testers.FilePath))
                return null;

            List<Test> testsList = LoadFromXmlFile<List<Test>>(tests.FilePath);
            return testsList.Find(t => t.Code == code)?.Copy();
        }

        public List<Test> GetTests(Predicate<Test> match = null)
        {
            if (!File.Exists(tests.FilePath))
                return new List<Test>();

            List<Test> testsList = LoadFromXmlFile<List<Test>>(tests.FilePath);

            return (from test in testsList
                    where match != null ? match(test) : true
                    select new Test(test)).ToList();
        }

        #endregion

        private void ConfigloadData()
        {
            try
            {
                configs.Root = XElement.Load(configs.FilePath);
                XElement codeXElement = configs.Root.Element(nameof(code));
                if (codeXElement == null)
                    configs.Root.Add(new XElement(nameof(code), code = 0));
                else
                    code = uint.Parse(codeXElement.Value);
            }
            catch
            {
                throw new Exception("Configs File upload problem");
            }
        }

        private Predicate<T> ComperisonOfKey<T>(T item) where T : IKey // TODO rename to equalitionOfKey
        {
            return t => item?.Key == t?.Key;
        }

        //private void SaveUsers()
        //{
        //    new XElement("users", from u in users
        //                          select new XElement("user",
        //                            new XElement("name", u.Value.name),
        //                            new XElement("password", u.Value.password),
        //                            new XElement("role", u.Value.role.ToString()))).Save("usersXml.xml");
        //}
        //
        //Dictionary<string, User> users = new Dictionary<string, User>();


        //Dictionary<PropertyInfo, string> traineePropertyNames;
        //traineeRoot.ReplaceAttributes(trainee); // TODO check if it work

    }
}
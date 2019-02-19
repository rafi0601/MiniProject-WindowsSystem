//Bs"d

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

        private static uint code;


        #region Bundles

        private protected struct HelpreBundle
        {
            private XElement root; public XElement Root
            {
                get => root;
                set
                {
                    root = value ?? throw new ArgumentNullException("Root cannot be null");
                }
            }
            public string FilePath { get; private set; }
            public bool ByLinqToXml { get; set; }


            private void Root_Changed(object sender, XObjectChangeEventArgs e) // CHECK why it's not work...
            {
                if (ByLinqToXml)
                    Root.Save(FilePath);
            }

            public HelpreBundle(XElement root, string folderPath, bool byLinqToXml) : this() // CHECK need input check for path?
            {
                Root = root;
                Root.Changed += Root_Changed;
                FilePath = Path.Combine(folderPath, $@"{Root.Name}.xml)");
                ByLinqToXml = byLinqToXml;
            }

            public void LoadData()
            {
                try
                {
                    Root = XElement.Load(FilePath);
                }
                catch
                {
                    throw new IOException("File upload problem");
                }
            }

            public void Save()
            {
                // UNDONE
            }
        }

        private HelpreBundle tests = new HelpreBundle(new XElement(nameof(tests)), filesPath, false);
        private HelpreBundle testers = new HelpreBundle(new XElement(nameof(testers)), filesPath, false);
        private HelpreBundle trainees = new HelpreBundle(new XElement(nameof(trainees)), filesPath, true);
        private HelpreBundle configs = new HelpreBundle(new XElement(nameof(configs)), filesPath, true);

        private static readonly string filesPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName, nameof(DS), $"{nameof(DS)}_Xml");

        #endregion


        public Dal_XmlImp()
        {
            if (!Directory.Exists(filesPath))
                Directory.CreateDirectory(filesPath);

            if (!File.Exists(trainees.FilePath))
                trainees.Root.Save(trainees.FilePath);
            else
                trainees.LoadData();

            if (!File.Exists(configs.FilePath))
            {
                configs.Root.Add(new XElement(nameof(code), /* because code==0 :) */ 0.ToString().PadLeft(totalWidth: (int)LENGTH_OF_CODE, paddingChar: '0')));
                configs.Root.Save(configs.FilePath);
            }
            else
            {
                ConfigloadData();
                //configs.LoadData();
            }
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

            if (testersList.Exists(EqualityOfKeys(tester)))
                throw new ExistingInTheDatabaseException("A tester with same ID already exists in the database.");

            testersList.Add(tester);
            SaveToXmlFile(testersList, testers.FilePath);
        }

        public void UpdateTester(Tester tester)
        {
            Inspections.TesterInspection(tester);

            //if (!File.Exists(testers.FilePath))
            //    throw new ExistingInTheDatabaseException("A tester with same ID doesn't exist in the database.");

            List<Tester> testersList = LoadFromXmlFile<List<Tester>>(testers.FilePath);

            int index = testersList.FindIndex(EqualityOfKeys(tester));
            if (-1 == index)
                throw new ExistingInTheDatabaseException("A tester with same ID doesn't exist in the database.");

            testersList[index] = tester;
            SaveToXmlFile(testersList, testers.FilePath);
        }

        public void RemoveTester(Tester tester)
        {
            List<Tester> testersList = LoadFromXmlFile<List<Tester>>(testers.FilePath);

            if (tester is null)
                throw new ArgumentNullException("Cannot remove null");

            if (0 == testersList.RemoveAll(EqualityOfKeys(tester)))
                throw new ExistingInTheDatabaseException("A tester with same ID doesn't exist in the database.");

            testersList.Remove(tester);
            SaveToXmlFile(testersList, testers.FilePath);
        }

        public Tester GetTester(string id)
        {
            if (!File.Exists(testers.FilePath))
                return null;

            return LoadFromXmlFile<List<Tester>>(testers.FilePath).Find(t => t.ID == id);
        }

        public List<Tester> GetTesters(Predicate<Tester> match = null)
        {
            if (!File.Exists(testers.FilePath))
                return new List<Tester>();

            List<Tester> testersList = LoadFromXmlFile<List<Tester>>(testers.FilePath);

            return (from tester in testersList
                    where match != null ? match(tester) : true
                    select tester).ToList();
        }

        #endregion

        #region Trainee Functions

        public void AddTrainee(Trainee trainee)
        {
            Inspections.TraineeInspection(trainee);

            if (GetTrainees().Exists(EqualityOfKeys(trainee)))
                throw new ExistingInTheDatabaseException("A trainee with same ID already exists in the database.");

            trainees.Root.Add(ToXElement(trainee));
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
            trainees.LoadData();

            if (trainee is null)
                throw new ArgumentNullException("Cannot remove null");

            XElement theTrainee = (from traineeXElement in trainees.Root.Elements()
                                   where traineeXElement.Element(nameof(trainee.ID)).Value == trainee.ID
                                   select traineeXElement).FirstOrDefault();
            if (theTrainee is null)
                throw new ExistingInTheDatabaseException("A trainee with same ID doesn't exist in the database.");

            theTrainee.Remove();
            trainees.Root.Save(trainees.FilePath);
        }

        public Trainee GetTrainee(string id)
        {
            trainees.LoadData();

            return (
                from trainee in trainees.Root.Elements()
                where trainee.Element(nameof(tmp.ID)).Value == id
                select FromXElement(trainee)
                ).FirstOrDefault();
        }

        public List<Trainee> GetTrainees(Predicate<Trainee> match = null)
        {
            trainees.LoadData();

            return (
                from traineeXElement in trainees.Root.Elements()
                let trainee = FromXElement(traineeXElement)
                where match != null ? match(trainee) : true
                select trainee
                ).ToList();
        }


        protected static readonly Trainee tmp = new Trainee();

        private static Trainee FromXElement(XElement traineeXElement)
        {
            try
            {
                XElement nameXElement = traineeXElement.Element(nameof(tmp.Name));
                XElement addressXElement = traineeXElement.Element(nameof(tmp.Address));
                XElement teacherNameXElement = traineeXElement.Element(nameof(tmp.TeacherName));
                return new Trainee(
                    traineeXElement.Element(nameof(tmp.ID)).Value,
                    new Person.PersonName(nameXElement.Element(nameof(tmp.Name.LastName)).Value, nameXElement.Element(nameof(tmp.Name.FirstName)).Value),
                    DateTime.Parse(traineeXElement.Element(nameof(tmp.Birthdate)).Value),
                    (Gender)Enum.Parse(typeof(Gender), traineeXElement.Element(nameof(tmp.Gender)).Value),
                    traineeXElement.Element(nameof(tmp.MobileNumber)).Value,
                    new Address(addressXElement.Element(nameof(tmp.Address.Street)).Value, uint.Parse(addressXElement.Element(nameof(tmp.Address.HouseNumber)).Value), addressXElement.Element(nameof(tmp.Address.City)).Value),
                    traineeXElement.Element(nameof(tmp.Password)).Value,
                    (Vehicle)Enum.Parse(typeof(Vehicle), traineeXElement.Element(nameof(tmp.VehicleTypeTraining)).Value),
                    (Gearbox)Enum.Parse(typeof(Gearbox), traineeXElement.Element(nameof(tmp.GearboxTypeTraining)).Value),
                    traineeXElement.Element(nameof(tmp.DrivingSchool)).Value,
                    new Person.PersonName(teacherNameXElement.Element(nameof(tmp.TeacherName.LastName)).Value, teacherNameXElement.Element(nameof(tmp.TeacherName.FirstName)).Value),
                    uint.Parse(traineeXElement.Element(nameof(tmp.NumberOfDoneLessons)).Value),
                    DateTime.Parse(traineeXElement.Element(nameof(tmp.TheLastTest)).Value)
                    );
            }
            catch (Exception ex)
            {
                throw new IOException("Trainee file not illegal", ex);
            }
        }

        private static XElement ToXElement(Trainee trainee)
        {
            return
                new XElement(nameof(Trainee),
                    new XElement(nameof(trainee.ID), trainee.ID),
                    new XElement(nameof(trainee.Name),
                        new XElement(nameof(trainee.Name.LastName), trainee.Name.LastName),
                        new XElement(nameof(trainee.Name.FirstName), trainee.Name.FirstName)),
                    new XElement(nameof(trainee.Birthdate), trainee.Birthdate),
                    new XElement(nameof(trainee.Gender), trainee.Gender),
                    new XElement(nameof(trainee.MobileNumber), trainee.MobileNumber),
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
                    new XElement(nameof(trainee.NumberOfDoneLessons), trainee.NumberOfDoneLessons),
                    new XElement(nameof(trainee.TheLastTest), trainee.TheLastTest)
                    );
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


            if (GetTester(test.TesterID) == null)
                throw new ExistingInTheDatabaseException("The tester doesn't exist in the database.");

            if (GetTrainee(test.TraineeID) == null)
                throw new ExistingInTheDatabaseException("The trainee doesn't exist in the database.");

            if (test.Code != null)
                throw new ArgumentException("It is impossible to order a test that already has a code.");
            if (code > MAX_VALUE_TO_CODE)
                throw new OverflowException("No more available codes.");
            test.Code = (++code).ToString().PadLeft(totalWidth: (int)LENGTH_OF_CODE, paddingChar: '0');


            try
            {
                testsList.Add(test);
                SaveToXmlFile(testsList, tests.FilePath);
            }
            catch
            {
                code--;
                test.Code = null;
                throw;
            }
            finally
            {
                configs.Root.Element(nameof(code)).Remove();
                configs.Root.Add(new XElement(nameof(code), code));
                configs.Root.Save(configs.FilePath);
            }
        }

        public void UpdateTest(Test test)
        {
            Inspections.TestInspection(test);

            List<Test> testsList = LoadFromXmlFile<List<Test>>(tests.FilePath);

            int index = testsList.FindIndex(EqualityOfKeys(test));
            if (-1 == index)
                throw new ExistingInTheDatabaseException("A test with same code doesn't exist in the database.");

            testsList[index] = test;
            SaveToXmlFile(testsList, tests.FilePath);
        }

        public Test GetTest(string code)
        {
            if (!File.Exists(testers.FilePath))
                return null;

            return LoadFromXmlFile<List<Test>>(tests.FilePath).Find(t => t.Code == code);
        }

        public List<Test> GetTests(Predicate<Test> match = null)
        {
            if (!File.Exists(tests.FilePath))
                return new List<Test>();

            List<Test> testsList = LoadFromXmlFile<List<Test>>(tests.FilePath);

            return (from test in testsList
                    where match != null ? match(test) : true
                    select test).ToList();
        }

        #endregion

        #region Config Functions

        private void ConfigloadData()
        {
            configs.LoadData();

            XElement codeXElement = configs.Root.Element(nameof(code));
            if (codeXElement == null)
                configs.Root.Add(new XElement(nameof(code), code = 0));
            else
                code = uint.Parse(codeXElement.Value);
        }

        #endregion

        #region Helpers Functions

        private Predicate<T> EqualityOfKeys<T>(T item) where T : IKey
        {
            return t => item?.Key == t?.Key;
        }

        protected static void SaveToXmlFile<T>(T source, string path)
        {
            try
            {
                FileStream file = new FileStream(path, FileMode.Create);
                XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
                xmlSerializer.Serialize(file, source);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new IOException("File serialize problem", ex);
            }
        }

        protected static T LoadFromXmlFile<T>(string path)
        {
            try
            {
                FileStream file = new FileStream(path, FileMode.Open);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                T result = (T)xmlSerializer.Deserialize(file);
                file.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new IOException("File deserializer problem.", ex);
            }
        }

        #endregion


        #region notes

        ///private void Root_Changed(object sender, XObjectChangeEventArgs e)
        ///tests.root.close();
        ///e.ObjectChange;
        ///(sender as XElement).Save(filesPath, SaveOptions.None); // TODO switch case

        ///Dictionary<PropertyInfo, string> traineePropertyNames;
        ///traineeRoot.ReplaceAttributes(trainee); // TODO check if it work

        ///trainees.root.Add(
        ///    from fieldInfo in trainee.GetType().GetFields()
        ///    where fieldInfo.IsPublic
        ///    join 
        ///    select new XElement(fieldInfo.Name, fieldInfo.GetValue(fieldInfo)));

        #endregion
    }
}
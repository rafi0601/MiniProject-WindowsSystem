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

namespace DAL
{
    internal class Dal_XmlImp : IDal
    {
        private struct MyStruct
        {
            public readonly string filePath;
            public readonly XElement root;

            public MyStruct(string filePath, XElement root)
            {
                this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
                this.root = root ?? throw new ArgumentNullException(nameof(root));
            }
        }

        MyStruct tests;
        MyStruct testers;
        MyStruct trainees;

        private const char s = 's';
        private const string filesPath = @"\\DS\DS_Xml";
        private readonly string testsFilePath;
        private readonly string testersFilePath;
        private readonly string traineesFilePath;
        private readonly XElement testsRoot = new XElement(nameof(Test) + s);
        private readonly XElement testersRoot = new XElement(nameof(Tester) + s);
        private readonly XElement traineesRoot = new XElement(nameof(Trainee) + s);
        private readonly FileStream testsFile;
        private readonly FileStream testersFile;

        public Dal_XmlImp()
        {
            testsFile = new FileStream(testsFilePath, FileMode.Create);

            testersFile = new FileStream(testsFilePath, FileMode.Create); //CHECK

            if (!File.Exists(traineesFilePath))
                traineesRoot.Save(traineesFilePath);
            else
                traineesRoot = XElement.Load(traineesFilePath);

            testsFilePath = $@"{filesPath}\{testsRoot.Name}.xml"; // BUG root.FirstSun
            testersFilePath = $@"{filesPath}\{testersRoot.Name}.xml";
            traineesFilePath = $@"{filesPath}\{traineesRoot.Name}.xml";

            tests = new MyStruct(testsFilePath, testsRoot);
            testers = new MyStruct(testersFilePath, testersRoot);
            trainees = new MyStruct(traineesFilePath, traineesRoot);

            testsRoot.Changed += XmlFile_Changed;
            testersRoot.Changed += XmlFile_Changed;
            traineesRoot.Changed += XmlFile_Changed;
        }

        private void XmlFile_Changed(object sender, XObjectChangeEventArgs e)
        {
            try
            {
                (sender as XElement).Save(filesPath, SaveOptions.None); // TODO switch case
            }
            catch
            {
                throw;
            }
            finally
            {
                //testsRoot.Close();
            }
            //e.ObjectChange;
        }

        private void CreateFile(string path)
        {

        }



        public void AddTest(Test test)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(testsRoot.GetType());
            xmlSerializer.Serialize(testsFile, testsRoot);
            testsFile.Close();
        }

        public void UpdateTest(Test test)
        {
            throw new NotImplementedException();
        }

        public Test GetTest(string code)
        {
            throw new NotImplementedException();
        }

        public List<Test> GetTests(Predicate<Test> match = null)
        {
            throw new NotImplementedException();
        }



        public void AddTester(Tester tester)
        {
            throw new NotImplementedException();
        }

        public void UpdateTester(Tester tester)
        {
            throw new NotImplementedException();
        }

        public void RemoveTester(Tester tester)
        {
            throw new NotImplementedException();
        }

        public Tester GetTester(string id)
        {
            throw new NotImplementedException();
        }

        public List<Tester> GetTesters(Predicate<Tester> match = null)
        {
            throw new NotImplementedException();
        }



        public void AddTrainee(Trainee trainee)
        {
            //traineesRoot.Add(
            //    from x in trainee.GetType().GetFields()
            //    select new XElement(x.Name, x.GetValue(x)));

            traineesRoot.Add(
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
                    new XElement(nameof(trainee.VehicleTypeTraining), trainee.VehicleTypeTraining),
                    new XElement(nameof(trainee.GearboxTypeTraining), trainee.GearboxTypeTraining),
                    new XElement(nameof(trainee.DrivingSchool), trainee.DrivingSchool),
                    new XElement(nameof(trainee.TeacherName),
                        new XElement(nameof(trainee.TeacherName.LastName), trainee.TeacherName.LastName),
                        new XElement(nameof(trainee.TeacherName.FirstName)), trainee.TeacherName.FirstName)),
                    new XElement(nameof(trainee.NumberOfDoneLessons), trainee.NumberOfDoneLessons));
        }

        public void UpdateTrainee(Trainee trainee)
        {
            throw new NotImplementedException();
        }

        public void RemoveTrainee(Trainee trainee)
        {
            Trainee tmp;
            (from traineeXElement in traineesRoot.Elements()
             where traineeXElement.Element(nameof(tmp.ID)).Value == trainee.ID
             select traineeXElement)
             .FirstOrDefault().Remove(); //TODO ?.
        }

        public Trainee GetTrainee(string id)
        {
            //IMPROVEMENT to that with foreach until it find the id
            Trainee tmp = new Trainee();

            return (
                from trainee in traineesRoot.Elements()
                where trainee.Element(nameof(tmp.ID)).Value == id
                let nameXElement = trainee.Element(nameof(tmp.Name))
                let addressXElement = trainee.Element(nameof(tmp.Address))
                let teacherNameXElement = trainee.Element(nameof(tmp.TeacherName))
                select new Trainee(
                    id,
                    new Name(nameXElement.Element(nameof(tmp.Name.LastName)).Value, nameXElement.Element(nameof(tmp.Name.FirstName)).Value),
                    DateTime.Parse(trainee.Element(nameof(tmp.Birthdate)).Value),
                    (Gender)Enum.Parse(typeof(Gender), trainee.Element(nameof(tmp.Gender)).Value),
                    trainee.Element(nameof(tmp.PhoneNumber)).Value,
                    new Address(addressXElement.Element(nameof(tmp.Address.Street)).Value, uint.Parse(addressXElement.Element(nameof(tmp.Address.HouseNumber)).Value), addressXElement.Element(nameof(tmp.Address.City)).Value),
                    (Vehicle)Enum.Parse(typeof(Vehicle), trainee.Element(nameof(tmp.VehicleTypeTraining)).Value),
                    (Gearbox)Enum.Parse(typeof(Gearbox), trainee.Element(nameof(tmp.GearboxTypeTraining)).Value),
                    trainee.Element(nameof(tmp.DrivingSchool)).Value,
                    new Name(teacherNameXElement.Element(nameof(tmp.TeacherName.LastName)).Value, teacherNameXElement.Element(nameof(tmp.TeacherName.FirstName)).Value),
                    uint.Parse(trainee.Element(nameof(tmp.NumberOfDoneLessons)).Value)
                )).FirstOrDefault();
        }

        public List<Trainee> GetTrainees(Predicate<Trainee> match = null)
        {
            Trainee tmp = new Trainee();

            return (
                from trainee in traineesRoot.Elements()
                let t = Tools.FromXElement(trainee)
                where match != null ? match(t) : true
                select t
                ).ToList();
        }
    }
}

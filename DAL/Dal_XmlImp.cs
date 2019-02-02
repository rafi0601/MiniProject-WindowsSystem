using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        private const string filesPath = @"\\DS\DS_Xml";
        private readonly string testsFilePath;
        private readonly string testersFilePath;
        private readonly string traineesFilePath;
        private readonly XElement testsRoot = new XElement(nameof(Test) + 's');
        private readonly XElement testersRoot = new XElement(nameof(Tester) + 's');
        private readonly XElement traineesRoot = new XElement(nameof(Trainee) + 's');

        public Dal_XmlImp()
        {

            testsFilePath = $@"{filesPath}\{testsRoot.Name}.xml";
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
            testsRoot.Add(tests);
        }

        public void AddTester(Tester tester)
        {
            throw new NotImplementedException();
        }

        public void AddTrainee(Trainee trainee)
        {
            throw new NotImplementedException();
        }

        public Test GetTest(string code)
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

        public List<Test> GetTests(Predicate<Test> match = null)
        {
            throw new NotImplementedException();
        }

        public Trainee GetTrainee(string id)
        {
            throw new NotImplementedException();
        }

        public List<Trainee> GetTrainees(Predicate<Trainee> match = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveTester(Tester tester)
        {
            throw new NotImplementedException();
        }

        public void RemoveTrainee(Trainee trainee)
        {
            throw new NotImplementedException();
        }

        public void UpdateTest(Test test)
        {
            throw new NotImplementedException();
        }

        public void UpdateTester(Tester tester)
        {
            throw new NotImplementedException();
        }

        public void UpdateTrainee(Trainee trainee)
        {
            throw new NotImplementedException();
        }
    }
}

//Bs"d

using BE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    class TraineeXml
    {
        XElement traineeRoot { get; set; }
        string traineePath = @"TraineeXml.xml";

        public TraineeXml()
        {
            if (!File.Exists(traineePath))
                CreateFile();
            else
                LoadData();


            //Dictionary<PropertyInfo, string> traineePropertyNames;
        }

        private void CreateFile()
        {
            traineeRoot = new XElement("trainees");
            traineeRoot.Save(traineePath);
        }

        private void LoadData()
        {
            try
            {
                traineeRoot = XElement.Load(traineePath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }

        //public void SaveStudentList(List<Trainee> traineeList)
        //{
        //    traineeRoot = new XElement("trainees",
        //                            from trainee in traineeList
        //                            select new XElement("trainee",
        //                                new XElement("id", stu.ID),
        //                               new XElement("name",
        //                                    new XElement("firstName", stu.name.FirstName),
        //                                    new XElement("lastName", stu.name.LastName)
        //                                    )
        //                                )
        //                            );
        //
        //    traineeRoot.Save(traineePath);
        //}


        //public List<Student> GetStudentList()
        //{
        //    LoadData();
        //    List<Student> students;

        //    try
        //    {
        //        students = (from stu in traineeRoot.Elements()
        //                    select new Student()
        //                    {
        //                        ID = stu.Element("id").Value,
        //                        name = new Name()
        //                        {
        //                            FirstName = stu.Element("name").Element("firstName").Value,
        //                            LastName = stu.Element("name").Element("lastName").Value
        //                        }
        //                    }).ToList();
        //    }
        //    catch
        //    {
        //        students = null;
        //    }
        //    return students;
        //}

        public void AddTrainee(Trainee trainee)
        {

            traineeRoot.Save(traineePath);
        }

        public bool RemoveTrainee(string id)
        {
            Trainee tmp = new Trainee();

            try
            {

                traineeRoot.Save(traineePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateStudent(Trainee trainee)
        {
            (
                from traineeXElement in traineeRoot.Elements()
                where traineeXElement.Element(nameof(trainee.ID)).Value == trainee.ID
                select traineeXElement
                                       ).FirstOrDefault();

            traineeRoot.ReplaceAttributes(trainee); // TODO check if it work

            //RemoveTrainee(trainee.ID);
            //AddTrainee(trainee);

            traineeRoot.Save(traineePath);
        }

        public Trainee GetTrainee(string id)
        {


            LoadData();

            return new Trainee();
        }
    }
}


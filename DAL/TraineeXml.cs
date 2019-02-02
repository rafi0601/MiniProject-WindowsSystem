//Bs"d

using BE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                CreateFiles();
            else
                LoadData();
        }

        private void CreateFiles()
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
            traineeRoot.Add(new XElement("trainee",
                new XElement("person",
                    new XElement("id", trainee.ID),
                    new XElement("name",
                        new XElement("lastName", trainee.Name.LastName),
                        new XElement("firstName", trainee.Name.FirstName)),
                    new XElement("birthdate", trainee.Birthdate),
                    new XElement("gender", trainee.Gender),
                    new XElement("phoneNumber", trainee.PhoneNumber),
                    new XElement("address",
                        new XElement("street", trainee.Address.Street),
                        new XElement("houseNumber", trainee.Address.HouseNumber),
                        new XElement("city", trainee.Address.City))),

                new XElement("vehicleTypeTraining", trainee.VehicleTypeTraining),
                new XElement("gearboxTypeTraining", trainee.GearboxTypeTraining),
                new XElement("drivingSchool", trainee.DrivingSchool),
                new XElement("teacherName",
                    new XElement("teacherNameLastName", trainee.TeacherName.LastName),
                    new XElement("teacherNameFirstName", trainee.TeacherName.FirstName)),
                new XElement("numberOfDoneLessons", trainee.NumberOfDoneLessons)));

            traineeRoot.Save(traineePath);
        }

        public bool RemoveTrainee(string id)
        {
            XElement traineeElement;

            try
            {
                traineeElement = (from tra in traineeRoot.Elements()
                                  where tra.Element("person").Element("id").Value == id
                                  select tra
                                  ).FirstOrDefault();

                traineeElement.Remove();
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
            XElement traineeElement = (
                                       from tra in traineeRoot.Elements()
                                       where tra.Element("person").Element("id").Value == trainee.ID
                                       select tra
                                       ).FirstOrDefault();

            RemoveTrainee(trainee.ID);
            AddTrainee(trainee);

            traineeRoot.Save(traineePath);
        }

        public Trainee GetTrainee(string id)
        {
            LoadData();
            Trainee trainee = (
                           from tra in traineeRoot.Elements()
                           where tra.Element("person").Element("id").Value == id
                           select new Trainee(
                               tra.Element("person").Element("id").Value,
                               new Name()
                               {
                                   LastName = tra.Element("person").Element("name").Element("lastName").Value,
                                   FirstName = tra.Element("person").Element("name").Element("firstName").Value,
                               },
                               tra.Element("person").Element("birthdate").Value,
                               tra.Element("person").Element("gender").Value,
                               tra.Element("person").Element("phoneNumber").Value,
                               new Address()
                               {
                                   Street = tra.Element("person").Element("address").Element("street").Value,
                                   HouseNumber = tra.Element("person").Element("address").Element("houseNumber").Value,
                                   HouseNumber = tra.Element("person").Element("address").Element("houseNumber").Value,
                               }







                           ).FirstOrDefault();

            return (Trainee)traineeElement;
        }
    }
}


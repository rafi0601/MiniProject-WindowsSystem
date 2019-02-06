//Bs"d

using BE;
using Continuous.User;
using Continuous.User.Users.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
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


    public struct User
    {
        public string name;
        public string password;
        public Type role;

        public static explicit operator User(LocalUserInfo localUserInfo)
        {
            return new User() { name = localUserInfo?.Name, role = Type.GetType(localUserInfo?.Description, true) };
        }
    }

    //class Tools
    //{
    //    //TODO פונקציה הרחבה למחלקת המרה (סוף המרות באואסאף) שממירה בין היוזרים
    //    //internal static User ToUser(this Convert c)
    //    //{
    //    //
    //    //}
    //}

    public interface IUserManager
    {
        void Add(User user);
        void Remove(User user);
        void ChangePassword(User user);
        bool Exist(User user);
        User Get(string name);
        List<User> Get(Predicate<User> predicate = null);
    }

    internal class UserManager_Dictionary : IUserManager
    {
        Dictionary<string, User> users = new Dictionary<string, User>();


        public void Add(User user)
        {
            users.Add(user.name, user);
        }

        public void Remove(User user)
        {
            users.Remove(user.name);
        }

        public void ChangePassword(User user)
        {
            users[user.name] = user;
        }

        public bool Exist(User user)
        {
            return users.Contains(new KeyValuePair<string, User>(user.name, user));
        }

        public User Get(string name)
        {
            return users[name];
        }

        public List<User> Get(Predicate<User> predicate = null)
        {
            return (from user in users
                    where predicate != null ? predicate(user.Value) : true
                    select user.Value).ToList();
        }
    }


    internal class UsersManager_Api : IUserManager
    {
        // private static readonly Continuous.Management.ContinuousManagementFactory factory = new Continuous.Management.ContinuousManagementFactory();
        // private static readonly Continuous.User.LocalUserGroups.ILocalUserGroupShell localUsersGroup = factory.LocalUserGroup();

        private static readonly Continuous.User.Users.IUserShell usersShell = new Continuous.Management.ContinuousManagementFactory().UserShell();

        private UsersManager_Api(User admin)
        {
            //Add(admin);
        }

        public UsersManager_Api()
        {
        }

        public void Add(User user)
        {
            usersShell.Create(new LocalUserCreateModel { Name = user.name, Password = user.password, Description = user.role.ToString() });
            //If you try add user with already existing name, the MethodInvocationException will occur
        }

        public void Remove(User user)
        {
            try
            {
                usersShell.Remove(user.name);
            }
            catch (MethodInvocationException e)
            {
                //if you try remove non-existing user the MethodInvocationException will occur.
            }
        }

        public bool Exist(User user)
        {
            return usersShell.Exists(user.name);
        }

        public User Get(string name)
        {
            return (User)usersShell.GetLocalUser(name);
            //Returns null if user is not existing.
        }

        public List<User> Get(Predicate<User> predicate = null)
        {
            return (from localUserInfo in usersShell.GetAllUsers()
                    let user = (User)localUserInfo
                    where predicate != null ? predicate(user) : true
                    select user)
                    .ToList();
        }


        public void ChangePassword(User user) //TODO return true if succeed
        {
            //var securePassword = new System.Security.SecureString();
            //foreach (char c in user.password)
            //    securePassword.AppendChar(c);
            //usersShell.ChangePassword(user.name, securePassword); // CHECK new System.Security.SecureString()

            usersShell.ChangePassword(user.name, user.password);
            //If you try change password in non-existing user the MethodInvocationException will occur
        }



        //bool IUserManager.Exist => throw new NotImplementedException();

        private class MyLocalUserInfo : LocalUserInfo
        {
            public string Password { get; }//=
        }
    }

    public /*static*/ abstract class SingletonU
    {
        protected static IUserManager instance = null;

        //protected Singleton() { }

        public static IUserManager Instance
        {
            get
            {
                if (instance == null)
                    //instance = new UsersManager_Api();
                    instance = new UserManager_Dictionary();
                return instance;
            }
        }
    }

}


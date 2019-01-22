//BS"D

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Continuous.User;
using Continuous.User.Users.Model;

namespace PLWPF
{
    struct User
    {
        public string name;
        public string password;
        public Type role;

        public static explicit operator User(LocalUserInfo localUserInfo)
        {
            return new User() { name = localUserInfo?.Name, role = Type.GetType(localUserInfo?.Description, true) };
        }
    }

    class Tools
    {
        //TODO פונקציה הרחבה למחלקת המרה (סוף המרות באואסאף) שממירה בין היוזרים
        //internal static User ToUser(this Convert c)
        //{
        //
        //}
    }

    interface IUserManager
    {
        void Add(User user);
        void Remove(User user);
        void ChangePassword(User user);
        bool Exist(User user);
        User Get(string name);
        List<User> Get(Predicate<User> predicate = null);
    }

    internal class UsersManager : IUserManager
    {
        // private static readonly Continuous.Management.ContinuousManagementFactory factory = new Continuous.Management.ContinuousManagementFactory();
        // private static readonly Continuous.User.LocalUserGroups.ILocalUserGroupShell localUsersGroup = factory.LocalUserGroup();

        private static readonly Continuous.User.Users.IUserShell usersShell = new Continuous.Management.ContinuousManagementFactory().UserShell();

        public UsersManager(User admin)
        {
            Add(admin);
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


        public void ChangePassword(User user) //TODO return true if succed
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
}


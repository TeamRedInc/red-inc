﻿using core.Modules.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public class Core
    {
        private UserDao userDao;

        /// <summary>
        /// Primary thread of execution
        /// All logic starts here!
        /// </summary>
        public Core()
        {
            userDao = new UserDao();
        }

        public bool AddUser(string email, string passwordHash, string firstName, string lastName, bool isAdmin)
        {
            UserData user = new UserData(0);
            user.Email = email;
            user.PasswordHash = passwordHash;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.IsAdmin = isAdmin;

            return userDao.Add(user);
        }

        public UserData Login(string email, string passwordHash)
        {
            UserData user = new UserData(0);
            user.Email = email;
            user.PasswordHash = passwordHash;

            return userDao.Login(user);
        }
    }
}

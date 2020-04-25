using Dapper;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Odbc;
using System.Linq;
using TRWpf.Interfaces;
using TRWpf.Models;

namespace TRWpf.DataAccess
{
    public class UserRepository : IUserRepository
    {

        private const string selectUsersSQL =
            "SELECT NAME, USER_ID, DEPT_ID, ACTIVATE, DEACTIVATE, FAMILY, ACC_LVL, FNAME, \"GROUP\" , \"POSITION\" FROM USERS";
       private const string insertUserSQL =
            "INSERT INTO USERS ( USER_ID, FAMILY, FNAME, NAME, ACTIVATE, DEACTIVATE, ACC_LVL, \"GROUP\", \"POSITION\", DEPT_ID) " +
            "VALUES ((SELECT MAX(USER_ID) + 1 AS USER_ID FROM USERS), ?Family?, ?Fname?, ?Name?, ?Activate?, ?Deactivate?, ?Acc_Lvl?, ?Group?, ?Position?, ?Dept_Id?) RETURNING USER_ID";
        private const string updateUserSQL =
            "UPDATE USERS SET FAMILY = ?Family?, NAME = ?Name?, FNAME = ?Fname?, \"GROUP\" = ?Group?, \"POSITION\" = ?Position?, " +
            "DEPT_ID = ?Dept_Id?, ACTIVATE = ?Activate?, DEACTIVATE = ?Deactivate?, ACC_LVL = ?Acc_Lvl? " +
            "WHERE USER_ID = ?User_Id?";
        private const string deleteUserSQL =
            @"DELETE FROM USERS WHERE USER_ID = ?User_Id?";

        //private const string connectionOdbcString =
        //    "Driver={Firebird/InterBase(r) driver};dbname=temporeale:c:\\database\\tr.ib;charset=NONE;uid=SYSDBA;password=rootXXXXXX";

        private string connectionString = null;

        public UserRepository(string connection)
        {
            connectionString = connection;
        }

        public UserRepository() : this(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
        }

        private static readonly Lazy<ObservableCollection<User>> instance =
            new Lazy<ObservableCollection<User>>(() =>
            {
                UserRepository ur = new UserRepository();
                return ur.GetUsers();
            });

        public static ObservableCollection<User> Users => instance.Value;
        private ObservableCollection<User> GetUsers()
        {
            using OdbcConnection db = new OdbcConnection(connectionString);
            return new ObservableCollection<User>(db.Query<User>(selectUsersSQL).ToList());
        }

        public int Create(User user)
        {
            int id;
            using (OdbcConnection db = new OdbcConnection(connectionString))
            {
                user?.Log("CREATE USER IN DB");
                var result = db.Query<int>(insertUserSQL, user);
                id = result.Single();
            }
            return id;
        }

        public void Delete(int id)
        {
            using OdbcConnection db = new OdbcConnection(connectionString);
            db.Execute(deleteUserSQL, new { User_Id = id });
        }

        public User GetById(int id)
        {
            return Users.FirstOrDefault(u => u.User_Id == id);
        }

        public void Update(User user)
        {
            using OdbcConnection db = new OdbcConnection(connectionString);
#if DEBUG
            user?.Log("UPDATE USER IN DB");
#endif
            db.Execute(updateUserSQL, user);
        }
    }
}

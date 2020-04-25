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
    public class User_PhotoRepository : IUser_PhotoRepository
    {
        const string selectUser_PhotosSQL =
            @"SELECT USER_ID, PHOTO  FROM USER_PHOTOS";
        const string selectUser_PhotoSQL =
            @"SELECT USER_ID, PHOTO  FROM USER_PHOTOS WHERE USER_ID = ?User_Id?";
        const string updateOrInsertUser_PhotoSQL =
            @"UPDATE OR INSERT INTO USER_PHOTOS (USER_ID, PHOTO) VALUES (?User_Id?, ?Photo?) MATCHING (USER_ID)";
        const string deleteUser_PhotoSQL = 
            @"DELETE FROM USER_PHOTOS WHERE USER_ID=?User_Id?";

        string connectionString = null;

        public User_PhotoRepository(string connection)
        {
            connectionString = connection;
        }

        public User_PhotoRepository() : this(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
        }

        private static readonly Lazy<ObservableCollection<User_Photo>> instance =
            new Lazy<ObservableCollection<User_Photo>>(() =>
            {
                User_PhotoRepository upr = new User_PhotoRepository();
                return upr.GetUser_Photos();
            });

        public static ObservableCollection<User_Photo> User_Photos => instance.Value;

        ObservableCollection<User_Photo> GetUser_Photos()
        {
            using OdbcConnection db = new OdbcConnection(connectionString);
            return new ObservableCollection<User_Photo>(
                db.Query<User_Photo>(selectUser_PhotosSQL).ToList()
            );
        }

        public void Delete(int id)
        {
            using OdbcConnection db = new OdbcConnection(connectionString);
            db.Execute(deleteUser_PhotoSQL, new { User_Id = id });
        }

        public User_Photo GetById(int id)
        {
            using OdbcConnection db = new OdbcConnection(connectionString);
            return db.QueryFirstOrDefault<User_Photo>(selectUser_PhotoSQL, new { User_Id = id });
        }    
        
        public void UpdateOrInsert(User_Photo user)
        {
            using OdbcConnection db = new OdbcConnection(connectionString);           
            db.Execute(updateOrInsertUser_PhotoSQL, user);            
        }
    }
}

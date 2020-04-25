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
    public class Acc_LevelRepository : IAcc_LevelRepository
    {

        private const string selectAcc_LevelsSQL =
            @"SELECT ACC_LVL, ACL_NAME FROM ACC_LEVELS";

        private readonly string connectionString = null;

        public Acc_LevelRepository(string connection)
        {
            connectionString = connection;
        }

        public Acc_LevelRepository() : this (ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
        }

        private static readonly Lazy<ObservableCollection<Acc_Level>> instance =
            new Lazy<ObservableCollection<Acc_Level>>(() =>
            {
                Acc_LevelRepository al = new Acc_LevelRepository();
                return al.GetAcc_Levels();
            });

        public static ObservableCollection<Acc_Level> Acc_Levels => instance.Value;

        private ObservableCollection<Acc_Level> GetAcc_Levels()
        {
            using OdbcConnection db = new OdbcConnection(connectionString);
            return new ObservableCollection<Acc_Level>(db.Query<Acc_Level>(selectAcc_LevelsSQL).ToList());
        }

        public static Acc_Level GetById(int id) => 
            Acc_Levels.FirstOrDefault(a => a.Acc_Lvl == id);
    }
}

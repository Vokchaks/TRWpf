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
    public class DepartmentRepository : IDepartmentRepository
    {

        private const string selectDepartmentsSQL =
            @"SELECT NAME, DEPT_ID FROM DEPARTMENTS";

        private readonly string connectionString = null;

        public DepartmentRepository(string connection)
        {
            connectionString = connection;
        }

        public DepartmentRepository() : this (ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
        }

        private static readonly Lazy<ObservableCollection<Department>> instance =
            new Lazy<ObservableCollection<Department>>(() =>
            {
                DepartmentRepository dr = new DepartmentRepository();
                return dr.GetDepartments();
            });

        public static ObservableCollection<Department> Departments => instance.Value;
        private ObservableCollection<Department> GetDepartments()
        {
            using OdbcConnection db = new OdbcConnection(connectionString);
            return new ObservableCollection<Department>(db.Query<Department>(selectDepartmentsSQL).ToList());
        }

        public Department GetById(int id)
        {
            var d = Departments.FirstOrDefault(d => d.Dept_Id == id);
            return d ?? Departments.First();
        }
    }
}

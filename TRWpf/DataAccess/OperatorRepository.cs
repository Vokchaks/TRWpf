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
    public class OperatorRepository : IOperatorRepository
    {

        private const string selectOperatorsSQL =
            "SELECT OP_ID, LOGIN, \"PASSWORD\", FULL_NAME FROM OPERATORS";

        private string connectionString = null;

        public OperatorRepository(string connection)
        {
            connectionString = connection;
        }

        public OperatorRepository() : this(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
        }

        public bool TestConnection()
        {
            bool testConnect = false;
            OdbcConnection db = new OdbcConnection(connectionString);
            try
            {
                using (db)
                {
                    db.Open();
                    testConnect = true;
                }
            }
            catch (Exception)
            {                
            }
            finally
            {
                db.Close();
            }
            return testConnect;
        }

        private static readonly Lazy<ObservableCollection<Operator>> instance =
            new Lazy<ObservableCollection<Operator>>(() =>
            {
                OperatorRepository or = new OperatorRepository();
                return or.GetOperators();
            });

        public static ObservableCollection<Operator> Operators => instance.Value;
        private ObservableCollection<Operator> GetOperators()
        {
            using OdbcConnection db = new OdbcConnection(connectionString);
            return new ObservableCollection<Operator>(db.Query<Operator>(selectOperatorsSQL).ToList());
        }
        public Operator GetById(int id)
        {
            return Operators.FirstOrDefault(o => o.Op_Id == id);
        }

        public Operator GetByLoginPassword(string login, string password)
        {
            return Operators.FirstOrDefault(o => (o.Login == login && o.Password == password));
        }
    }
}

using Dapper;
using System;
using System.Collections.ObjectModel;
using System.Data.Odbc;
using System.Linq;
using TRWpf.Interfaces;
using TRWpf.Models;
using System.Configuration;
using System.Diagnostics;

namespace TRWpf.DataAccess
{
    public class CardRepository  : ICardRepository
    {
        private const string selectCardsSQL =
                "SELECT CARD_NUM, USER_ID, STATUS, LAST_USED, CARD_TYPE, DB_SEG FROM CARDS";

        const string updateOrInsertCardSQL =
               @"UPDATE OR INSERT INTO CARDS (CARD_NUM, USER_ID, STATUS, CARD_TYPE, DB_SEG ) " +
                "VALUES (?Card_Num?, ?User_Id?, ?Status?, ?Card_Type?, ?Db_Seg?) " + 
                "MATCHING (CARD_NUM)";

        private const string deleteCardSQL =
                "DELETE FROM CARDS WHERE CARD_NUM = ?Card_Num?";

        private readonly string connectionString;
        public CardRepository(string connection)
        {
            connectionString = connection;
        }

        public CardRepository() : this (ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
        }

        public void Delete(int id)
        {
            Debug.WriteLine($"DELETE CARD {id}");
            using OdbcConnection db = new OdbcConnection(connectionString);
            db.Execute(deleteCardSQL, new { Card_Num = id });
        }

        public Card GetById(int id)
        {
            return Cards.FirstOrDefault(c => c.Card_Num == id);
        }

        private static readonly Lazy<ObservableCollection<Card>> instance =
            new Lazy<ObservableCollection<Card>>(() =>
            {
                CardRepository cr = new CardRepository();
                return cr.GetCards();
            });
  
        public static ObservableCollection<Card> Cards => instance.Value;

        private ObservableCollection<Card> GetCards()
        {
            using OdbcConnection db = new OdbcConnection(connectionString);
            return new ObservableCollection<Card>(db.Query<Card>(selectCardsSQL).ToList());
        }

        public void UpdateOrInsert(Card card)
        {
            card?.Log("UPDATE OR INSERT CARD");
            using OdbcConnection db = new OdbcConnection(connectionString);
            db.Execute(updateOrInsertCardSQL, card);
        }
    }
}


  
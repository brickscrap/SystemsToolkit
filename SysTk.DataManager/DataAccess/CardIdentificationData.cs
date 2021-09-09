using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysTk.DataManager.Models;

namespace SysTk.DataManager.DataAccess
{
    public class CardIdentificationData
    {
        private readonly string _pathToDb;
        private SqliteDataAccess _db = new();

        public CardIdentificationData(string pathToDb)
        {
            _pathToDb = pathToDb;
        }

        public List<CardIdentificationModel> GetAllCards()
        {
            string sql = "select * from CardIdentifications";

            return _db.LoadData<CardIdentificationModel, dynamic>(sql, new { }, _pathToDb);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysTk.DataManager.Models;

namespace SysTk.DataManager.DataAccess
{
    public class CardIdentificationData : ICardIdentificationData
    {
        private readonly ISqliteDataAccess _db;

        public CardIdentificationData(ISqliteDataAccess db)
        {
            _db = db;
        }

        public List<CardIdentificationModel> GetAllCards(string dbPath)
        {
            string sql = "select * from CardIdentifications";

            return _db.LoadData<CardIdentificationModel, dynamic>(sql, new { }, dbPath);
        }
    }
}

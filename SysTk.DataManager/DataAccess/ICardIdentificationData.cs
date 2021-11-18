using System.Collections.Generic;
using SysTk.DataManager.Models;

namespace SysTk.DataManager.DataAccess
{
    public interface ICardIdentificationData
    {
        List<CardIdentificationModel> GetAllCards(string dbPath);
    }
}
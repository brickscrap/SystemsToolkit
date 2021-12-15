using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysTk.DataManager.Models;

namespace SysTk.DataManager.DataAccess
{
    public class FtpCredentialData
    {
        private readonly ISqliteDataAccess _data;

        public FtpCredentialData(ISqliteDataAccess data)
        {
            _data = data;
        }

        public List<FtpCredentialsModel> GetCredentialsBySiteId(string siteId)
        {
            List<FtpCredentialsModel> output = new();

            string sql = "select * from credentials" +
                "where Id = ";

            // TODO: Finish this
            output = _data.LoadData<FtpCredentialsModel, dynamic>("", new { }, "");

            return output;
        }
    }
}

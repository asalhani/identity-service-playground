using IdentityService.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper.FastCrud;
using System.Threading.Tasks;
using IdentityService.DataAccess.Abstract.Models;
using System.Data;

namespace IdentityService.DataAccess
{
    public class AspNetPasswordHistoryDA : BaseDataAccess<AspNetPasswordHistoryModel>, IAspNetPasswordHistoryRepository
    {
        public AspNetPasswordHistoryDA(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public List<AspNetPasswordHistoryModel> ListByUserId(string userId)
        {
            return DbConnection.Find<AspNetPasswordHistoryModel>(stmt => stmt
                .Where($"{nameof(UserMessageModel.UserId):C}=@P1")
                .WithParameters(new { P1 = userId })).ToList();
        }
    }
}

using IdentityService.DataAccess.Abstract;
using IdentityService.DataAccess.Abstract.Models;
using Dapper.FastCrud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace IdentityService.DataAccess
{
    public class UserMessageDA : BaseDataAccess<UserMessageModel>, IUserMessageRepository
    {
        public UserMessageDA(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public UserMessageModel GetByToken(string token)
        {
            return FindWhere($"InvitationToken='{token}'").FirstOrDefault();
        }

        public List<UserMessageModel> GetAllByUserId(string userId)
        {
            return DbConnection.Find<UserMessageModel>(stmt => stmt
                .Where($"UserId=@P1")
                .WithParameters(new { P1 = userId })).ToList();
        }

        public void UpdateIsCancelled(UserMessageModel item)
        {
            DbConnection.Execute(
                "UPDATE [IdentityService].[UserMessage] SET IsCancelled = @isCancelled, UpdatedBy = @updatedBy, UpdatedDate = @updatedDate WHERE Id = @id",
                new { isCancelled = item.IsCancelled, updatedBy = item.UpdatedBy, updatedDate = item.UpdatedDate, id = item.Id });
        }
    }
}

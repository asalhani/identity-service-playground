using IdentityService.DataAccess.Abstract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DataAccess.Abstract
{
    public interface IUserMessageRepository : IBaseRepository<UserMessageModel>
    {
        UserMessageModel GetByToken(string token);
        List<UserMessageModel> GetAllByUserId(string userId);
        void UpdateIsCancelled(UserMessageModel item);
    }
}

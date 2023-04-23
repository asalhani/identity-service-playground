using IdentityService.DataAccess.Abstract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DataAccess.Abstract
{
    public interface IAspNetPasswordHistoryRepository : IBaseRepository<AspNetPasswordHistoryModel>
    {
        List<AspNetPasswordHistoryModel> ListByUserId(string userId);
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DataAccess.Abstract
{
    public interface IBaseRepository<Entity>
    {
        IEnumerable<Entity> GetAll();
        Entity Get(Entity id);
        void Add(Entity entity);
        bool Delete(Entity entityId);
        bool Update(Entity prod);
        IEnumerable<Entity> FindWhere(FormattableString whereClause);
        IEnumerable<Entity> Query(string sqlQuery, object sqlParams);
    }
}

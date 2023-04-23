using Dapper;
using Dapper.FastCrud;
using IdentityService.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DataAccess
{
    public class BaseDataAccess<TableEntity> : IBaseRepository<TableEntity>
    {
        public IDbConnection DbConnection { get; }

        public BaseDataAccess(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public void Add(TableEntity entity)
        {
            DbConnection.Insert(entity);
        }
        public bool Update(TableEntity entity)
        {
            return DbConnection.Update(entity);
        }
        public bool Delete(TableEntity entityId)
        {
            return DbConnection.Delete(entityId);
        }
        public TableEntity Get(TableEntity entityId)
        {
            return DbConnection.Get(entityId);
        }
        public IEnumerable<TableEntity> Query(string sqlQuery, object sqlParams)
        {
            return DbConnection.Query<TableEntity>(sqlQuery, sqlParams);
        }
        public IEnumerable<TableEntity> GetAll()
        {
            return DbConnection.Find<TableEntity>();
        }
        public IEnumerable<TableEntity> FindWhere(FormattableString whereClause)
        {
            return DbConnection.Find<TableEntity>(entity => entity.Where(whereClause));
        }
    }
}

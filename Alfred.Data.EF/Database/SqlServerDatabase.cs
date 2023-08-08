using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Alfred.Util.Extension;

namespace Alfred.Data.EF
{
    public class SqlServerDatabase : IDatabase
    {
        #region 构造函数
        /// <summary>
        /// 构造方法
        /// </summary>
        /// 放数据访问的上下文对象
        public SqlServerDatabase(string connString)
        {
            //构造函数,构成数据库的上下文对象
            dbContext = new SqlServerDbContext(connString);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        /// 数据库上下文,由它调用数据库的各项操作
        public DbContext dbContext { get; set; }
        /// <summary>
        /// 事务对象
        /// </summary>
        public IDbContextTransaction dbContextTransaction { get; set; }
        #endregion

        #region 事务提交
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        public async Task<IDatabase> BeginTrans()
        {
            DbConnection dbConnection = dbContext.Database.GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                await dbConnection.OpenAsync();
            }
            dbContextTransaction = await dbContext.Database.BeginTransactionAsync();
            return this;
        }
        /// <summary>
        /// 提交当前操作的结果
        /// </summary>
        public async Task<int> CommitTrans()
        {
            try
            {
                //工具方法,设置实体的默认值
                DbContextExtension.SetEntityDefaultValue(dbContext);

                //调用SaveChangesAsync方法来保存更改,并将返回的影响的行数赋值给returnValue变量
                int returnValue = await dbContext.SaveChangesAsync();

                //再根据dbContextTransaction是否为空来判断是否存在数据库事务,如果存在事务,则提交事务;
                //最后再关闭数据链接
                if (dbContextTransaction != null)
                {
                    await dbContextTransaction.CommitAsync();
                    await this.Close();
                }
                else
                {
                    await this.Close();
                }
                //返回操作受影响的行数
                return returnValue;
            }
            catch
            {
                throw;
            }
            finally
            {
                //最终关闭数据连接
                if (dbContextTransaction == null)
                {
                    await this.Close();
                }
            }
        }
        /// <summary>
        /// 把当前操作回滚成未提交状态
        /// </summary>
        public async Task RollbackTrans()
        {
            await this.dbContextTransaction.RollbackAsync();
            await this.dbContextTransaction.DisposeAsync();
            await this.Close();
        }
        /// <summary>
        /// 关闭连接 内存回收
        /// </summary>
        public async Task Close()
        {
            await dbContext.DisposeAsync();
        }
        #endregion

        #region 执行 SQL 语句
        public async Task<int> ExecuteBySql(string strSql)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(strSql);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(strSql);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }
        public async Task<int> ExecuteBySql(string strSql, params DbParameter[] dbParameter)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameter);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameter);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }
        public async Task<int> ExecuteByProc(string procName)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName));
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName));
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }
        public async Task<int> ExecuteByProc(string procName, params DbParameter[] dbParameter)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName, dbParameter), dbParameter);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName, dbParameter), dbParameter);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }
        #endregion

        #region 对象实体 添加、修改、删除
        public async Task<int> Insert<T>(T entity) where T : class
        {
            dbContext.Entry<T>(entity).State = EntityState.Added;
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Insert<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Entry<T>(entity).State = EntityState.Added;
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        public async Task<int> Delete<T>() where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName));
            }
            return -1;
        }
        public async Task<int> Delete<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Set<T>().Remove(entity);
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Delete<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Set<T>().Attach(entity);
                dbContext.Set<T>().Remove(entity);
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            IEnumerable<T> entities = await dbContext.Set<T>().Where(condition).ToListAsync();
            return entities.Count() > 0 ? await Delete(entities) : 0;
        }
        public async Task<int> Delete<T>(long keyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                string keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }
        public async Task<int> Delete<T>(long[] keyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                string keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }
        public async Task<int> Delete<T>(string propertyName, long propertyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, propertyName, propertyValue));
            }
            return -1;
        }

        //异步方法,接受一个泛型参数T的实体对象,返回一个代表更新操作影响的整数.
        //方法内部使用dbContext的Attach方法将实体附加到数据库的上下文,获取实体的属性信息,并遍历每个属性
        public async Task<int> Update<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);

            //获取缓存中的中的值,如果没有还是调用数据库的值
            Hashtable props = DatabasesExtension.GetPropertyInfo<T>(entity);

            //遍历实体中的每个属性
            foreach (string item in props.Keys)
            {
                //跳过该属性,因为通常不可以修改实体的主键
                if (item == "Id")
                {
                    continue;
                }
                //获取object对象
                object value = dbContext.Entry(entity).Property(item).CurrentValue;
                //值不为空的话,将该属性标记为已修改
                if (value != null)
                {
                    //已经更改
                    dbContext.Entry(entity).Property(item).IsModified = true;
                }
            }
            //根据dbContextTransaction是否为空来决定是否提交数据库事务并返回操作影响的行数
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Update<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Entry<T>(entity).State = EntityState.Modified;
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> UpdateAllField<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            IEnumerable<T> entities = await dbContext.Set<T>().Where(condition).ToListAsync();
            return entities.Count() > 0 ? await Update(entities) : 0;
        }

        public IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return dbContext.Set<T>().Where(condition);
        }
        #endregion

        #region 对象实体 查询
        //这段代码是一异步方法,它接受一个object类型的keyValue参数,并返回一个代表找到的实体对象的泛型.
        //方法内部使用泛型参数类型和传入的keyValue值来调用数据库上下文(dbContext)的Set方法,并在该实体上调用FindAsync方法来调用查找实体对象.等待找到实体对象并返回结果
        public async Task<T> FindEntity<T>(object keyValue) where T : class
        {
            return await dbContext.Set<T>().FindAsync(keyValue);
        }
        public async Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await dbContext.Set<T>().Where(condition).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindList<T>() where T : class, new()
        {
            return await dbContext.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> FindList<T>(Func<T, object> orderby) where T : class, new()
        {
            var list = await dbContext.Set<T>().ToListAsync();
            list = list.OrderBy(orderby).ToList();
            return list;
        }
        public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await dbContext.Set<T>().Where(condition).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindList<T>(string strSql) where T : class
        {
            return await FindList<T>(strSql, null);
        }
        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter) where T : class
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                var reader = await new DbHelper(dbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
                return DatabasesExtension.IDataReaderToList<T>(reader);
            }
        }
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            var tempData = dbContext.Set<T>().AsQueryable();
            return await FindList<T>(tempData, sort, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(Expression<Func<T, bool>> condition, string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            var tempData = dbContext.Set<T>().Where(condition);
            return await FindList<T>(tempData, sort, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, string sort, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            return await FindList<T>(strSql, null, sort, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                DbHelper dbHelper = new DbHelper(dbContext, dbConnection);
                StringBuilder sb = new StringBuilder();
                sb.Append(DatabasePageExtension.SqlServerPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex));
                object tempTotal = await dbHelper.ExecuteScalarAsync(CommandType.Text, "SELECT COUNT(1) FROM (" + strSql + ") T", dbParameter);
                int total = tempTotal.ParseToInt();
                if (total > 0)
                {
                    var reader = await dbHelper.ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                    return (total, DatabasesExtension.IDataReaderToList<T>(reader));
                }
                else
                {
                    return (total, new List<T>());
                }
            }
        }
        private async Task<(int total, IEnumerable<T> list)> FindList<T>(IQueryable<T> tempData, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            tempData = DatabasesExtension.AppendSort<T>(tempData, sort, isAsc);
            var total = tempData.Count();
            if (total > 0)
            {
                tempData = tempData.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize).AsQueryable();
                var list = await tempData.ToListAsync();
                return (total, list);
            }
            else
            {
                return (total, new List<T>());
            }
        }
        #endregion

        #region 数据源查询
        public async Task<DataTable> FindTable(string strSql)
        {
            return await FindTable(strSql, null);
        }
        public async Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                var reader = await new DbHelper(dbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
                return DatabasesExtension.IDataReaderToDataTable(reader);
            }
        }
        public async Task<(int total, DataTable)> FindTable(string strSql, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            return await FindTable(strSql, null, sort, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, DataTable)> FindTable(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                DbHelper dbHelper = new DbHelper(dbContext, dbConnection);
                StringBuilder sb = new StringBuilder();
                sb.Append(DatabasePageExtension.SqlServerPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex));
                object tempTotal = await dbHelper.ExecuteScalarAsync(CommandType.Text, "SELECT COUNT(1) FROM (" + strSql + ") T", dbParameter);
                int total = tempTotal.ParseToInt();
                if (total > 0)
                {
                    var reader = await dbHelper.ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                    DataTable resultTable = DatabasesExtension.IDataReaderToDataTable(reader);
                    return (total, resultTable);
                }
                else
                {
                    return (total, new DataTable());
                }
            }
        }

        public async Task<object> FindObject(string strSql)
        {
            return await FindObject(strSql, null);
        }
        public async Task<object> FindObject(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                return await new DbHelper(dbContext, dbConnection).ExecuteScalarAsync(CommandType.Text, strSql, dbParameter);
            }
        }
        public async Task<T> FindObject<T>(string strSql) where T : class
        {
            var list = await dbContext.SqlQuery<T>(strSql);
            return list.FirstOrDefault();
        }
        #endregion
    }
}

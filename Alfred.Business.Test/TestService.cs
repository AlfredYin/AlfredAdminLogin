using Alfred.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Business.Test
{
    public class TestService : RepositoryFactory
    {
        public async Task<TestEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<TestEntity>(id);
        }

        public async Task<List<TestEntity>> GetList()
        {
            var list=await this.BaseRepository().FindList<TestEntity>();

            return list.ToList();
        }
    }
}
